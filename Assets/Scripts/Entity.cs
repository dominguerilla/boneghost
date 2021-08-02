using UnityEngine;
//using DialogueEditor;
using UnityEngine.Events;

public class EntityEvent : UnityEvent<GameObject> {

}
 public class Entity : MonoBehaviour
 {
    public EntityEvent onInteract;
    public UnityEvent onConversationEnter;
    public UnityEvent onConversationExit;

    private void Awake()
    {
        onInteract = new EntityEvent();
    }

    private void OnEnable()
    {
        /*
        ConversationManager.OnConversationStarted += OnConversationEnter;
        ConversationManager.OnConversationEnded += OnConversationExit;
        */
    }

    private void OnDisable()
    {
        /*
        ConversationManager.OnConversationStarted -= OnConversationEnter;
        ConversationManager.OnConversationEnded -= OnConversationExit;
        */
    }

    private void OnConversationEnter()
    {
        onConversationEnter.Invoke();
    }

    private void OnConversationExit()
    {
        onConversationExit.Invoke();
    }

    public void DisableAfterNextExit()
    {
        onConversationExit.AddListener(DisableAfterConversationExit);
    }

    void DisableAfterConversationExit()
    {
        onConversationExit.RemoveListener(DisableAfterConversationExit);
        this.gameObject.SetActive(false);
    }

    public void Interact(GameObject caller)
    {
        onInteract.Invoke(caller);
    }
 }