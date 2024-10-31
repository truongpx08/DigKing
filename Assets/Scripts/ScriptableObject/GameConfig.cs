using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig",
    order = 1)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private float baseMoveDuration;
    public float BaseMoveDuration => this.baseMoveDuration;
}