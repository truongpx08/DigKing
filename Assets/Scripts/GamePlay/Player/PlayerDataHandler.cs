using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Cell currentCell;
}

public class PlayerDataHandler : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    public PlayerData Data => this.data;

    public void SetCurrentCell(Cell currentCell)
    {
        this.data.currentCell = currentCell;
    }
}