using UnityEngine;
using UnityEngine.Playables;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Moving,
        Dashing,
        Attacking,
        TakingDamage,
        Dead
    }
    public State CurrentState { get; set; }
    private void Awake()
    {
        CurrentState = State.Idle;
    }
    public bool CanSwitch()
    {
        Debug.Log($"Current State: {CurrentState}");
        return CurrentState == State.Idle || CurrentState == State.Dead;
    }
}
