using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TruongSingleton<Player>
{
    [SerializeField] private PlayerInitializer initializer;
    public PlayerInitializer Initializer => this.initializer;
    [SerializeField] private PlayerMovement movement;
    public PlayerMovement Movement => movement;
    [SerializeField] private PlayerData data;
    public PlayerData Data => this.data;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadData();
        LoadMovement();
        LoadInitializer();
    }

    private void LoadData()
    {
        this.data = GetComponentInChildren<PlayerData>();
    }

    private void LoadMovement()
    {
        this.movement = GetComponentInChildren<PlayerMovement>();
    }

    private void LoadInitializer()
    {
        this.initializer = GetComponentInChildren<PlayerInitializer>();
    }
}