using System;
using TMPro;
using UnityEngine;

public class GameEndEffect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private TMP_Text errorsText;

    public void Set(TimeSpan totalTimeTook, int errors)
    {
        timeText.text = string.Format($"{totalTimeTook.Hours}H:{totalTimeTook.Minutes}M:{totalTimeTook.Seconds}S");
        errorsText.text = string.Format($"YOU MADE {errors} ERRORS!");
    }
}
