using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirectionType
{
    Position2,
    Position8,
    Position4,
    Position6,
    Position1,
    Position3,
    Position7,
    Position9,
}

public class PlayerInput : TruongSingleton<PlayerInput>
{
    private Vector2 mouseStartPosition;
    private Vector2 mouseEndPosition;
    private bool isDragging;
    private const float DragThreshold = 50f; // Threshold to determine a drag  

    [SerializeField] private EDirectionType direction;
    public EDirectionType Direction => direction;

    protected override void Update()
    {
        // Check if the mouse is being pressed down  
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button  
        {
            mouseStartPosition = Input.mousePosition;
            isDragging = true;
        }

        if (isDragging)
        {
            // Update mouse position while dragging  
            mouseEndPosition = Input.mousePosition;

            // Check if the mouse button has been released  
            if (Input.GetMouseButtonUp(0))
            {
                DetectDrag();
                isDragging = false;
            }
        }
    }

    private void DetectDrag()
    {
        // Calculate drag distance  
        float dragDistanceX = mouseEndPosition.x - mouseStartPosition.x;
        float dragDistanceY = mouseEndPosition.y - mouseStartPosition.y;

        // Check drag along the X axis  
        if (Mathf.Abs(dragDistanceX) > Mathf.Abs(dragDistanceY)) // Dragging horizontally  
        {
            if (dragDistanceX > DragThreshold)
            {
                OnDragRight();
            }
            else if (dragDistanceX < -DragThreshold)
            {
                OnDragLeft();
            }
        }
        else // Dragging vertically  
        {
            if (dragDistanceY > DragThreshold)
            {
                OnDragUp();
            }
            else if (dragDistanceY < -DragThreshold)
            {
                OnDragDown();
            }
        }
    }

    private void OnDragUp()
    {
        Debug.Log("Dragged up");
        SetDirection(EDirectionType.Position2);
        Player.Instance.StateMachine.ChangeState(EPlayerState.Movement);
    }

    private void OnDragDown()
    {
        Debug.Log("Dragged down");
        SetDirection(EDirectionType.Position8);
        Player.Instance.StateMachine.ChangeState(EPlayerState.Movement);
    }

    private void OnDragLeft()
    {
        Debug.Log("Dragged left");
        SetDirection(EDirectionType.Position4);
        Player.Instance.StateMachine.ChangeState(EPlayerState.Movement);
    }

    private void OnDragRight()
    {
        Debug.Log("Dragged right");
        SetDirection(EDirectionType.Position6);
        Player.Instance.StateMachine.ChangeState(EPlayerState.Movement);
    }

    private void SetDirection(EDirectionType up)
    {
        this.direction = up;
    }
}