using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public enum EDirectionType
{
    Up,
    Down,
    Left,
    Right,
}


public class PlayerMovement : PlayerReference
{
    [SerializeField] private EDirectionType currentMoveType;
    [SerializeField] private bool isStopping;
    [SerializeField] private int remainingMoveCount;
    [SerializeField] private bool shouldReduceRemainingMoves;

    public void Move(EDirectionType newType)
    {
        if (!CanMove(newType)) return;
        this.currentMoveType = newType;
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