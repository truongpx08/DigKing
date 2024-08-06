using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public enum EPlayerMovementType
{
    Up,
    Down,
    Left,
    Right,
}


public class PlayerMovement : PlayerReference
{
    [SerializeField] private EPlayerMovementType currentMoveType;
    [SerializeField] private bool isStopping;
    [SerializeField] private int remainingMoveCount;
    [SerializeField] private bool shouldReduceRemainingMoves;

    public void Move(EPlayerMovementType newType)
    {
        if (!CanMove(newType)) return;
        this.currentMoveType = newType;
        StartMovement();
    }

    private bool CanMove(EPlayerMovementType newType)
    {
        if (isStopping) return true;
        if (this.remainingMoveCount <= 0) return false;
        if (newType == this.currentMoveType) return false;
        if (IsOppositeDirection(newType)) return false;
        return true;
    }

    private bool IsOppositeDirection(EPlayerMovementType newType)
    {
        switch (this.currentMoveType)
        {
            case EPlayerMovementType.Up:
                if (newType == EPlayerMovementType.Down) return true;
                break;
            case EPlayerMovementType.Down:
                if (newType == EPlayerMovementType.Up) return true;
                break;
            case EPlayerMovementType.Left:
                if (newType == EPlayerMovementType.Right) return true;
                break;
            case EPlayerMovementType.Right:
                if (newType == EPlayerMovementType.Left) return true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    private void StartMovement()
    {
        SetIsStopping(false);
        this.shouldReduceRemainingMoves = true;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        Cell nextCell = GetNextCell();
        if (nextCell == null || nextCell.StateMachine.CurrentState == ECellState.Disabled)
        {
            OnMovementCompleted();
            yield break; // Exit if there is no next cell  
        }

        if (shouldReduceRemainingMoves && nextCell.StateMachine.CurrentState == ECellState.Think)
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
        this.player.Data.SetCurrentCell(nextCell);
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

    private Cell GetNextCell()
    {
        var currentCell = this.player.Data.ModelData.currentCell.Data.ModelData;
        return currentMoveType switch
        {
            EPlayerMovementType.Up => currentCell.upCell,
            EPlayerMovementType.Down => currentCell.downCell,
            EPlayerMovementType.Left => currentCell.leftCell,
            EPlayerMovementType.Right => currentCell.rightCell,
            _ => throw new ArgumentOutOfRangeException(nameof(currentMoveType), currentMoveType, null)
        };
    }

    public void SetIsStopping(bool value)
    {
        this.isStopping = value;
    }
}