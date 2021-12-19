using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculator : MonoBehaviour
{
    public GameObject endScreen;
    public GameObject flood;

    string totalText = "Total: ";
    string timerText = "Time Left Over: ";
    string wallText = "Wall Pipes Fixed: ";
    string toiletText = "Toilets Fixed: ";

    Text totalScoreText;
    Text timerScoreText;
    Text wallScoreText;
    Text toiletScoreText;
    Text winLose;

    public float timerScore;
    public int wallScore;
    public int toiletScore;
    public float waterMulti;

    public bool win;

    private int scorePower = 5;

    private void Start()
    {
        totalScoreText = endScreen.transform.GetChild(0).GetComponent<Text>();
        timerScoreText = endScreen.transform.GetChild(1).GetComponent<Text>();
        wallScoreText = endScreen.transform.GetChild(2).GetComponent<Text>();
        toiletScoreText = endScreen.transform.GetChild(3).GetComponent<Text>();
        winLose = endScreen.transform.GetChild(6).GetComponent<Text>();


    }

    float waterMultiplyer()
    {
        return Mathf.RoundToInt(100 - ((waterMulti * 100) / flood.GetComponent<FloodScript>().endSizeY));
    }

    float calcTotalScore()
    {
        return ((wallScore * scorePower) + (toiletScore * scorePower * 2)) + waterMultiplyer();
    }

    public void updateScore()
    {

        waterMulti = GameObject.Find("Flood").gameObject.transform.localScale.y;

        totalScoreText.text = totalText + Mathf.RoundToInt(calcTotalScore()).ToString();
        wallScoreText.text = wallText + wallScore.ToString() + " * 5 = " + (wallScore * scorePower).ToString() + " points";
        toiletScoreText.text = toiletText + toiletScore.ToString() + " * 10 = " + (toiletScore * scorePower).ToString() + " points";

        if (win)
        {
            winLose.text = "You Survived";

            timerScoreText.text = "Water Score: " + waterMultiplyer().ToString() + " % not filled = " + waterMultiplyer().ToString() + " points";
        }
        else if (!win)
        {
            winLose.text = "You did not Survive";

            timerScoreText.text = timerText + Mathf.RoundToInt(timerScore).ToString() + " seconds";
        }
    }



}
