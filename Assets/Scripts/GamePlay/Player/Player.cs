using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TruongSingleton<Player>
{
    [SerializeField] private PlayerDataHandler dataHandler;
    public PlayerDataHandler DataHandler => this.dataHandler;
    [SerializeField] private PlayerStateMachine stateMachine;
    public PlayerStateMachine StateMachine => this.stateMachine;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadData();
        LoadStateMachine();
    }

    private void LoadStateMachine()
    {
        this.stateMachine = GetComponentInChildren<PlayerStateMachine>();
    }

    private void LoadData()
    {
        this.dataHandler = GetComponentInChildren<PlayerDataHandler>();
    }
}