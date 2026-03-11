using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI GameOverscoreText;
    public TextMeshProUGUI VictoryscoreText;

    public TextMeshProUGUI highScoreText;

    float score = 0;
    float highscore = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameOverscoreText.text = score.ToString() + " POINTS";
        GameOverscoreText.text = "HIGHSCORE: " + highscore.ToString();

        VictoryscoreText.text = score.ToString() + " POINTS";
        VictoryscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(float newScore)
    {
        score += 1;
        GameOverscoreText.text = score.ToString() + " POINTS";
        VictoryscoreText.text = score.ToString() + " POINTS";
    }

    // messy but will fix. i'm guessing "savedhighscore" is the text file?"
    // implemented from youtube tutorial.
    public void UpdateHighScore()
    {
        // 1. Get the current high score (defaults to 0 if it doesn't exist yet)
        highscore = PlayerPrefs.GetFloat("SavedHighScore", 0f);

        // 2. Check if the player beat it
        if (score > highscore)
        {
            PlayerPrefs.SetFloat("SavedHighScore", score);
            PlayerPrefs.Save(); // This forces Unity to write the data to disk immediately
            highscore = score; // Update our local variable for the UI
        }

        // 3. Update the UI - You need to target the .text property!
        highScoreText.text = "Best: " + highscore.ToString();
        //GameOverscoreText.text = "Score: " + score.ToString();
    }
}
