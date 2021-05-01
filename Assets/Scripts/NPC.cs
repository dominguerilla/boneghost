using UnityEngine;
using DialogueEditor;

 public class NPC : MonoBehaviour
 {
     public NPCConversation Conversation;

     private void OnMouseOver()
     {
         if (Input.GetMouseButtonDown(0))
         {
            ConversationManager.Instance.StartConversation(Conversation);
         }
     }
 }