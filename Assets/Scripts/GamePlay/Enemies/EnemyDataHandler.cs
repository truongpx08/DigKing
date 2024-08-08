using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public Cell currentCell;
}

public class EnemyDataHandler : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => this.data;

    public void SetCurrentCell(Cell currentCell)
    {
        this.data.currentCell = currentCell;
    }
}