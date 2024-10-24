using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Red,
    Orange,
    Yellow,
    // Green,
    // Blue,
    Indigo,
    // Violet
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EEnemyType type;
    public EEnemyType Type => type;
    [SerializeField] private EnemyStateMachine stateMachine;
    public EnemyStateMachine StateMachine => this.stateMachine;
    [SerializeField] private EnemyDataHandler dataHandler;
    public EnemyDataHandler DataHandler => this.dataHandler;
}