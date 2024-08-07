using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : TruongSingleton<Red>
{
    private RedStateMachine stateMachine;
    public RedStateMachine StateMachine => this.stateMachine;
    [SerializeField] private RedDataHandler dataHandler;
    public RedDataHandler DataHandler => this.dataHandler;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRedData();
        LoadStateMachine();
    }

    private void LoadRedData()
    {
        this.dataHandler = GetComponentInChildren<RedDataHandler>();
    }

    private void LoadStateMachine()
    {
        this.stateMachine = GetComponentInChildren<RedStateMachine>();
    }
}