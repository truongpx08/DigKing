using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
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
                EnableGo(this.cellRef.Model);
                this.cellRef.Model.color = Color.cyan;
                this.cellRef.DataHandler.Data.type = ECellType.Thin;
                break;
            case ECellState.Think:
                EnableGo(this.cellRef.Model);
                this.cellRef.Model.color = Color.white;
                this.cellRef.DataHandler.Data.type = ECellType.Thick;
                break;
            case ECellState.Disabled:
                DisableGo(this.cellRef.Model);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
}