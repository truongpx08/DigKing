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
    Think,
    Disabled,
}

public class CellStateMachine : TruongMonoBehaviour
{
    [SerializeField] private ECellState currentState;
    public ECellState CurrentState => currentState;

    private TruongStateMachine stateMachine;
    public CellInitialState InitialState { get; private set; }
    private CellThinState thinState;
    private CellThinkState thinkState;
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
            case ECellState.Think:
                if (!HasComponent<CellThinkState>())
                    this.thinkState = gameObject.AddComponent<CellThinkState>();
                stateMachine?.ChangeState(thinkState);
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
        this.cellRef.StateMachine.ChangeState(
            this.DataRef.type == ECellType.Thick
                ? ECellState.Think
                : ECellState.Thin);
    }

    private void AddName()
    {
        this.cellRef.name = $"Cell_{this.DataRef.x}_{this.DataRef.y}";
    }

    private void AddPosition()
    {
        var space = Map.Instance.Generator.Space;
        Vector3 position = new Vector3(this.DataRef.x / space, this.DataRef.y / space, 0);
        this.cellRef.transform.localPosition = position;
    }

    public void AddAdjacentCells()
    {
        this.MapGenerator.CellList.ForEach(cellItem =>
        {
            if (cellItem.IsThisCell(this.DataRef.x, this.DataRef.y + 1))
                this.DataRef.upCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x, this.DataRef.y - 1))
                this.DataRef.downCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y))
                this.DataRef.leftCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y))
                this.DataRef.rightCell = cellItem;

            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y + 1))
                this.DataRef.upLeftCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y + 1))
                this.DataRef.upRightCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y - 1))
                this.DataRef.downLeftCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y - 1))
                this.DataRef.downRightCell = cellItem;
        });
    }
}

public class CellThinState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        EnableGo(this.cellRef.Model);
        this.cellRef.Model.color = Color.cyan;
        this.cellRef.DataHandler.Data.type = ECellType.Thin;
    }
}

public class CellThinkState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        EnableGo(this.cellRef.Model);
        this.cellRef.Model.color = Color.white;
        this.cellRef.DataHandler.Data.type = ECellType.Thick;
    }
}

public class CellDisabledState : CellBaseState, IEnterState
{
    public void Enter()
    {
        LoadCellReference();

        DisableGo(this.cellRef.Model);
    }
}