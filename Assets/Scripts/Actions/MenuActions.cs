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

    bool _isPaused;
    bool _inConversation;

    public override void Register(FPSControls controls)
    {
        base.Register(controls);
        controls.Player.Menu1.performed += _ => TogglePause();
        controls.UI.Menu1.performed += _ => TogglePause();
    }

    public void TogglePause()
    {
        if (_inConversation) return;

        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;

        pauseUI.SetActive(_isPaused);

        if (_isPaused)
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

    public void Pause()
    {
        if (!_isPaused) TogglePause();
    }

    public void Unpause()
    {
        if (_isPaused) TogglePause();
    }


}