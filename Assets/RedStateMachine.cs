using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum ERedState
{
    Initial,
    Movement,
    Attack,
    Disabled,
}

public class RedStateMachine : TruongMonoBehaviour
{
    private TruongStateMachine _stateMachine;
    private RedInitialState initialState;
    private RedMovementState movementState;
    private RedDisabledState disabledState;

    public void ChangeState(ERedState nextState)
    {
        if (this._stateMachine == null)
            _stateMachine = new TruongStateMachine();
        switch (nextState)
        {
            case ERedState.Initial:
                if (!HasComponent<RedInitialState>())
                    this.initialState = gameObject.AddComponent<RedInitialState>();
                _stateMachine?.ChangeState(initialState);
                break;
            case ERedState.Movement:
                if (!HasComponent<RedMovementState>())
                    this.movementState = gameObject.AddComponent<RedMovementState>();
                _stateMachine?.ChangeState(movementState);
                break;
            case ERedState.Attack:
                break;
            case ERedState.Disabled:
                if (!HasComponent<RedDisabledState>())
                    this.disabledState = gameObject.AddComponent<RedDisabledState>();
                _stateMachine?.ChangeState(disabledState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
}


public class RedBaseState : TruongMonoBehaviour
{
    [SerializeField] protected Red red;
    protected Cell CurrentCell => this.red.DataHandler.Data.currentCell;
    protected CellData CurrentCellData => CurrentCell.DataHandler.Data;

    protected void LoadRedReference()
    {
        if (this.red == null)
            this.red = GetComponentInParent<Red>();
    }
}

public class RedInitialState : RedBaseState, IEnterState
{
    [Button]
    public void Enter()
    {
        LoadRedReference();

        var currentCell = Map.Instance.GetRandomThinCell();
        if (currentCell != null)
        {
            this.red.DataHandler.SetCurrentCell(currentCell);
            this.red.transform.position = currentCell.transform.position;
            this.red.StateMachine.ChangeState(ERedState.Movement);
        }
    }
}

public class RedMovementState : RedBaseState, IEnterState
{
    [SerializeField] private EDirectionType movementType;
    [SerializeField] private bool canMove;
    [SerializeField] private Cell nextCellToMove;

    [Button]
    public void Enter()
    {
        LoadRedReference();

        this.nextCellToMove = FindRandomThinCell();
        this.movementType = CurrentCell.GetDirection(nextCellToMove);

        this.canMove = this.nextCellToMove != null;
        if (this.canMove)
            StartCoroutine(MoveCoroutine());
    }

    private Cell FindRandomThinCell()
    {
        var cells = CurrentCell.Get4AdjacentCells().ToList();
        var thinCells = cells.FindAll(cell => cell != null && cell.StateMachine.CurrentState == ECellState.Thin);
        return thinCells.Count == 0 ? null : thinCells[Random.Range(0, thinCells.Count)];
    }

    private Cell FindRandomThinCellForNavigation()
    {
        var cells = CurrentCell.Get4AdjacentCells().ToList();
        var thinCells = cells.FindAll(cell => cell != null && cell.StateMachine.CurrentState == ECellState.Thin);
        var oppositeDirection = GetOppositeDirection(this.movementType);
        var oppositeCell = CurrentCell.GetCellWithDirection(oppositeDirection);
        if (oppositeCell != null) thinCells.Remove(oppositeCell);
        return thinCells.Count == 0 ? null : thinCells[Random.Range(0, thinCells.Count)];
    }

    private EDirectionType GetOppositeDirection(EDirectionType directionType)
    {
        return directionType switch
        {
            EDirectionType.Up => EDirectionType.Down,
            EDirectionType.Down => EDirectionType.Up,
            EDirectionType.Left => EDirectionType.Right,
            EDirectionType.Right => EDirectionType.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(directionType), directionType, null)
        };
    }

    private IEnumerator MoveCoroutine()
    {
        if (CurrentCell.StateMachine.CurrentState == ECellState.Disabled)
        {
            red.StateMachine.ChangeState(ERedState.Disabled);
            yield break;
        }

        Cell nextCell = CharacterVirtual.GetNextCellToMove(CurrentCellData, this.movementType);
        if (nextCell == null || nextCell.StateMachine.CurrentState is ECellState.Disabled or ECellState.Think)
        {
            //Navigation
            this.nextCellToMove = FindRandomThinCellForNavigation();
            this.movementType = CurrentCell.GetDirection(this.nextCellToMove);
            LoopMovement();
            yield break;
        }

        Vector3 startPosition = red.transform.position;
        Vector3 targetPosition = nextCell.transform.position;
        float duration = 0.15f; // Movement duration  
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate the new position and move based on the progress  
            red.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame  
        }

        // Ensure the final position is accurate  
        red.transform.position = targetPosition;
        this.red.DataHandler.SetCurrentCell(nextCell);

        LoopMovement();
    }

    private void LoopMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
    }
}

public class RedDisabledState : RedBaseState, IEnterState
{
    public void Enter()
    {
        LoadRedReference();
        DisableGo(red);
    }
}