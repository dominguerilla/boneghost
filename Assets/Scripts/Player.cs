using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor;

public class Player : MonoBehaviour
{
    public float interactDistance = 3f;

    public FirstPersonAIO controller;
    public GameObject pauseUI;

    Entity _currentEntity;
    bool _inConversation = false;
    bool _isPaused = false;

    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += OnConversationEnter;
        ConversationManager.OnConversationEnded += OnConversationExit;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= OnConversationEnter;
        ConversationManager.OnConversationEnded -= OnConversationExit;
    }

    private void Start()
    {
        if (!controller) controller = GetComponent<FirstPersonAIO>();
        LockCursor();
    }

    private void Update()
    {
        CheckForInteractables();
        CheckInput();
    }

    void CheckInput()
    {
        //TODO: Use the new Input System.
        if (Input.GetMouseButtonDown(0))
        {
            if (!_inConversation && _currentEntity) _currentEntity.Interact(this.gameObject);
        }
        
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (_inConversation) return;

        _isPaused = !_isPaused;

        pauseUI.SetActive(_isPaused);
        if (_isPaused) EnableMouseUIInput();
        else DisableMouseUIInput();

    }

    void CheckForInteractables()
    {
        RaycastHit hit;
        if (!_inConversation && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance))
        {
            _currentEntity = hit.transform.gameObject.GetComponent<Entity>();
        }
        else
        {
            _currentEntity = null;
        }
    }

    private void OnConversationEnter()
    {
        EnableMouseUIInput();
        _inConversation = true;
    }

    private void OnConversationExit()
    {
        DisableMouseUIInput();
        _inConversation = false;
    }

    void EnableMouseUIInput()
    {
        UnlockCursor();
        DisableCameraMovement();
        DisablePlayerMovement();
    }

    void DisableMouseUIInput()
    {
        LockCursor();
        EnableCameraMovement();
        EnablePlayerMovement();
    }

    private void EnableCameraMovement()
    {
        controller.enableCameraMovement = true;
    }

    private void DisableCameraMovement()
    {
        controller.enableCameraMovement = false;
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void EnablePlayerMovement()
    {
        controller.playerCanMove = true;
    }

    void DisablePlayerMovement()
    {
        controller.playerCanMove = false;
    }
}
