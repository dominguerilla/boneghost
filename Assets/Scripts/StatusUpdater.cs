using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Used in PlayerStatus.OnRaceChange() to update the Status UI and dialogue character portraits.
/// </summary>
public class StatusUpdater : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] DialogueActor playerActor;

    [Header("UI Elements")]
    [SerializeField] Text statusText;
    [SerializeField] Image characterPortrait;

    [Header("UI Portraits")]
    [SerializeField] Sprite boneForm;
    [SerializeField] Sprite ghostForm;
    [SerializeField] Sprite demonForm;

    [Header("Chat Portraits")]
    [SerializeField] Sprite boneFormChat;
    [SerializeField] Sprite ghostFormChat;
    [SerializeField] Sprite demonFormChat;

    public void UpdateStatusText()
    {
        string raceName = playerStatus.GetRace().name;
        statusText.text = raceName;
    }

    public void UpdateUIPortrait()
    {
        RACE race = playerStatus.GetRace().race;
        switch (race)
        {
            case RACE.GHOST:
                characterPortrait.sprite = ghostForm;
                break;
            case RACE.DEMON:
                characterPortrait.sprite = demonForm;
                break;
            case RACE.BONE:
                characterPortrait.sprite = boneForm;
                break;
            default:
                Debug.LogError($"No UI portrait set for race {race}");
                characterPortrait.sprite = boneForm;
                break;
        }
    }

    public void UpdateCharacterPortrait()
    {
        RACE race = playerStatus.GetRace().race;
        switch (race)
        {
            case RACE.GHOST:
                playerActor.spritePortrait = ghostFormChat;
                break;
            case RACE.DEMON:
                playerActor.spritePortrait = demonFormChat;
                break;
            case RACE.BONE:
                playerActor.spritePortrait = boneFormChat;
                break;
            default:
                Debug.LogError($"No character portrait set for race {race}");
                playerActor.spritePortrait = boneFormChat;
                break;
        }
    }
}
