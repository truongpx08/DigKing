using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : TruongMonoBehaviour
{
    [SerializeField] protected Player player;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayer();
    }

    private void LoadPlayer()
    {
        this.player = GetComponentInParent<Player>();
    }
}