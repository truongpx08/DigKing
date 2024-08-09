using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : PlayerReference
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        return;
        // Kiểm tra nếu vật thể va chạm có tag xác định  
        if (col.CompareTag($"Red"))
        {
            Debug.Log("Va chạm với Red!");
            player.StateMachine.ChangeState(EPlayerState.Disabled);
        }
    }
}