using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor;

public class Player : MonoBehaviour
{
    [SerializeField]
    float interactDistance = 3f;

    public FirstPersonAIO controller;

    Entity currentEntity;

    bool _inConversation = false;

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
        if (Input.GetMouseButtonDown(0))
        {
            if(!_inConversation) currentEntity.StartConversation();
        }
    }

    void CheckForInteractables()
    {
        RaycastHit hit;
        if (!_inConversation && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance))
        {
            currentEntity = hit.transform.gameObject.GetComponent<Entity>();
        }
    }

    private void OnConversationEnter()
    {
        UnlockCursor();
        DisableCameraMovement();
        DisablePlayerMovement();
        _inConversation = true;
    }

    private void OnConversationExit()
    {
        LockCursor();
        EnableCameraMovement();
        EnablePlayerMovement();
        _inConversation = false;
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
