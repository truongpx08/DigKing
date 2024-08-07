using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongStateMachine
{
    private IExitState exitState;

    public void ChangeState(object newState)
    {
        exitState?.Exit();
        exitState = newState as IExitState;
        var enterState = newState as IEnterState;
        enterState?.Enter();
    }
}

public interface IEnterState
{
    void Enter();
}

public interface IExitState
{
    void Exit();
}