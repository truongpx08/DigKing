using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float speed;
    public Cell currentCell;

    public EnemyData(float speed, Cell currentCell)
    {
        this.speed = speed;
        this.currentCell = currentCell;
    }
}

public class EnemyDataHandler : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => this.data;
    public void Constructor(EnemyData enemyData)
    {
        this.data = enemyData;
    }

    public void SetCurrentCell(Cell cell)
    {
        this.data.currentCell = cell;
    }
}