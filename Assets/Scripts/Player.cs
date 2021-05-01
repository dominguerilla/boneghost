using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor;

public class Player : MonoBehaviour
{
    public FirstPersonAIO controller;

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

    private void OnConversationEnter()
    {
        UnlockCursor();
        DisableCameraMovement();
    }

    private void OnConversationExit()
    {
        LockCursor();
        EnableCameraMovement();
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
}
