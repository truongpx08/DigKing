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
    [SerializeField] private EPlayerMovementType type;
    [SerializeField] private bool shouldStop;
    [SerializeField] private bool isStopping;
    private TweenerCore<Vector3, Vector3, VectorOptions> moveTween;

    public void Move(EPlayerMovementType newType)
    {
        if (!CanMove(newType)) return;
        this.type = newType;

        if (moveTween != null && moveTween.IsActive())
        {
            this.shouldStop = true;
            return;
        }

        StartMovement();
    }

    private bool CanMove(EPlayerMovementType newType)
    {
        if (isStopping) return true;
        if (newType == this.type) return false;
        if (IsOppositeDirection(newType)) return false;
        return true;
    }

    private bool IsOppositeDirection(EPlayerMovementType newType)
    {
        switch (this.type)
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
        this.shouldStop = false;
        this.isStopping = false;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        Cell nextCell = GetNextCell();
        if (nextCell == null)
        {
            OnMovementCompleted();
            yield break; // Exit if there is no next cell  
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
        if (!this.shouldStop)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCoroutine()); // Continue moving to the next cell  
        }
        else
            StartMovement();
    }

    private void OnMovementCompleted()
    {
        isStopping = true;
        Debug.Log("OnMovementCompleted");
    }

    private Cell GetNextCell()
    {
        var currentCell = this.player.Data.ModelData.currentCell.Data.ModelData;
        return type switch
        {
            EPlayerMovementType.Up => currentCell.upCell,
            EPlayerMovementType.Down => currentCell.downCell,
            EPlayerMovementType.Left => currentCell.leftCell,
            EPlayerMovementType.Right => currentCell.rightCell,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}