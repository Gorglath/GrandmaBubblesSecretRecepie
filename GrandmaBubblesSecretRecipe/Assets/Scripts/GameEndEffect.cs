using System;
using TMPro;
using UnityEngine;

public class GameEndEffect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText;

    public void Set(TimeSpan totalTimeTook)
    {
        timeText.text = string.Format($"{totalTimeTook.Hours}H:{totalTimeTook.Minutes}M:{totalTimeTook.Seconds}S");
    }
}
