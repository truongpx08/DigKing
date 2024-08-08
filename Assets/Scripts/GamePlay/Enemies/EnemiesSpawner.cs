using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : TruongSingleton<EnemiesSpawner>
{
    [SerializeField] private EnemiesFactory factory;
    [SerializeField] private List<Enemy> enemyList;
    public List<Enemy> EnemyList => this.enemyList;

    public void SpawnEnemies()
    {
        var red = this.factory.CreateEnemy(EEnemyType.Red);
        red.StateMachine.ChangeState(EEnemyState.Initial);
        this.enemyList.Add(red);
    }
}