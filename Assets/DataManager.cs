using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : TruongSingleton<DataManager>
{
    [SerializeField] private EnemiesScriptableObject enemies;
    public EnemiesScriptableObject Enemies => this.enemies;
}