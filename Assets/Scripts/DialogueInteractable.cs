using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor;

//TODO: duplication of logic in ColorChanger
[RequireComponent(typeof(Entity))]
public class DialogueInteractable : MonoBehaviour
{

    public NPCConversation[] dialogue;

    Entity entity;
    int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        entity.onInteract.AddListener(StartConversation);

    }

    void StartConversation(GameObject caller) {
        if (dialogue != null && dialogue[currentIndex])
        {
            ConversationManager.Instance.StartConversation(dialogue[currentIndex]);
        }
        else
        {
            Debug.LogError($"No dialogue assigned to {gameObject.name}!");
        }
    }

    public void ChangeConversation(int index)
    {
        currentIndex = index;
    }

}
