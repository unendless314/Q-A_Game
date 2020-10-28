using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    // 控制結算面板顯示及消失

    public GameObject objectToDisable, objectToEnable;
    public bool disable = false, enable = false;

    // 控制結算面板顯示及消失

    public Text successOrFailText, RemainderText, MoneyText, ExperienceText;

    public int score;
    public int lives;

    public int questionNumbers;  //改寫
    public int wrongAnswerTimes;
    public int rightAnswerTimes;


    public float timeLimit = ScoreBoard.GetTimeLimit();
    public float[] seconds;
    public bool[] answerRecords;

    void Start()
    {
        score = ScoreBoard.GetScore();
        lives = ScoreBoard.GetLives();

        questionNumbers = ScoreBoard.GetQuestionNumbers();  //改寫
        wrongAnswerTimes = ScoreBoard.GetWrongAnswerTimes();
        rightAnswerTimes = ScoreBoard.GetRightAnswerTimes();
        timeLimit = ScoreBoard.GetTimeLimit();

        seconds = new float[questionNumbers];
        answerRecords = new bool[questionNumbers];

        for (int i = 0; i < questionNumbers; i++)
        {
            seconds[i] = ScoreBoard.GetSeconds(i);
            answerRecords[i] = ScoreBoard.GetAnswerRecords(i);
        }

        successOrFailText = GameObject.Find("SuccessOrFail").GetComponent<Text>();
        RemainderText = GameObject.Find("Remainder").GetComponent<Text>();
        MoneyText = GameObject.Find("Money").GetComponent<Text>();
        ExperienceText = GameObject.Find("Experience").GetComponent<Text>();

        
        if (rightAnswerTimes >= 4)
        {
            successOrFailText.text = "SUCCESS!";
            RemainderText.text = "恭喜!!!";
            MoneyText.text = "+" + 999999.ToString();
            ExperienceText.text = "+" + 9999.ToString();
        }
        else
        {
            successOrFailText.text = "FAIL!";
            RemainderText.text = "補考請再接再厲";
            MoneyText.text = "+" + 99.ToString();
            ExperienceText.text = "+" + 9.ToString();
        }
    }

    public void OnButtonClick()
    {
        disable = true;
        enable = true;

        if (disable)
        {
            objectToDisable.SetActive(false);
        }

        /*
        if (enable)
        {
            objectToEnable.SetActive(true);
        }
        */

    }

    void Update()
    {
        /*
        if (disabled)
        {
            objectToDisable.SetActive(false);
        }
        else
        {
            objectToDisable.SetActive(true);
        }
        */
    }
}
