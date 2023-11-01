using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ArcaneSurvivorsClient;
using TMPro;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerText : MonoBehaviour, IPauseable {
    public bool IsRunning { get; set; }
    public float LeftSeconds => leftSeconds;

    private float leftSeconds;
    private TextMeshProUGUI text;

    public bool fillZeroColor;
    public Color zeroColor;

    private StringBuilder builder = new();

    public event Action TimeOut;
    public event Action TimeOutOnce;

    private void Update() {
        if (!IsRunning)
            return;

        leftSeconds -= Time.deltaTime;
        if (leftSeconds <= 0) {
            leftSeconds = 0;
            if (IsRunning) {
                IsRunning = false;
                TimeOut?.Invoke();
                TimeOutOnce?.Invoke();
                TimeOutOnce = null;
            }
        }

        UpdateText();
    }

    public void SetRunning(bool isRunning) {
        IsRunning = isRunning;
    }

    public void SetLeftSeconds(float seconds) {
        leftSeconds = seconds;
        UpdateText();
    }

    public void AddLeftSeconds(float seconds) {
        leftSeconds += seconds;
        UpdateText();
    }

    private void UpdateText() {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        int minutes = (int)(leftSeconds / 60);
        int seconds = (int)(leftSeconds % 60);
        string displayText = $"{minutes:D2}:{seconds:D2}";

        int firstNumberIndex = -1;
        for (int i = 0; i < displayText.Length; ++i) {
            char character = displayText[i];
            if (character != '0' && character != ':') {
                firstNumberIndex = i;
                break;
            }
        }


        if (fillZeroColor && firstNumberIndex > 0) {
            builder.Clear();

            for (int i = 0; i < firstNumberIndex; ++i) {
                if (displayText[i] == ':')
                    builder.Append(':');
                else
                    builder.Append('0');
            }

            displayText =
                $"<color=#{ColorUtility.ToHtmlStringRGB(zeroColor)}>{builder.ToString()}</color>{displayText.Substring(firstNumberIndex, displayText.Length - firstNumberIndex)}";
        }

        text.text = displayText;
    }
}