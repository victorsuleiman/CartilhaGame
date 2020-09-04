using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreboard : MonoBehaviour
{

    public List<int> scores = new List<int>();
    public List<string> realNames = new List<string>();
    Cartilha cartilha;
    public bool yourTimeToShine = false;
    Transform scoreboard;
    GameObject canvas;
    Transform whoWon;
    bool tie = false;
    // Start is called before the first frame update
    void Start()
    {
        
        cartilha = FindObjectOfType<Cartilha>();
        realNames = cartilha.realNames;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (yourTimeToShine)
        {
            yourTimeToShine = false;
            displayFinalScoreboard();
        }

    }

    void displayFinalScoreboard()
    {
        string scoreText = "";
        for (int i = 0; i < scores.Count; i++)
        {
            scoreText += realNames[i] + ": " + scores[i] + "\n";
        }

        int maxValueIndex = 0;
        int maxValue = 0;
        foreach (int score in scores)
        {
            if (score > maxValue) maxValue = score;
        }
        maxValueIndex = scores.IndexOf(maxValue);

        //find if there's no tie
        int same = 0;
        foreach (int score in scores)
        {
            if (maxValue == score) same++;
        }
        if (same > 1) tie = true;

        canvas = GameObject.Find("Canvas");
        scoreboard = canvas.transform.Find("Scoreboard");
        scoreboard.GetComponent<Text>().text = scoreText;

        Transform winnerTransform = canvas.transform.Find("Winner");
        string winner = "";
        if (tie) winner = "It's a tie!";
        else winner = realNames[maxValueIndex] + " wins the game with " + maxValue + " points!";
        winnerTransform.GetComponent<Text>().text = winner;

    }
}
