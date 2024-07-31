using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 mouseStartPosition;
    private Vector2 mouseEndPosition;
    private bool isDragging = false;
    private float dragThreshold = 50f; // Threshold to determine a drag  

    private void Update()
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
            if (dragDistanceX > dragThreshold)
            {
                OnDragRight();
            }
            else if (dragDistanceX < -dragThreshold)
            {
                OnDragLeft();
            }
        }
        else // Dragging vertically  
        {
            if (dragDistanceY > dragThreshold)
            {
                OnDragUp();
            }
            else if (dragDistanceY < -dragThreshold)
            {
                OnDragDown();
            }
        }
    }

    private void OnDragUp()
    {
        Debug.Log("Dragged up");
        Player.Instance.Movement.Move(EPlayerMovementType.Up);
    }

    private void OnDragDown()
    {
        Debug.Log("Dragged down");
        Player.Instance.Movement.Move(EPlayerMovementType.Down);
    }

    private void OnDragLeft()
    {
        Debug.Log("Dragged left");
        Player.Instance.Movement.Move(EPlayerMovementType.Left);
    }

    private void OnDragRight()
    {
        Debug.Log("Dragged right");
        Player.Instance.Movement.Move(EPlayerMovementType.Right);
    }
}