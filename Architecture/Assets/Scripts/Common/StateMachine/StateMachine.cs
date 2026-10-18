using System;

public sealed class StateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState nextState)
    {
        if (nextState == null)
            throw new ArgumentNullException(nameof(nextState));

        if (ReferenceEquals(CurrentState, nextState))
            return;

        CurrentState?.Exit();

        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Tick(float deltaTime)
    {
        CurrentState?.Tick(deltaTime);
    }

    public void Clear()
    {
        IState previousState = CurrentState;
        CurrentState = null;

        previousState?.Exit();
    }
}