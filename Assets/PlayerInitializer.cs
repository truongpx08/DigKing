using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : PlayerReference
{
    public void Initialize()
    {
        var currentCell = Map.Instance.GetRandomThinCell();
        if (currentCell != null)
        {
            this.player.Data.SetCurrentCell(currentCell);
            this.player.transform.position = currentCell.transform.position;
            this.player.Movement.SetIsStopping(true);
        }
    }
}