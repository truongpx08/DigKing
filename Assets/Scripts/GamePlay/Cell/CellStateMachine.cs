using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ECellState
{
    Thin,
    Think,
    Disabled,
}

public class CellStateMachine : CellReference
{
    [SerializeField] private ECellState currentState;
    public ECellState CurrentState => currentState;

    [Button]
    public void ChangeState(ECellState nextState)
    {
        currentState = nextState;
        switch (nextState)
        {
            case ECellState.Thin:
                EnableGo(this.cell.Model);
                this.cell.Model.color = Color.cyan;
                this.cell.Data.ModelData.type = ECellType.Thin;
                break;
            case ECellState.Think:
                EnableGo(this.cell.Model);
                this.cell.Model.color = Color.white;
                this.cell.Data.ModelData.type = ECellType.Thick;
                break;
            case ECellState.Disabled:
                DisableGo(this.cell.Model);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
}