using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Enemy redPrefab;
    [SerializeField] private Enemy bluePrefab;

    public Enemy CreateEnemy(EEnemyType type)
    {
        return Instantiate(GetPrefab(type), this.container);
    }

    private Enemy GetPrefab(EEnemyType type)
    {
        return type switch
        {
            EEnemyType.Red => GetRedPrefab(),
            EEnemyType.Orange => GetBluePrefab(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private Enemy GetBluePrefab()
    {
        if (this.bluePrefab == null) this.bluePrefab = DataManager.Instance.Enemies.GetEnemy(EEnemyType.Orange).prefab;
        return this.bluePrefab;
    }

    private Enemy GetRedPrefab()
    {
        if (this.redPrefab == null) this.redPrefab = DataManager.Instance.Enemies.GetEnemy(EEnemyType.Red).prefab;
        return this.redPrefab;
    }
}