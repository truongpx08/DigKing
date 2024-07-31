using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : TruongSingleton<GamePlayManager>
{
    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    private void Initialize()
    {
        Map.Instance.Generator.GenerateMap();
        Player.Instance.Initializer.Initialize();
    }
}