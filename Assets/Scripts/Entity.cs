using UnityEngine;
using DialogueEditor;
using UnityEngine.Events;

public class EntityEvent : UnityEvent<GameObject> {

}
 public class Entity : MonoBehaviour
 {
    public EntityEvent onInteract;

    private void Awake()
    {
        onInteract = new EntityEvent();
    }

    public void Interact(GameObject caller)
    {
        onInteract.Invoke(caller);
    }
 }