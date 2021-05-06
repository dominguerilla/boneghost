using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor;

[RequireComponent(typeof(Entity))]
public class DialogueInteractable : MonoBehaviour
{
    public NPCConversation dialogue;

    Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        if (!dialogue) dialogue = GetComponent<NPCConversation>();
        entity = GetComponent<Entity>();
        entity.onInteract.AddListener(StartConversation);

    }

    void StartConversation(GameObject caller) {
        ConversationManager.Instance.StartConversation(dialogue);
    }

}
