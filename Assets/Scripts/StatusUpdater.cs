using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusUpdater : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    [Header("UI Elements")]
    [SerializeField] Text statusText;
    [SerializeField] Image characterPortrait;

    [Header("Character Portraits")]
    [SerializeField] Sprite boneForm;
    [SerializeField] Sprite ghostForm;
    [SerializeField] Sprite demonForm;

    public void UpdateStatusText()
    {
        string raceName = playerStatus.GetRace().name;
        statusText.text = raceName;
    }

    public void UpdateCharacterPortrait()
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
                Debug.LogError($"No character portrait set for race {race.ToString()}");
                characterPortrait.sprite = boneForm;
                break;
        }
    }
}
