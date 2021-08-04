using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using DialogueEditor;
public class DialogueTrigger : MonoBehaviour
{
    //public NPCConversation[] dialogues;
    public bool hasTriggered = false;
    int currentIndex = 0;

    public void TriggerDialogueOnce()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        //ConversationManager.Instance.StartConversation(dialogues[currentIndex]);
    }

    
}
