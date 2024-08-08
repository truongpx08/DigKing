using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemiesFactory : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Enemy redPrefab;

    public Enemy CreateEnemy(EEnemyType type)
    {
        return type switch
        {
            EEnemyType.Red => Instantiate(GetPrefab(type), this.container),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private Enemy GetPrefab(EEnemyType type)
    {
        return type switch
        {
            EEnemyType.Red => this.redPrefab,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}