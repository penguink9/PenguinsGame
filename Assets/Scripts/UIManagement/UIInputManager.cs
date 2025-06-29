using UnityEngine;
using System;

[DefaultExecutionOrder(-100)]
public class UIInputManager : Singleton<UIInputManager>
{
    private UIController uiControls;
    public event Action OnSettingOpen;

    protected override void Awake()
    {
        base.Awake();
        uiControls = new UIController();
    }

    private void OnEnable()
    {
        uiControls.Enable();

        // Binding cho các action UI
        uiControls.UI.OpenSetting.performed += ctx => OnSettingOpen?.Invoke();
    }
    private void OnDisable()
    {
        uiControls.Disable();
    }

}
