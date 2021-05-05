using UnityEngine;
using DialogueEditor;

 public class Entity : MonoBehaviour
 {
     public NPCConversation Conversation;

    public void StartConversation()
    {
        ConversationManager.Instance.StartConversation(Conversation);
    }
 }