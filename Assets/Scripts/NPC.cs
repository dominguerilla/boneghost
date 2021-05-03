using UnityEngine;
using DialogueEditor;

 public class NPC : MonoBehaviour
 {
     public NPCConversation Conversation;

     private void OnMouseOver()
     {
         if (!ConversationManager.Instance.IsConversationActive &&  Input.GetMouseButtonDown(0))
         {
            ConversationManager.Instance.StartConversation(Conversation);
         }
     }
 }