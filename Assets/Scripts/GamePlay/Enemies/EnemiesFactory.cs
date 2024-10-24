using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Enemy redPrefab;
    [SerializeField] private Enemy orangePrefab;
    [SerializeField] private Enemy indigoPrefab;
    [SerializeField] private Enemy yellowPrefab;

    public Enemy CreateEnemy(EEnemyType type)
    {
        var pre = GetPrefab(type);
        return Instantiate(pre, this.container);
    }

    private Enemy GetPrefab(EEnemyType type)
    {
        switch (type)
        {
            case EEnemyType.Red:
                if (this.redPrefab == null)
                    this.redPrefab = DataManager.Instance.Enemies.GetEnemy(EEnemyType.Red).prefab;
                return redPrefab;
            case EEnemyType.Orange:
                if (this.orangePrefab == null)
                    this.orangePrefab = DataManager.Instance.Enemies.GetEnemy(EEnemyType.Orange).prefab;
                return this.orangePrefab;
            case EEnemyType.Yellow:
                if (this.yellowPrefab == null)
                    this.yellowPrefab = DataManager.Instance.Enemies.GetEnemy(EEnemyType.Yellow).prefab;
                return this.yellowPrefab;
            case EEnemyType.Indigo:
                if (this.indigoPrefab == null)
                    this.indigoPrefab = DataManager.Instance.Enemies.GetEnemy(EEnemyType.Indigo).prefab;
                return this.indigoPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}