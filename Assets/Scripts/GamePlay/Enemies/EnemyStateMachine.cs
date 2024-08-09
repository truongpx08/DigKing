using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum EEnemyState
{
    Initial,
    Movement,
    Disabled,
}

public class EnemyStateMachine : TruongMonoBehaviour
{
    private TruongStateMachine stateMachine;
    private EnemyInitialState initialState;
    private EnemyMovementState movementState;
    private EnemyDisabledState disabledState;

    public void ChangeState(EEnemyState nextState)
    {
        if (this.stateMachine == null)
            stateMachine = new TruongStateMachine();
        switch (nextState)
        {
            case EEnemyState.Initial:
                if (!HasComponent<EnemyInitialState>())
                    this.initialState = gameObject.AddComponent<EnemyInitialState>();
                stateMachine?.ChangeState(initialState);
                break;
            case EEnemyState.Movement:
                if (!HasComponent<EnemyMovementState>())
                    this.movementState = gameObject.AddComponent<EnemyMovementState>();
                stateMachine?.ChangeState(movementState);
                break;
            case EEnemyState.Disabled:
                if (!HasComponent<EnemyDisabledState>())
                    this.disabledState = gameObject.AddComponent<EnemyDisabledState>();
                stateMachine?.ChangeState(disabledState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
}


public class EnemyBaseState : TruongMonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    protected Cell CurrentCell => this.enemy.DataHandler.Data.currentCell;
    protected CellData CurrentCellData => CurrentCell.DataHandler.Data;

    protected void LoadEnemyReference()
    {
        if (this.enemy == null)
            this.enemy = GetComponentInParent<Enemy>();
    }
}

public class EnemyInitialState : EnemyBaseState, IEnterState
{
    [Button]
    public void Enter()
    {
        LoadEnemyReference();

        EnableGo(enemy);
        Cell currentCell = enemy.Type switch
        {
            EEnemyType.Red => Map.Instance.GetRandomThinCellWithoutCharacter(),
            EEnemyType.Blue => Map.Instance.GetRandomThickCellWithoutCharacter(),
            _ => null
        };
        
        if (currentCell == null) return;
        this.enemy.DataHandler.SetCurrentCell(currentCell);
        this.enemy.transform.position = currentCell.transform.position;
        this.enemy.StateMachine.ChangeState(EEnemyState.Movement);
    }
}

public class EnemyMovementState : EnemyBaseState, IEnterState
{
    [SerializeField] private EDirectionType movementType;
    [SerializeField] private bool canMove;
    [SerializeField] private Cell nextCellToMove;

    [Button]
    public void Enter()
    {
        LoadEnemyReference();

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
            enemy.StateMachine.ChangeState(EEnemyState.Disabled);
            yield break;
        }

        Cell nextCell = CharacterVirtual.GetNextCellToMove(CurrentCellData, this.movementType);
        if (nextCell == null || nextCell.StateMachine.CurrentState is ECellState.Disabled or ECellState.Thick)
        {
            //Navigation
            this.nextCellToMove = FindRandomThinCellForNavigation();
            this.movementType = CurrentCell.GetDirection(this.nextCellToMove);
            LoopMovement();
            yield break;
        }

        Vector3 startPosition = enemy.transform.position;
        Vector3 targetPosition = nextCell.transform.position;
        float duration = 0.15f; // Movement duration  
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate the new position and move based on the progress  
            enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame  
        }

        // Ensure the final position is accurate  
        enemy.transform.position = targetPosition;
        this.enemy.DataHandler.SetCurrentCell(nextCell);

        LoopMovement();
    }

    private void LoopMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
    }
}

public class EnemyDisabledState : EnemyBaseState, IEnterState
{
    public void Enter()
    {
        LoadEnemyReference();
        DisableGo(enemy);
    }
}