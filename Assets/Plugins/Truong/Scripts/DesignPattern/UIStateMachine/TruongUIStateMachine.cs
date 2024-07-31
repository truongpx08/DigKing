using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongUIStateMachine : TruongMonoBehaviour
{
    [SerializeField] protected Transform defaultState;
    [SerializeField] private Transform currentState;
    public Transform CurrentState => currentState;
    private int currentStateIndex;

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        currentStateIndex = -1;
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(defaultState);
    }

    private void ChangeState(Transform stateTransform)
    {
        if (stateTransform == null) return;
        ChangeState(stateTransform.name);
    }

    [Button]
    public void ChangeState(string stateName)
    {
        // Debug.Log("Changing state to " + stateName);
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name != stateName)
            {
                DisableOtherState(child);
                continue;
            }

            // Debug.Log("State to " + stateName);
            currentStateIndex = i;
            EnableCurrentState(child);
            SetCurrentState(child);
        }
    }


    public void ChangeState(int stateIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (i != stateIndex)
            {
                DisableOtherState(child);
                continue;
            }

            currentStateIndex = i;

            EnableCurrentState(child);
            SetCurrentState(child);
        }
    }


    [Button]
    public void Next()
    {
        if (currentStateIndex < 0)
        {
            ChangeState(0);
            return;
        }

        var nextStateIndex = currentStateIndex + 1;
        if (nextStateIndex > transform.childCount - 1)
        {
            ChangeState(0);
            return;
        }

        ChangeState(nextStateIndex);
    }

    [Button]
    public void Previous()
    {
        if (currentStateIndex < 0)
        {
            ChangeState(transform.childCount - 1);
            return;
        }

        int previousStateIndex = currentStateIndex - 1;
        if (previousStateIndex < 0)
        {
            ChangeState(transform.childCount - 1);
            return;
        }

        ChangeState(previousStateIndex);
    }


    [Button]
    public void Exit()
    {
        currentStateIndex = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            DisableOtherState(child);
        }

        SetCurrentState(null);
    }

    private void SetCurrentState(Transform state)
    {
        if (state == this.currentState) return;
        var previorState = this.currentState;
        this.currentState = state;
        OnStateChanged(previorState, this.currentState);
    }


    protected virtual void OnStateChanged(Transform preState, Transform curState)
    {
        // For override
    }

    protected virtual void EnableCurrentState(Transform state)
    {
        if (!state) return;
        state.gameObject.SetActive(true);
    }

    protected virtual void DisableOtherState(Transform state)
    {
        if (!state) return;
        state.gameObject.SetActive(false);
    }

    /// <summary>
    /// Call in awake
    /// </summary>
    /// <param name="state"></param>
    protected void SetDefaultState(Transform state)
    {
        this.defaultState = state;
    }
}