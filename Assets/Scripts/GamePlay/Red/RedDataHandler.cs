using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RedData
{
    public Cell currentCell;
}

public class RedDataHandler : MonoBehaviour
{
    [SerializeField] private RedData data;
    public RedData Data => this.data;

    public void SetCurrentCell(Cell currentCell)
    {
        this.data.currentCell = currentCell;
    }
}