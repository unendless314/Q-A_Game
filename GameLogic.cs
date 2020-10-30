using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//程式碼有很大的問題，就是可以連擊，問題出現以前就直接先回答完後面的所有題目

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

    public int feverCounter;
    public float timeLimit;
    private float timeRemaing;
    public bool startCountDown;
    public float[] seconds_Array;
    public bool[] answerRecords_Array;
    public int[] choiceNumbers_Array;
    public int[] answerNumbers_Array;

    public int databaseQuestionNumbers; //資料庫的題目總數
    public int currentQuestionNumber;   //這個是 question array 的 index 不是真正的題號
    public int[] questionIDs_Array;

    public Question[] questions_Array;

    /// <summary>
    /// View + UI
    /// </summary>

#if UNITY_ANDROID

#else

#endif

    public Text questionContentsText;
    public Text[] optionContentsText_Array;
    public Text scoreText;
    public Text livesText;
    public Text questionNumbersText;
    public Text wrongAnswerTimesText;
    public Text rightAnswerTimesText;
    public Text countDownTimerText;
    public Text feverCounterText;
    public Image[] optionBGImages_Array;
    public GameObject[] answerRecordsToggles_Array;

    public GameObject GameResult;

    // Start is called before the first frame update
    void Start()
    {
        ///////// 上限 5 問題 4 選項，暫時先寫死
        answerRecords_Array = new bool[5];
        answerRecordsToggles_Array = new GameObject[5];
        answerNumbers_Array = new int[5];
        choiceNumbers_Array = new int[] { 100, 100, 100, 100, 100 }; //new int[5]; 沒有回答的問題，都會變成選 100
        optionContentsText_Array = new Text[4];
        optionBGImages_Array = new Image[4];
        seconds_Array = new float[5];
        ///////// 以上先寫死，日後再修改優化

        GetUIComponents();

        ScoreBoard.Initialize(questionNumbers, lives, timeLimit);   //設定每場遊戲有幾個題目 + 幾條命 + 倒數幾秒

        UpdatePlayStatus();
        databaseQuestionNumbers = GameDataManager.Singleton.listQuestion.Count; //這裏要接資料庫
        SetQuestionIDs(questionNumbers, databaseQuestionNumbers);    //獲取問題編號陣列，questionIDs 陣列初始化結束
        SetQuestions();

        /*
         如果要動態產生選項按鈕的話，程式碼要寫在這裡
         */

        UpdateUI(currentQuestionNumber);
    }

    void Update()
    {
        if (gameOver)
        {

        }
        else
        {
            if (startCountDown == false)
            {

            }
            else

            if (currentQuestionNumber < questionNumbers)    //題數 index 小於問題總數，且計時開始才算時間
            {
                ScoreBoard.SetSeconds(currentQuestionNumber, Time.deltaTime);
                timeRemaing = Mathf.Ceil(timeLimit - ScoreBoard.GetSeconds(currentQuestionNumber));

                if (timeRemaing <= 0)
                {
                    MakeYourChoice(100);
                }

                countDownTimerText.text = timeRemaing.ToString();   //這其實是UI
            }
        }
    }

    #region 取得UI元件
    private void GetUIComponents()
    {
        //既然是 public 的話，為什麼不用拉的就好呢?
        questionContentsText = GameObject.Find("Question").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        livesText = GameObject.Find("Lives").GetComponent<Text>();
        questionNumbersText = GameObject.Find("QuestionNumber").GetComponent<Text>();
        wrongAnswerTimesText = GameObject.Find("WrongAnswer").GetComponent<Text>();
        rightAnswerTimesText = GameObject.Find("RightAnswer").GetComponent<Text>();
        countDownTimerText = GameObject.Find("CountDownTimer").GetComponent<Text>();
        feverCounterText = GameObject.Find("FeverCounter").GetComponent<Text>();

        for (int i = 0; i < answerRecordsToggles_Array.Length; i++)
        {
            answerRecordsToggles_Array[i] = GameObject.Find("Toggle" + (i + 1).ToString());
        }

        for (int i = 0; i < optionContentsText_Array.Length; i++)
        {
            optionContentsText_Array[i] = GameObject.Find("Option" + (i + 1).ToString()).GetComponent<Text>();
            optionBGImages_Array[i] = GameObject.Find("OptionBG" + (i + 1).ToString()).GetComponent<Image>();
        }
    }
    #endregion 取得UI元件

    #region 檢查遊戲是否已經結束
    public void CheckIsGameOver()
    {
        UpdatePlayStatus();

        if (lives <= 0 || wrongAnswerTimes >= 3)
        {
            ScoreBoard.SetGameover();   //計分板設定遊戲狀態為結束
            gameOver = ScoreBoard.IsGameOver();
        }
        else if (currentQuestionNumber >= questionNumbers)    //index 的計算要減 1，所以相等的時候其實表示目前題號已經超過題數上限
        {
            ScoreBoard.SetGameover();   //計分板設定遊戲狀態為結束
            gameOver = ScoreBoard.IsGameOver();
        }
        else
        {
        }

        if (gameOver)
        {
            Debug.Log("玩完了，請接下一場");
            return;
        }
    }
    #endregion 檢查遊戲是否已經結束

    #region 更新計分板
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
            answerRecords_Array[i] = ScoreBoard.GetAnswerRecords(i);
        }
    }
    #endregion 更新計分板


    #region 設定問題
    public void SetQuestionIDs(int questionNumbers, int databaseQuestionNumbers)  //預設取幾道題目，總題庫量多少題，這裏要接資料庫
    {
        //會需要取題目編號範圍
        if (questionNumbers > databaseQuestionNumbers)   //笨笨的防呆?
        {
            Debug.Log("出題數量大於資料表題目筆數");
            return;
        }

        questionIDs_Array = new int[questionNumbers];
        questions_Array = new Question[questionNumbers];

        for (int i = 0; i < questionNumbers; i++)
        {
            // 因為極大值會超出 index 範圍，所以取餘數，讓極大值發生時視同為 0;
            int randomQuestionID = ((int)UnityEngine.Random.Range(1, databaseQuestionNumbers)) % databaseQuestionNumbers;

            for (int j = 0; j < i; j++)
            {
                while (questionIDs_Array[j] == randomQuestionID)  //題數必須要大於題庫數量，否則 while 迴圈解不了 2020/10/27
                {
                    randomQuestionID = (int)UnityEngine.Random.Range(1, databaseQuestionNumbers);
                    j = 0;
                }
            }

            questionIDs_Array[i] = randomQuestionID;

        }
    }

    public void SetQuestions()     //設定題目內容
    {
        for (int i = 0; i < questionIDs_Array.Length; i++)
        {
            questions_Array[i] = GameDataManager.Singleton.listQuestion[questionIDs_Array[i]];
        }
    }
    #endregion 設定問題

    public void UpdateUI(int counter)
    {
        ShowToggles(counter);
        ShowScoreBoard();

        IEnumerator coroutine = ShowNextPage(counter);
        StartCoroutine(coroutine);
    }

    private void ShowToggles(int counter)   //性質不一樣，答 1 題才更新 1 題
    {
        for (int i = 0; i < counter; i++)
        {
            answerRecordsToggles_Array[i].GetComponent<Toggle>().isOn = answerRecords_Array[i];
            answerRecordsToggles_Array[i].transform.Find("Background").GetComponent<Image>().color = Color.yellow;
        }
    }

    public void ShowScoreBoard()
    {
        scoreText.text = "目前的分數: " + score.ToString();
        livesText.text = "剩餘生命數: " + lives.ToString();
        wrongAnswerTimesText.text = "答錯次數: " + wrongAnswerTimes.ToString();
        rightAnswerTimesText.text = "答對次數: " + rightAnswerTimes.ToString();
        feverCounterText.text = "連續答對題數: " + feverCounter.ToString();

        if (gameOver)
        {
            questionNumbersText.text = "目前題號: " + currentQuestionNumber.ToString(); //遊戲結束時，文字顯示另外處理
        }
        else
        {
            questionNumbersText.text = "目前題號: " + (currentQuestionNumber + 1).ToString();   //文字顯示 = 題號 index +1
        }
    }

    private IEnumerator ShowNextPage(int counter)
    {
        float waitTime = 0;

        switch (counter)
        {
            case 0:
                waitTime = 1.0f;
                break;
            default:
                waitTime = 2.0f;
                break;
        }

        if (counter > 0)
        {
            ShowAnswer();
        }

        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < optionBGImages_Array.Length; i++)
        {
            optionBGImages_Array[i].color = new Color(0.5283019f, 0.3562276f, 0, 1);
        }

        if (gameOver)
        {
            GameResult.SetActive(true);
        }
        else
        {
            ShowQuestion(currentQuestionNumber);
            ShowOptions(currentQuestionNumber);
            startCountDown = true;
        }
    }

    private void ShowAnswer()
    {
        int choice = choiceNumbers_Array[currentQuestionNumber - 1];
        int answer = answerNumbers_Array[currentQuestionNumber - 1];

        if (choice == 100)
        {
            optionBGImages_Array[answer].color = new Color(1, 0.92f, 0.016f, 1);
        }
        else
        {
            optionBGImages_Array[choice].color = new Color(1, 0, 0, 1);
            optionBGImages_Array[answer].color = new Color(1, 0.92f, 0.016f, 1);
        }
    }

    public void ShowQuestion(int counter)
    {
        questionContentsText.text = "問題" + (currentQuestionNumber + 1).ToString() + ": " + questions_Array[counter].questionContents;
    }

    public void ShowOptions(int counter)
    {
        int howManyOptions = questions_Array[counter].optionContents_Array.Length;
        questions_Array[counter].Initialize(howManyOptions);  //關鍵在於把正確答案的編號丟進布林陣列
        questions_Array[counter].Permutation(howManyOptions); //先創布林陣列，才能開始進行亂數排序

        for (int i = 0; i < howManyOptions; i++)
        {
            optionContentsText_Array[i].text = AddPrefix(i) + questions_Array[counter].optionContents_Array[i];
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

    #region 點擊Button選項並判斷答案
    public void MakeYourChoice(int youChooseNumber)
    {

        if (gameOver)
        {

        }
        else
        {
            if (startCountDown == true) //倒數計時過程中才能選取答案
            {
                seconds_Array[currentQuestionNumber] = ScoreBoard.GetSeconds(currentQuestionNumber);
                choiceNumbers_Array[currentQuestionNumber] = youChooseNumber;
                ScoreBoard.SetChoiceNumbers(currentQuestionNumber, youChooseNumber);
                CheckAnswer(currentQuestionNumber);
            }
        }

        startCountDown = false;
    }

    public void CheckAnswer(int counter)
    {
        questions_Array[counter].FindAnswerNumber(questions_Array[counter].optionOrder_Array.Length);
        answerNumbers_Array[counter] = questions_Array[counter].answerNumber;

        if (choiceNumbers_Array[currentQuestionNumber] == answerNumbers_Array[counter])
        {
            Debug.Log("回答正確");

            ScoreBoard.AnswerRight(100, currentQuestionNumber);
            CheckIsGameOver();
            UpdateUI(currentQuestionNumber);
        }
        else
        {
            Debug.Log("回答錯誤");

            ScoreBoard.AnswerWrong(100);
            CheckIsGameOver();
            UpdateUI(currentQuestionNumber);
        }
    }
    #endregion 點擊Button選項並判斷答案
}
