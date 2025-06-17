using System;
using UnityEngine;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    private PlayerController playerControls;

    // Các sự kiện public
    public event Action OnAttack;
    public event Action OnDash;
    public event Action OnHeal;
    public event Action OnInteract;
    public event Action<int> OnCharacterSelect;

    public Vector2 MoveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerController();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        // Binding cho các action combat
        playerControls.Combat.Attack.performed += ctx => OnAttack?.Invoke();
        playerControls.Combat.Dash.performed += ctx => OnDash?.Invoke();
        playerControls.Combat.Heal.performed += ctx => OnHeal?.Invoke();
        playerControls.Combat.Interact.performed += ctx => OnInteract?.Invoke();

        // Binding cho move
        playerControls.Movement.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        playerControls.Movement.Move.canceled += ctx => MoveInput = Vector2.zero;
        //Binding cho character selection
        playerControls.SelectChar.Keyboard.performed += ctx => OnCharacterSelect?.Invoke((int)ctx.ReadValue<float>());
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}

