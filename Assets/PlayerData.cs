using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModelData
{
    public Cell currentCell;
}

public class PlayerData : MonoBehaviour
{
    [SerializeField] private PlayerModelData modelData;
    public PlayerModelData ModelData => this.modelData;

    public void SetCurrentCell(Cell currentCell)
    {
        this.modelData.currentCell = currentCell;
    }
}