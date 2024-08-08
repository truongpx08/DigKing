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
        Map.Instance.Generator.Initial();
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        Player.Instance.StateMachine.ChangeState(EPlayerState.Initial);
        EnemiesSpawner.Instance.SpawnEnemies();
    }
}