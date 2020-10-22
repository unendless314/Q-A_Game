using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    /// <summary>
    /// Model + Controller
    /// </summary>

    public int score;
    public int lives;
    public int questionNumbers;
    public int wrongAnswerTimes;
    public int rightAnswerTimes;
    public bool gameOver;

    public int databaseQuestionNumbers; //資料庫的題目總數
    public int currentQuestionNumber;   //這個是 question array 的 index 不是真正的題號

    public int[] questionIDs;
    public string[] optionsContents;    //非必要變數，現階段拿掉不影響遊戲進行 2020/10/22
    public string questionContents; //非必要變數，現階段拿掉不影響遊戲進行 2020/10/22
    public string answerContents;   //目前會用到，日後可能重構省略
    public Question[] questions;

    public int answerNumber;
    public int whatIsYourChoice;

    /// <summary>
    /// View + UI
    /// </summary>

    public Text questionContentsText;
    public Text[] optionContentsText;
    public Text scoreText;
    public Text livesText;
    public Text questionNumbersText;
    public Text wrongAnswerTimesText;
    public Text rightAnswerTimesText;
   
    // Start is called before the first frame update
    void Start()
    {
        questionContentsText = GameObject.Find("Question").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        livesText = GameObject.Find("Lives").GetComponent<Text>();
        questionNumbersText = GameObject.Find("QuestionNumber").GetComponent<Text>();
        wrongAnswerTimesText = GameObject.Find("WrongAnswer").GetComponent<Text>();
        rightAnswerTimesText = GameObject.Find("RightAnswer").GetComponent<Text>();

        optionContentsText = new Text[4];   //上限 4 選項，暫時先寫死
        optionsContents = new string[4];    //上限 4 選項，暫時先寫死
        for (int i = 0; i < optionContentsText.Length; i++)
        {
            optionContentsText[i] = GameObject.Find("Option" + (i + 1).ToString()).GetComponent<Text>();
        }

        ScoreBoard.Initialize(questionNumbers, lives);   //設定每場遊戲有幾個題目 + 幾條命
        UpdatePlayStatus();

        databaseQuestionNumbers = GameDataManager.Singleton.listQuestion.Count; //這裏要接資料庫
        currentQuestionNumber = wrongAnswerTimes + rightAnswerTimes;  //目前在第幾題 (index)

        SetQuestionIDs(questionNumbers, databaseQuestionNumbers);    //獲取問題編號陣列，questionIDs 陣列初始化結束
        SetQuestions();

        /*
         如果要動態產生選項按鈕的話，程式碼要寫在這裡
         */

        ShowQuestion(currentQuestionNumber);
        ShowOptions(currentQuestionNumber); //我的答案好像永遠是最後一個沒有換位置，只有錯誤選項換過?
        ShowAnswer(currentQuestionNumber);

        //   從這裡切開   //

        //CheckAnswer(currentQuestionNumber);
        ShowScoreBoard();

    }

    void Update()
    {
        if (gameOver)
        {
            ShowScoreBoard();
        }
        else
        {
            //   確定可以用鍵盤操控法，接下來用按鈕操控法   //

            if (Input.GetKeyDown(KeyCode.Space))
            {
                questions[currentQuestionNumber].FindAnswerNumber(4);   //上限 4 選項，暫時先寫死
                answerNumber = questions[currentQuestionNumber].answerNumber;
                whatIsYourChoice = answerNumber;
                CheckAnswer(currentQuestionNumber);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                whatIsYourChoice = 111;
                CheckAnswer(currentQuestionNumber);
            }
        }
    }

    public void CheckIsGameOver()
    {
        UpdatePlayStatus();
        currentQuestionNumber = wrongAnswerTimes + rightAnswerTimes;

        if (lives <= 0 || wrongAnswerTimes >= 3)
        {
            ScoreBoard.SetGameover();   //計分板設定遊戲狀態為結束
            gameOver = ScoreBoard.IsGameOver();
            Debug.Log("遊戲結束");
        }
        else if (currentQuestionNumber >= questionNumbers)    //index 的計算要減 1
        {
            ScoreBoard.SetGameover();   //計分板設定遊戲狀態為結束
            gameOver = ScoreBoard.IsGameOver();
            Debug.Log("遊戲結束");
        }
        else
        {
            Debug.Log("遊戲進行中...");
        }
    }

    public void UpdatePlayStatus()
    {
        score = ScoreBoard.GetScore();
        lives = ScoreBoard.GetLives();
        questionNumbers = ScoreBoard.GetQuestionNumbers();
        wrongAnswerTimes = ScoreBoard.GetWrongAnswerTimes();
        rightAnswerTimes = ScoreBoard.GetRightAnswerTimes();
        gameOver = ScoreBoard.IsGameOver();
    }

    public void SetQuestionIDs(int questionNumbers, int databaseQuestionNumbers)  //預設取幾道題目，總題庫量多少題，這裏要接資料庫
    {
        questionIDs = new int[questionNumbers];
        questions = new Question[questionNumbers];

        for (int i = 0; i < questionNumbers; i++)
        {
            // 因為極大值會超出 index 範圍，所以取餘數，讓極大值發生時視同為 0;
            int randomQuestionID = ((int)UnityEngine.Random.Range(1, databaseQuestionNumbers)) % databaseQuestionNumbers;

            for (int j = 0; j < i; j++)
            {
                while (questionIDs[j] == randomQuestionID)
                {
                    randomQuestionID = (int)UnityEngine.Random.Range(1, databaseQuestionNumbers);
                    j = 0;
                }
            }

        questionIDs[i] = randomQuestionID;

        }
    }

    public void SetQuestions()
    {
        for (int i = 0; i < questionIDs.Length; i++)
        {
            questions[i] = GameDataManager.Singleton.listQuestion[questionIDs[i]];
        }
    }
    
    public void ShowQuestion(int counter)  //這裡要開啟資料庫連線
    {
        if (counter < questionNumbers)
        {
            questionContents = questions[counter].questionContents;
            questionContentsText.text = "問題" + currentQuestionNumber.ToString() + ": " + questions[counter].questionContents;
        }
    }

    public void ShowOptions(int counter)    //這裡要開啟資料庫連線
    {
        if (counter < questionNumbers)
        {
            int optionNumbers = questions[counter].optionContents.Length;
            questions[counter].optionNumbers = optionNumbers;
            questions[counter].Permutation(optionNumbers);

            for (int i = 0; i < optionNumbers; i++)
            {
                optionContentsText[i].text = AddPrefix(i) + questions[counter].optionContents[i];
                optionsContents[i] = questions[counter].optionContents[i];
            }
        }
    }

    private string AddPrefix(int i)
    {
        switch (i)
        {
            case 0:
                return "(A) ";
            case 1:
                return "(B) ";
            case 2:
                return "(C) ";
            case 3:
                return "(D) ";
            default:
                return "(隱藏選項) ";
        }
    }

    public void ShowAnswer(int counter)    //這裡要開啟資料庫連線，未必用得到
    {
        if (counter < questionNumbers)
        {
            answerContents = questions[counter].answerContents;
        }
    }

    public void MakeYourChoice(int youChooseNumber)
    {
        whatIsYourChoice = youChooseNumber;

        CheckAnswer(currentQuestionNumber);

    }

    public void CheckAnswer(int counter)
    {
        if (counter < questionNumbers)
        {
            questions[counter].FindAnswerNumber(4);   //上限 4 選項，暫時先寫死
            answerNumber = questions[counter].answerNumber;

            Debug.Log("答案是" + answerNumber + ": " + questions[counter].answerContents);
            Debug.Log("我選的是" + whatIsYourChoice.ToString() + ": " + questions[counter].optionContents[whatIsYourChoice]);

            if (whatIsYourChoice == answerNumber)
            {
                Debug.Log("回答正確");

                ScoreBoard.AnswerRight(100);
                CheckIsGameOver();
                ShowQuestion(currentQuestionNumber);
                ShowOptions(currentQuestionNumber);
                ShowAnswer(currentQuestionNumber);
                ShowScoreBoard();

            }
            else
            {
                Debug.Log("回答錯誤");

                ScoreBoard.AnswerWrong(100);
                CheckIsGameOver();
                ShowQuestion(currentQuestionNumber);
                ShowOptions(currentQuestionNumber);
                ShowAnswer(currentQuestionNumber);
                ShowScoreBoard();

            }
        }
    }
    
    private void ShowScoreBoard()
    {
        scoreText.text = "目前的分數: " + score.ToString();
        livesText.text = "剩餘生命數: " + lives.ToString();
        wrongAnswerTimesText.text = "答錯次數: " + wrongAnswerTimes.ToString();
        rightAnswerTimesText.text = "答對次數: " + rightAnswerTimes.ToString();

        if (currentQuestionNumber == questionNumbers)
        {
            questionNumbersText.text = "目前題號: " + questionNumbers.ToString();
        }
        else
        {
            if (gameOver)
            {
                questionNumbersText.text = "目前題號: " + currentQuestionNumber.ToString();
            }
            else
            {
                questionNumbersText.text = "目前題號: " + (currentQuestionNumber + 1).ToString();
            }
        }
    }
}
