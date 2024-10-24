using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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
                this.initialState ??= gameObject.AddComponent<EnemyInitialState>();
                stateMachine?.ChangeState(initialState);
                break;
            case EEnemyState.Movement:
                this.movementState ??= gameObject.AddComponent<EnemyMovementState>();
                stateMachine?.ChangeState(movementState);
                break;
            case EEnemyState.Disabled:
                this.disabledState ??= gameObject.AddComponent<EnemyDisabledState>();
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
            EEnemyType.Orange => Map.Instance.GetRandomThickCellWithoutCharacter(),
            EEnemyType.Yellow => Map.Instance.GetRandomThickCellWithoutCharacter(),
            EEnemyType.Indigo => Map.Instance.GetRandomThickCellWithoutCharacter(),
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
    private IMovementStrategy movementStrategy;

    public void Enter()
    {
        LoadEnemyReference();

        switch (enemy.Type)
        {
            case EEnemyType.Red:
                if (!HasComponent<IMovementStrategy>())
                    this.movementStrategy = gameObject.AddComponent<BorderMovement>();
                break;
            case EEnemyType.Orange:
                if (!HasComponent<IMovementStrategy>())
                    this.movementStrategy = gameObject.AddComponent<PingPongMovement>();
                break;
            case EEnemyType.Yellow:
                if (!HasComponent<IMovementStrategy>())
                    this.movementStrategy = gameObject.AddComponent<PopOut1357Movement>();
                break;
            case EEnemyType.Indigo:
                if (!HasComponent<IMovementStrategy>())
                    this.movementStrategy = gameObject.AddComponent<FourDirectionMovement2468>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        movementStrategy.Move();
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