using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesScriptableObject", menuName = "ScriptableObjects/EnemiesScriptableObject",
    order = 1)]
public class EnemiesScriptableObject : ScriptableObject
{
    [SerializeField] private List<EnemyModelData> enemies;

    public EnemyModelData GetEnemy(EEnemyType type)
    {
        return enemies.Find(enemy => enemy.type == type);
    }
}

[Serializable]
public class EnemyModelData
{
    public EEnemyType type;
    public Enemy prefab;
}