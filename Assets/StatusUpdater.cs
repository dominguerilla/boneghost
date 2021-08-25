using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusUpdater : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] Text statusText;

    public void UpdateStatusText()
    {
        string raceName = playerStatus.GetRace().name;
        statusText.text = raceName;
    }
}
