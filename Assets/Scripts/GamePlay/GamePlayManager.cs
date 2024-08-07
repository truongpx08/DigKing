using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GamePlayManager : TruongSingleton<GamePlayManager>
{
    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    [Button]
    public void Initialize()
    {
        Map.Instance.Generator.GenerateMap();
        Player.Instance.StateMachine.ChangeState(EPlayerState.Initial);
        Red.Instance.StateMachine.ChangeState(ERedState.Initial);
    }
}