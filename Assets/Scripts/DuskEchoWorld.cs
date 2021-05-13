using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
/// <summary>
/// A simple blackboard for world state for the Dialogue Editor.
/// </summary>
public class DuskEchoWorld : MonoBehaviour
{
    // Gruffboy
    public bool wasDrinkingIncident;
    public bool saidDusksEcho;

    // Onna
    public bool trustsPlayer;

    // Gatekeeper
    public bool saidRecipientsName;
    public bool isGateOpen;

    public void checkDrinkingIncident()
    {
        ConversationManager.Instance.SetBool("wasDrinkingIncident", wasDrinkingIncident);
    }

    public void setDrinkingIncident(bool input)
    {
        wasDrinkingIncident = input;
    }

    /// <summary>
    /// Checks if the Gatekeeper said the name of the town
    /// </summary>
    /// <returns></returns>
    public void checkSaidDusksEcho()
    {
        ConversationManager.Instance.SetBool("saidDusksEcho", saidDusksEcho);
    }

    public void setSaidDusksEcho(bool input)
    {
        saidDusksEcho = input;
    }

    public void checkTrustsPlayer()
    {
        ConversationManager.Instance.SetBool("trustsPlayer", trustsPlayer);
    }

    public void setTrustsPlayer(bool input)
    {
        trustsPlayer = input;
    }

    public void checkSaidRecipientsName()
    {
        ConversationManager.Instance.SetBool("saidRecipientsName", saidRecipientsName);
    }

    public void setSaidRecipientsName(bool input)
    {
        saidRecipientsName = input;
    }

    public void checkIsGateOpen()
    {
        ConversationManager.Instance.SetBool("isGateOpen", isGateOpen);
    }

    public void setIsGateOpen(bool input)
    {
        isGateOpen = input;
    }
}
