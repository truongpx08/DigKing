using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovementStrategy : MonoBehaviour
{
}

// Interface định nghĩa chiến lược di chuyển  
interface IMovementStrategy
{
    void Move();
}

// Base cho các chiến lược di chuyển  
public class BaseMovementStrategy : TruongMonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    protected Cell CurrentCell => this.enemy.DataHandler.Data.currentCell;
    protected CellData CurrentCellData => CurrentCell.DataHandler.Data;

    protected EDirectionType direction;

    protected void LoadEnemyReference()
    {
        if (this.enemy == null)
            this.enemy = GetComponentInParent<Enemy>();
    }

    protected bool IsCellDisabled(Cell cell)
    {
        return cell.StateMachine.CurrentState == ECellState.Disabled;
    }

    protected IEnumerator MoveToCell(Cell nextCell, float duration)
    {
        Vector3 startPosition = enemy.transform.position;
        Vector3 targetPosition = nextCell.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Đợi đến khung hình tiếp theo  
        }

        enemy.transform.position = targetPosition;
        this.enemy.DataHandler.SetCurrentCell(nextCell);
    }

    protected Cell FindRandomThinCell()
    {
        var cells = CurrentCell.Get4AdjacentCells().ToList();
        var thinCells = cells.FindAll(cell => cell != null && cell.StateMachine.CurrentState == ECellState.Thin);
        return thinCells.Count == 0 ? null : thinCells[Random.Range(0, thinCells.Count)];
    }

    protected Cell FindRandomCell()
    {
        var cells = CurrentCell.Get4AdjacentCells().ToList();
        return cells.Count == 0 ? null : cells[Random.Range(0, cells.Count)];
    }

    protected Cell FindRandomThinCellForNavigation()
    {
        var cells = CurrentCell.Get4AdjacentCells().ToList();
        var thinCells = cells.FindAll(cell => cell != null && cell.StateMachine.CurrentState == ECellState.Thin);
        var oppositeDirection = DirectionUtils.GetOppositeDirection(direction);
        var oppositeCell = CurrentCell.GetCellWithDirection(oppositeDirection);

        if (oppositeCell != null) thinCells.Remove(oppositeCell);
        return thinCells.Count == 0 ? null : thinCells[Random.Range(0, thinCells.Count)];
    }

    [Button]
    protected Cell FindRandomCellForNavigation()
    {
        var cells = CurrentCell.Get4AdjacentCells().ToList();

        var oppositeDirection = DirectionUtils.GetOppositeDirection(direction);
        var oppositeCell = CurrentCell.GetCellWithDirection(oppositeDirection);
        if (oppositeCell != null) cells.Remove(oppositeCell);

        var sameDirectionCell = CurrentCell.GetCellWithDirection(direction);
        cells.Remove(sameDirectionCell);

        if (cells.Contains(null)) cells.Remove(null);
        if (cells.Contains(null)) cells.Remove(null);
        var result = cells.Count == 0 ? null : cells[Random.Range(0, cells.Count)];
        return result;
    }
}

// Chiến lược di chuyển quanh các biên  
public class BorderMovement : BaseMovementStrategy, IMovementStrategy
{
    private Cell nextCellToMove;

    public void Move()
    {
        LoadEnemyReference();
        nextCellToMove = FindRandomThinCell();
        direction = CurrentCell.GetDirection(nextCellToMove);

        if (nextCellToMove != null)
            StartCoroutine(MoveCoroutine());
    }


    private IEnumerator MoveCoroutine()
    {
        if (IsCellDisabled(CurrentCell))
        {
            enemy.StateMachine.ChangeState(EEnemyState.Disabled);
            yield break;
        }

        Cell nextCell = CharacterUtils.GetNextCellToMove(CurrentCellData, direction);
        if (nextCell == null || nextCell.StateMachine.CurrentState is ECellState.Disabled or ECellState.Thick)
        {
            nextCellToMove = FindRandomThinCellForNavigation();
            direction = CurrentCell.GetDirection(nextCellToMove);
            LoopMovement();
            yield break;
        }

        yield return MoveToCell(nextCell, 0.15f);
        LoopMovement();
    }


    private void LoopMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
    }
}

// Chiến lược di chuyển bật đi bật lại  
public class PingPongMovement : BaseMovementStrategy, IMovementStrategy
{
    public void Move()
    {
        LoadEnemyReference();

        direction = CurrentCell.GetRandomDirection();
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        if (IsCellDisabled(CurrentCell))
        {
            enemy.StateMachine.ChangeState(EEnemyState.Disabled);
            yield break;
        }

        Cell nextCell = CharacterUtils.GetNextCellToMove(CurrentCellData, direction);
        if (nextCell == null || nextCell.StateMachine.CurrentState is ECellState.Disabled)
        {
            direction = DirectionUtils.GetOppositeDirection(direction);
            LoopMovement();
            yield break;
        }

        yield return MoveToCell(nextCell, 0.2f);
        LoopMovement();
    }

    private void LoopMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
    }
}

public class FourDirectionMovement : BaseMovementStrategy, IMovementStrategy
{
    private Cell nextCellToMove;
    private int moveCount;

    public void Move()
    {
        LoadEnemyReference();
        this.nextCellToMove = FindRandomCell();
        direction = CurrentCell.GetDirection(nextCellToMove);

        if (nextCellToMove != null)
            StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        this.moveCount++;
        if (IsCellDisabled(CurrentCell))
        {
            enemy.StateMachine.ChangeState(EEnemyState.Disabled);
            yield break;
        }

        Cell nextCell = CharacterUtils.GetNextCellToMove(CurrentCellData, direction);
        if (nextCell == null || nextCell.StateMachine.CurrentState is ECellState.Disabled || this.moveCount == 8)
        {
            nextCellToMove = FindRandomCellForNavigation();
            direction = CurrentCell.GetDirection(nextCellToMove);
            LoopMovement();
            this.moveCount = 0;
            yield break;
        }

        yield return MoveToCell(nextCell, 0.15f);
        LoopMovement();
    }

    private void LoopMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
    }
}