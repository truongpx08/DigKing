using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[Serializable]
public enum EPlayerState
{
    Initial,
    Movement,
    Disabled,
}

public class PlayerStateMachine : TruongMonoBehaviour
{
    private TruongStateMachine stateMachine;
    private PlayerInitialState initialState;
    public PlayerMovementState MovementState { get; private set; }
    private PlayerDisabledState disabledState;

    public void ChangeState(EPlayerState nextState)
    {
        if (this.stateMachine == null)
            stateMachine = new TruongStateMachine();
        switch (nextState)
        {
            case EPlayerState.Initial:
                if (!HasComponent<PlayerInitialState>())
                    this.initialState = gameObject.AddComponent<PlayerInitialState>();
                stateMachine?.ChangeState(initialState);
                break;
            case EPlayerState.Movement:
                stateMachine?.ChangeState(this.MovementState);
                break;
            case EPlayerState.Disabled:
                if (!HasComponent<PlayerDisabledState>())
                    this.disabledState = gameObject.AddComponent<PlayerDisabledState>();
                stateMachine?.ChangeState(disabledState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }


    public void SetMovementState()
    {
        if (this.MovementState != null) return;

        if (HasComponent<PlayerDisabledState>())
        {
            this.MovementState = gameObject.GetComponent<PlayerMovementState>();
            return;
        }

        this.MovementState = gameObject.AddComponent<PlayerMovementState>();
    }
}

public class PlayerBaseState : TruongMonoBehaviour
{
    [SerializeField] protected Player player;

    protected void LoadPlayerReference()
    {
        if (this.player == null)
            this.player = GetComponentInParent<Player>();
    }
}

public class PlayerInitialState : PlayerBaseState, IEnterState
{
    [Button]
    public void Enter()
    {
        LoadPlayerReference();

        EnableGo(player);

        var currentCell = Map.Instance.GetRandomThinCellWithoutCharacter();
        if (currentCell != null)
        {
            this.player.DataHandler.SetCurrentCell(currentCell);
            this.player.transform.position = currentCell.transform.position;

            this.player.StateMachine.SetMovementState();
            this.player.StateMachine.MovementState.SetIsStopping(true);
            this.player.StateMachine.MovementState.ResetRemainingMoveCount();
        }
    }
}

public class PlayerMovementState : PlayerBaseState, IEnterState
{
    [SerializeField] private EDirectionType currentMoveType;
    [SerializeField] private bool isStopping;
    [SerializeField] private int remainingMoveCount;
    [SerializeField] private bool shouldReduceRemainingMoves;

    public void Enter()
    {
        LoadPlayerReference();

        var direction = PlayerInput.Instance.Direction;
        if (!CanMove(direction)) return;
        this.currentMoveType = direction;
        StartMovement();
    }

    private bool CanMove(EDirectionType newType)
    {
        if (isStopping) return true;
        if (this.remainingMoveCount <= 0) return false;
        if (newType == this.currentMoveType) return false;
        if (CharacterVirtual.IsOppositeDirection(newType, this.currentMoveType)) return false;
        return true;
    }


    private void StartMovement()
    {
        SetIsStopping(false);
        this.shouldReduceRemainingMoves = true;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        Cell nextCell = CharacterVirtual.GetNextCellToMove(this.player.DataHandler.Data.currentCell.DataHandler.Data,
            this.currentMoveType);
        if (nextCell == null || nextCell.StateMachine.CurrentState == ECellState.Disabled)
        {
            OnMovementCompleted();
            yield break; // Exit if there is no next cell  
        }

        if (shouldReduceRemainingMoves && nextCell.StateMachine.CurrentState == ECellState.Thick)
        {
            this.shouldReduceRemainingMoves = false;

            this.remainingMoveCount--;
        }

        Vector3 startPosition = player.transform.position;
        Vector3 targetPosition = nextCell.transform.position;
        float duration = 0.25f; // Movement duration  
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate the new position and move based on the progress  
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame  
        }

        // Ensure the final position is accurate  
        player.transform.position = targetPosition;
        this.player.DataHandler.SetCurrentCell(nextCell);
        nextCell.StateMachine.ChangeState(ECellState.Thin);

        //Loop Movement
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
    }

    private void OnMovementCompleted()
    {
        SetIsStopping(true);
        ResetRemainingMoveCount();
        Debug.Log("OnMovementCompleted");
        Map.Instance.Collapse.Collapse();
    }

    public void ResetRemainingMoveCount()
    {
        this.remainingMoveCount = 3;
    }

    public void SetIsStopping(bool value)
    {
        this.isStopping = value;
    }
}

public class PlayerDisabledState : PlayerBaseState, IEnterState
{
    public void Enter()
    {
        LoadPlayerReference();

        DisableGo(player);
        GamePlayManager.Instance.Initialize();
    }
}