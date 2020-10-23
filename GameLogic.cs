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

    public int feverCounter; //2020/10/23
    public float timeLimit;    //2020/10/23
    public float[] seconds;  //2020/10/23
    public bool[] answerRecords; //2020/10/23

    public int databaseQuestionNumbers; //資料庫的題目總數
    public int currentQuestionNumber;   //這個是 question array 的 index 不是真正的題號
    public int[] questionIDs;

    public Question[] questions;

    public int answerNumber;
    public int[] whatIsYourChoice;

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
    public Text countDownTimerText;
    public Text feverCounterText;
    public GameObject[] answerRecordsToggle;
   
    // Start is called before the first frame update
    void Start()
    {
        questionContentsText = GameObject.Find("Question").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        livesText = GameObject.Find("Lives").GetComponent<Text>();
        questionNumbersText = GameObject.Find("QuestionNumber").GetComponent<Text>();
        wrongAnswerTimesText = GameObject.Find("WrongAnswer").GetComponent<Text>();
        rightAnswerTimesText = GameObject.Find("RightAnswer").GetComponent<Text>();

        countDownTimerText = GameObject.Find("CountDownTimer").GetComponent<Text>();
        feverCounterText = GameObject.Find("FeverCounter").GetComponent<Text>();


        ///////// 上限 5 問題 4 選項，暫時先寫死

        answerRecords = new bool[5];
        answerRecordsToggle = new GameObject[5];
        whatIsYourChoice = new int[5];
        optionContentsText = new Text[4];

        for (int i = 0; i < answerRecordsToggle.Length; i++)
        {
            answerRecordsToggle[i] = GameObject.Find("Toggle" + (i + 1).ToString());
        }

        for (int i = 0; i < optionContentsText.Length; i++)
        {
            optionContentsText[i] = GameObject.Find("Option" + (i + 1).ToString()).GetComponent<Text>();
        }

        ///////// 以上寫死日後再修改

        ScoreBoard.Initialize(questionNumbers, lives, timeLimit);   //設定每場遊戲有幾個題目 + 幾條命 + 倒數幾秒
        UpdatePlayStatus();

        databaseQuestionNumbers = GameDataManager.Singleton.listQuestion.Count; //這裏要接資料庫

        SetQuestionIDs(questionNumbers, databaseQuestionNumbers);    //獲取問題編號陣列，questionIDs 陣列初始化結束
        SetQuestions();

        /*
         如果要動態產生選項按鈕的話，程式碼要寫在這裡
         */

        UpdateUI();
    }

    void Update()
    {
        if (currentQuestionNumber < questionNumbers)
        {
            ScoreBoard.SetSeconds(currentQuestionNumber, Time.deltaTime);
            countDownTimerText.text = Mathf.Ceil(timeLimit - ScoreBoard.GetSeconds(currentQuestionNumber)).ToString();
        }
    }

    public void CheckIsGameOver()
    {
        UpdatePlayStatus();

        if (lives <= 0 || wrongAnswerTimes >= 3)
        {
            ScoreBoard.SetGameover();   //計分板設定遊戲狀態為結束
            gameOver = ScoreBoard.IsGameOver();
            //Debug.Log("遊戲結束");
        }
        else if (currentQuestionNumber >= questionNumbers)    //index 的計算要減 1
        {
            ScoreBoard.SetGameover();   //計分板設定遊戲狀態為結束
            gameOver = ScoreBoard.IsGameOver();
            //Debug.Log("遊戲結束");
        }
        else
        {
            //Debug.Log("遊戲進行中...");
        }

        if (gameOver)
        {
            Debug.Log("玩完了，請接下一場");

            //////// 要顯示出結算頁面

            return;
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
        currentQuestionNumber = wrongAnswerTimes + rightAnswerTimes;    //目前在第幾題 (index)

        feverCounter = ScoreBoard.GetFeverCounter();

        for (int i = 0; i < questionNumbers; i++)
        {
            answerRecords[i] = ScoreBoard.GetAnswerRecords(i);
        }

        //public int timeLimit;    //2020/10/23
        //public float[] seconds;  //2020/10/23
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

    public void SetQuestions()     //設定題目內容
    {
        for (int i = 0; i < questionIDs.Length; i++)
        {
            questions[i] = GameDataManager.Singleton.listQuestion[questionIDs[i]];
        }
    }

    public void UpdateUI()
    {
        ShowQuestion(currentQuestionNumber);
        ShowOptions(currentQuestionNumber);
        ShowToggles(currentQuestionNumber);
        ShowScoreBoard();
    }

    public void ShowQuestion(int counter)  //這裡要開啟資料庫連線
    {
        if (counter < questionNumbers)
        {
            questionContentsText.text = "問題" + (currentQuestionNumber + 1).ToString() + ": " + questions[counter].questionContents;
        }
    }

    public void ShowOptions(int counter)    //這裡要開啟資料庫連線
    {
        if (counter < questionNumbers)
        {
            int howManyOptions = questions[counter].optionContents.Length;
            questions[counter].Initialize(howManyOptions, questions[counter].answerNumber);  //關鍵在於把正確答案的編號丟進布林陣列
            questions[counter].Permutation(howManyOptions); //先創布林陣列，才能開始進行亂數排序

            for (int i = 0; i < howManyOptions; i++)
            {
                optionContentsText[i].text = AddPrefix(i) + questions[counter].optionContents[i];
            }
        }
    }

    private string AddPrefix(int i) //選項加開頭字串
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

    private void ShowToggles(int counter)   //性質不一樣，答 1 題才更新 1 題
    {
        for (int i = 0; i < counter; i++)
        {
            answerRecordsToggle[i].GetComponent<Toggle>().isOn = answerRecords[i];
            answerRecordsToggle[i].transform.Find("Background").GetComponent<Image>().color = Color.yellow;    
        }
    }

    public void ShowScoreBoard()
    {
        scoreText.text = "目前的分數: " + score.ToString();
        livesText.text = "剩餘生命數: " + lives.ToString();
        wrongAnswerTimesText.text = "答錯次數: " + wrongAnswerTimes.ToString();
        rightAnswerTimesText.text = "答對次數: " + rightAnswerTimes.ToString();
        feverCounterText.text = "連續答對題數: " + feverCounter.ToString();

        if (currentQuestionNumber == questionNumbers)
        {
            questionNumbersText.text = "目前題號: " + questionNumbers.ToString();   //最後一題的時候，文字顯示特別處理
        }
        else
        {
            if (gameOver)
            {
                questionNumbersText.text = "目前題號: " + currentQuestionNumber.ToString(); //遊戲提前結束時，文字顯示特別處理
            }
            else
            {
                questionNumbersText.text = "目前題號: " + (currentQuestionNumber + 1).ToString();   //文字顯示 = 題號 index +1
            }
        }
    }

    public void MakeYourChoice(int youChooseNumber)
    {
        if (gameOver)
        {
            
        }
        else
        {
            whatIsYourChoice[currentQuestionNumber] = youChooseNumber;
            CheckAnswer(currentQuestionNumber);
        }
    }

    public void CheckAnswer(int counter)
    {
        if (counter < questionNumbers)
        {
            questions[counter].FindAnswerNumber(questions[counter].optionOrder.Length);  
            answerNumber = questions[counter].answerNumber;

            if (whatIsYourChoice[currentQuestionNumber] == answerNumber)
            {
                Debug.Log("回答正確");

                ScoreBoard.AnswerRight(100, currentQuestionNumber);
                CheckIsGameOver();
                UpdateUI();
            }
            else
            {
                Debug.Log("回答錯誤");

                ScoreBoard.AnswerWrong(100);
                CheckIsGameOver();
                UpdateUI();
            }
        }
    }
}
