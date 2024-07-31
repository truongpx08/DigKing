using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ECellState
{
    Thin,
    Think,
}

public class CellStateMachine : CellReference
{
    [Button]
    public void ChangeState(ECellState state)
    {
        switch (state)
        {
            case ECellState.Thin:
                this.cell.Model.color = Color.cyan;
                this.cell.Data.ModelData.type = ECellType.Thin;
                break;
            case ECellState.Think:
                this.cell.Model.color = Color.white;
                this.cell.Data.ModelData.type = ECellType.Thick;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}