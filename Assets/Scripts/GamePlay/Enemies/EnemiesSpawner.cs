using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesSpawner : TruongSingleton<EnemiesSpawner>
{
    [SerializeField] private EnemiesFactory factory;
    [SerializeField] private List<Enemy> enemyList;
    public List<Enemy> EnemyList => this.enemyList;

    public void SpawnEnemies()
    {
        int numberEnemies = 10;
        // int numberEnemies = 1;
        for (int i = 0; i < numberEnemies; i++)
        {
            SpawnOneEnemy();
        }
    }

    private void SpawnOneEnemy()
    {
        var enemyTypeList = Enum.GetNames(typeof(EEnemyType));
        var enemyType = enemyTypeList[Random.Range(0, enemyTypeList.Length)];
        // var enemyType = EEnemyType.Yellow.ToString();
        var enemy = this.factory.CreateEnemy(Enum.Parse<EEnemyType>(enemyType));
        enemy.StateMachine.ChangeState(EEnemyState.Initial);
        this.enemyList.Add(enemy);
    }
}