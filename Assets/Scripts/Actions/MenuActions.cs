using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Mango.Actions;

public class MenuActions : PlayerAction
{

    public UnityEvent onPause;
    public UnityEvent onUnpause;

    [SerializeField]
    GameObject pauseUI;

    bool isPaused;
    bool inConversation;
    bool canPause = true;

    public override void Register(FPSControls controls)
    {
        base.Register(controls);
        controls.Player.Menu1.performed += TogglePause;
        controls.UI.Menu1.performed += TogglePause;
    }

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (inConversation) return;
        if (!isPaused && !canPause) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        pauseUI.SetActive(isPaused);

        if (isPaused)
        {
            controls.Player.Disable();
            controls.UI.Enable();
            onPause.Invoke();
        }
        else
        {
            controls.UI.Disable();
            controls.Player.Enable();
            onUnpause.Invoke();
        }

    }

    // Dumb hack I have to do in order to hook up Unity UI buttons to this function
    public void TogglePause()
    {
        TogglePause(new InputAction.CallbackContext());
    }

    public void SetCanPause(bool value)
    {
        canPause = value;
    }

    private void OnDestroy()
    {
        controls.Player.Menu1.performed -= TogglePause;
        controls.UI.Menu1.performed -= TogglePause;
    }

}
