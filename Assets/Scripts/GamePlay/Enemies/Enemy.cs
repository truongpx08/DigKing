using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Red,
    Blue,
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EEnemyType type;
    [SerializeField] private EnemyStateMachine stateMachine;
    public EnemyStateMachine StateMachine => this.stateMachine;
    [SerializeField] private EnemyDataHandler dataHandler;
    public EnemyDataHandler DataHandler => this.dataHandler;
}