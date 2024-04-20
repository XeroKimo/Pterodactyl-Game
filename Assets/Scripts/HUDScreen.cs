using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Button retryButton;
    public TextMeshProUGUI pressStartText;

    public void SetScoreText(int currentScore, int? highScore)
    {
        string text = string.Empty;
        if(highScore.HasValue)
            text = "<color=#989898>HI " + highScore.Value.ToString("D6") + "</color> ";

        text += currentScore.ToString("D6");
        scoreText.text = text;
    }
}
