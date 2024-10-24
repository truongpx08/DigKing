using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public enum ECellState
{
    Initial,
    Thin,
    Thick,
    Disabled,
}

public class CellStateMachine : TruongMonoBehaviour
{
    [SerializeField] private ECellState currentState;
    public ECellState CurrentState => currentState;

    private TruongStateMachine stateMachine;
    public CellInitialState InitialState { get; private set; }
    private CellThinState thinState;
    private CellThickState thickState;
    private CellDisabledState disabledState;


    [Button]
    public void ChangeState(ECellState nextState)
    {
        if (this.stateMachine == null)
            stateMachine = new TruongStateMachine();
        currentState = nextState;
        switch (nextState)
        {
            case ECellState.Initial:
                if (!HasComponent<CellInitialState>())
                    this.InitialState = gameObject.AddComponent<CellInitialState>();
                stateMachine?.ChangeState(InitialState);
                break;
            case ECellState.Thin:
                if (!HasComponent<CellThinState>())
                    this.thinState = gameObject.AddComponent<CellThinState>();
                stateMachine?.ChangeState(thinState);
                break;
            case ECellState.Thick:
                if (!HasComponent<CellThickState>())
                    this.thickState = gameObject.AddComponent<CellThickState>();
                stateMachine?.ChangeState(thickState);
                break;
            case ECellState.Disabled:
                if (!HasComponent<CellDisabledState>())
                    this.disabledState = gameObject.AddComponent<CellDisabledState>();
                stateMachine?.ChangeState(this.disabledState);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
}


public class CellBaseState : CellReference
{
}


public class CellInitialState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        AddPosition();
        AddName();
        var nextState = GetNextState(DataRef.x, DataRef.y);
        this.cell.StateMachine.ChangeState(nextState);
    }

    private ECellState GetNextState(int x, int y)
    {
        if (x == 0)
            return ECellState.Thin;
        if (x == MapGenerator.Width - 1)
            return ECellState.Thin;
        if (y == 0)
            return ECellState.Thin;
        if (y == MapGenerator.Height - 1)
            return ECellState.Thin;
        return ECellState.Thick;
    }


    private void AddName()
    {
        this.cell.name = $"Cell_{this.DataRef.x}_{this.DataRef.y}";
    }

    private void AddPosition()
    {
        var space = Map.Instance.Generator.Space;
        Vector3 position = new Vector3(this.DataRef.x / space, this.DataRef.y / space, 0);
        this.cell.transform.localPosition = position;
    }

    public void AddAdjacentCells()
    {
        this.MapGenerator.CellList.ForEach(cellItem =>
        {
            if (cellItem.IsThisCell(this.DataRef.x, this.DataRef.y + 1))
                this.DataRef.cellPosition2 = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x, this.DataRef.y - 1))
                this.DataRef.cellPosition8 = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y))
                this.DataRef.cellPosition4 = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y))
                this.DataRef.cellPosition6 = cellItem;

            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y + 1))
                this.DataRef.cellPosition1 = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y + 1))
                this.DataRef.cellPosition3 = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y - 1))
                this.DataRef.cellPosition7 = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y - 1))
                this.DataRef.cellPosition9 = cellItem;
        });
    }
}

public class CellThinState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        EnableGo(this.cell.Model);
        this.cell.Model.color = Color.cyan;
    }
}

public class CellThickState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        EnableGo(this.cell.Model);
        this.cell.Model.color = Color.white;
    }
}

public class CellDisabledState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        DisableGo(this.cell.Model);
    }
}