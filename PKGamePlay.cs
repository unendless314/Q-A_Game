using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PKGamePlay : MonoBehaviour
{
    public int playerScore;
    public int aIScore;
    public int questionNumbers;
    public int optionNumbers;
    public int wrongAnswerTimes;
    public int rightAnswerTimes;
    public int playerFeverCounter;
    public int aIFeverCounter;
    public float timeLimit;
    public float timeRemaing;
    public bool startCountDown;

    public float[] playerSeconds_Array;
    public bool[] playerAnswerRecords_Array;
    public int[] playerChoiceNumbers_Array;
    public int[] answerNumbers_Array;

    public float[] aISeconds_Array;
    public bool[] aIAnswerRecords_Array;
    public int[] aIChoiceNumbers_Array;

    public int databaseQuestionNumbers; //資料庫的題目總數
    public int currentQuestionNumber;   //這個是 question array 的 index 不是真正的題號
    public int[] questionIDs_Array;
    public string[] optionContents_Array;
    public bool[] optionOrder_Array;

    public Question2[] questions_Array;
    public Text questionContentsText;
    public Text[] optionContentsText_Array;
    public Slider timeSlider;
    public RawImage[] checkRawImage_Array;
    public Image[] optionBtnImage_Array;
    public Text playerScoreText;
    public Text aIScoreText;

    public GameObject aI_CorrectObj;
    public GameObject aI_IncorrectObj;
    public GameObject player_CorrectObj;
    public GameObject player_IncorrectObj;
    public GameObject result_AObject;
    public GameObject result_BObject;
    //public GameObject result_CObject; //平手畫面

    // Update is called once per frame
    void Update()
    {
        if (currentQuestionNumber == questionNumbers)
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
                playerSeconds_Array[currentQuestionNumber] += Time.deltaTime;
                aISeconds_Array[currentQuestionNumber] += Time.deltaTime;

                timeRemaing = timeLimit - playerSeconds_Array[currentQuestionNumber];

                if (timeRemaing > 0)
                {
                    timeSlider.value = timeRemaing / timeLimit;
                }

                if (timeRemaing < 9)    //我時間寫死了
                {
                    if (aIChoiceNumbers_Array[currentQuestionNumber] == 100)
                    {
                        MakeAIChoice();
                    }
                }

                if (timeRemaing <= 0)
                {
                    MakePlayerChoice(99);
                }
            }
        }
    }

    public void Initialize()
    {
        playerScore = 0;
        aIScore = 0;
        wrongAnswerTimes = 0;
        rightAnswerTimes = 0;
        currentQuestionNumber = wrongAnswerTimes + rightAnswerTimes;
        playerFeverCounter = 0;
        aIFeverCounter = 0;
        startCountDown = false;
        databaseQuestionNumbers = JsonDataManager.Singleton.processedList.Count;


        questionIDs_Array = new int[questionNumbers];
        questions_Array = new Question2[questionNumbers];

        playerChoiceNumbers_Array = new int[questionNumbers];
        answerNumbers_Array = new int[questionNumbers];
        playerAnswerRecords_Array = new bool[questionNumbers];
        playerSeconds_Array = new float[questionNumbers];

        aISeconds_Array = new float[questionNumbers];
        aIAnswerRecords_Array = new bool[questionNumbers];
        aIChoiceNumbers_Array = new int[questionNumbers];

        optionContents_Array = new string[optionNumbers];
        optionOrder_Array = new bool[optionNumbers];


        for (int i = 0; i < playerChoiceNumbers_Array.Length; i++)
        {
            playerChoiceNumbers_Array[i] = 100;
            aIChoiceNumbers_Array[i] = 100;
        }

        questionContentsText.text = "";
        for (int i = 0; i < optionContentsText_Array.Length; i++)
        {
            optionContentsText_Array[i].text = "";
        }

        timeSlider.value = 1;

        for (int i = 0; i < checkRawImage_Array.Length; i++)
        {
            checkRawImage_Array[i].enabled = false;
        }
    }

    public void SetQuestionIndexes()
    {
        if (questionNumbers > databaseQuestionNumbers)  //題數必須要大於題庫數量，避免底下 while 迴圈出包
        {
            questionNumbers = databaseQuestionNumbers;
        }

        for (int i = 0; i < questionNumbers; i++)
        {
            int randomQuestionIndex = Random.Range(0, databaseQuestionNumbers) % databaseQuestionNumbers;

            for (int j = 0; j < i; j++)
            {
                while (questionIDs_Array[j] == JsonDataManager.Singleton.processedList[randomQuestionIndex].i_id)
                {
                    randomQuestionIndex = Random.Range(0, databaseQuestionNumbers) % databaseQuestionNumbers;
                    j = 0;
                }
            }

            questionIDs_Array[i] = JsonDataManager.Singleton.processedList[randomQuestionIndex].i_id;
        }
    }

    public void SetQuestions()
    {
        for (int i = 0; i < questionIDs_Array.Length; i++)
        {
            questions_Array[i] = JsonDataManager.Singleton.processedList.Find((Question2 obj) => obj.i_id == questionIDs_Array[i]);
        }
    }

    public void UpdateUI()
    {
        IEnumerator coroutine = ShowNextPage();
        StartCoroutine(coroutine);
    }

    public IEnumerator ShowNextPage()
    {
        float waitTime = 2;
        yield return new WaitForSeconds(waitTime);

        ResetUIComponents();
        ShowQuestion();
        SetOptionContents();
        Permutation();
        FindAnswerNumber();
        ShowOptions();
        startCountDown = true;
    }

    public void ResetUIComponents()
    {
        for (int i = 0; i < optionBtnImage_Array.Length; i++)
        {
            optionBtnImage_Array[i].color = new Color(0.9622642f, 0.3850217f, 0.2950338f);
        }

        for (int i = 0; i < currentQuestionNumber; i++)
        {

        }

        aI_CorrectObj.SetActive(false);
        aI_IncorrectObj.SetActive(false);
        player_CorrectObj.SetActive(false);
        player_IncorrectObj.SetActive(false);

        //ResetToggles();   //對戰模式沒有此按鈕

    }

    public void ResetToggles()
    {
        for (int i = 0; i < questionNumbers; i++)
        {
            if (checkRawImage_Array.Length <= questionNumbers)
            {
                checkRawImage_Array[i].enabled = false;
            }
        }

        for (int i = 0; i < currentQuestionNumber; i++)
        {
            if (checkRawImage_Array.Length <= questionNumbers)
            {
                checkRawImage_Array[i].enabled = true;
            }
        }
    }

    public void ShowQuestion()
    {
        questionContentsText.text = "問題" + (currentQuestionNumber + 1).ToString() + ": " + questions_Array[currentQuestionNumber].s_QuestionContents;
    }

    private void SetOptionContents()
    {
        optionContents_Array[0] = questions_Array[currentQuestionNumber].s_Answer;
        optionContents_Array[1] = questions_Array[currentQuestionNumber].s_Option1;
        optionContents_Array[2] = questions_Array[currentQuestionNumber].s_Option2;
        optionContents_Array[3] = questions_Array[currentQuestionNumber].s_Option3;
        
        for (int i = 1; i < optionNumbers; i++)
        {
            optionOrder_Array[i] = false;
        }

        optionOrder_Array[0] = true;
    }

    public void Permutation()
    {
        List<string> optionListA = new List<string>();
        List<string> optionListB = new List<string>();

        List<bool> optionOrderA = new List<bool>();
        List<bool> optionOrderB = new List<bool>();

        optionListA = optionContents_Array.ToList();
        optionOrderA = optionOrder_Array.ToList();

        while (optionListA.Count > 0)
        {
            int randomIndex = Random.Range(0, optionListA.Count) % optionListA.Count;
            optionListB.Add(optionListA[randomIndex]);
            optionOrderB.Add(optionOrderA[randomIndex]);

            optionListA.Remove(optionListA[randomIndex]);
            optionOrderA.Remove(optionOrderA[randomIndex]);
        }

        optionContents_Array = optionListB.ToArray();
        optionOrder_Array = optionOrderB.ToArray();

        questions_Array[currentQuestionNumber].s_Answer = optionContents_Array[0];
        questions_Array[currentQuestionNumber].s_Option1 = optionContents_Array[1];
        questions_Array[currentQuestionNumber].s_Option2 = optionContents_Array[2];
        questions_Array[currentQuestionNumber].s_Option3 = optionContents_Array[3];
    }

    public void FindAnswerNumber()
    {
        int answerNumber = -1;  //答案 -1 不存在

        for (int i = 0; i < optionOrder_Array.Length; i++) //跑完迴圈以後，答案編號就存起來了
        {
            if (optionOrder_Array[i] == true)
            {
                answerNumber = i;
                break;
            }
        }

        answerNumbers_Array[currentQuestionNumber] = answerNumber;
    }

    public void ShowOptions()   //這裡隱含寫死只有 4 選項
    {
        for (int i = 0; i < 4; i++)
        {
            optionContentsText_Array[i].text = AddPrefix(i) + optionContents_Array[i];
        }
    }

    public string AddPrefix(int i) //選項加開頭字串
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

    public void MakePlayerChoice(int chooseNumber)
    {
        if (currentQuestionNumber == questionNumbers)
        {
        }
        else
        {
            if (startCountDown == true && playerChoiceNumbers_Array[currentQuestionNumber] == 100) //倒數計時過程中才能選取答案
            {
                playerChoiceNumbers_Array[currentQuestionNumber] = chooseNumber;
                CheckAnswer("thisIsPlayer");
            }
        }
    }
    

    public void MakeAIChoice()
    {
        if (currentQuestionNumber == questionNumbers)
        {
        }
        else
        {
            if (startCountDown == true) //倒數計時過程中才能選取答案
            {
                aIChoiceNumbers_Array[currentQuestionNumber] = Random.Range(0, optionNumbers);
                CheckAnswer("thisIsAI");
            }
        }
    }

    public void CheckAnswer(string whoAreYou)
    {
        switch (whoAreYou)
        {
            case "thisIsAI":

                if (aIChoiceNumbers_Array[currentQuestionNumber] == answerNumbers_Array[currentQuestionNumber])
                {
                    aI_CorrectObj.SetActive(true);

                    aIAnswerRecords_Array[currentQuestionNumber] = true;
                    aIFeverCounter += 1;

                    CalculateScore(whoAreYou, true, timeRemaing, aIFeverCounter);
                }
                else
                {
                    aI_IncorrectObj.SetActive(true);

                    aIAnswerRecords_Array[currentQuestionNumber] = false;
                    aIFeverCounter = 0;
                }

                break;

            case "thisIsPlayer":

                if (playerChoiceNumbers_Array[currentQuestionNumber] == answerNumbers_Array[currentQuestionNumber])
                {
                    player_CorrectObj.SetActive(true);

                    playerAnswerRecords_Array[currentQuestionNumber] = true;
                    playerFeverCounter += 1;

                    CalculateScore(whoAreYou, true, timeRemaing, playerFeverCounter);
                    rightAnswerTimes += 1;
                }
                else
                {
                    player_IncorrectObj.SetActive(true);

                    playerAnswerRecords_Array[currentQuestionNumber] = false;
                    playerFeverCounter = 0;

                    wrongAnswerTimes += 1;
                }

                ShowAnswer();
                break;

            default:
                break;
        }

        ShowScore();

        if (everyoneHasAnswered())
        {
            startCountDown = false;

            currentQuestionNumber = rightAnswerTimes + wrongAnswerTimes;
            ShowNextQuestionOrEndGame();
        }
    }

    public void CalculateScore(string whoAreYou, bool rightAnswer, float timeRemaing, int feverCounter)
    {
        switch (whoAreYou)
        {
            case "thisIsAI":
                if (rightAnswer)
                {
                    aIScore += 100 + (int)(timeRemaing * 10) + feverCounter * 15;  //這裡還要改
                }

                break;

            case "thisIsPlayer":
                if (rightAnswer)
                {
                    playerScore += 100 + (int)(timeRemaing * 10) + feverCounter * 15;   //這裡還要改
                }

                break;

            default:
                break;
        }
    }

    public void ShowScore()
    {
        aIScoreText.text = "AI 得分: " + aIScore.ToString();
        playerScoreText.text = "玩家得分: " + playerScore.ToString();
    }

    public bool everyoneHasAnswered()
    {
        if (aIChoiceNumbers_Array[currentQuestionNumber] == 100)
        {
            return false;
        }

        if (playerChoiceNumbers_Array[currentQuestionNumber] == 100)
        {
            return false;
        }

        return true;
    }

    public void ShowAnswer()
    {
        //readAgainBtn.SetActive(false); //對戰模式沒有此按鈕

        for (int i = 0; i < optionBtnImage_Array.Length; i++)
        {
            optionBtnImage_Array[i].color = new Color(0.3396226f, 0.2685777f, 0.257921f, 0.6f);
        }

        optionBtnImage_Array[answerNumbers_Array[currentQuestionNumber]].color = new Color(0.9811321f, 0.6248419f, 0.01388392f);
    }

    public void ShowNextQuestionOrEndGame()
    {
        if (currentQuestionNumber == questionNumbers)
        {
            IEnumerator coroutine = ShowGameResult();
            StartCoroutine(coroutine);
        }
        else
        {
            UpdateUI();
        }
    }

    public IEnumerator ShowGameResult()
    {
        float waitTime = 2;
        yield return new WaitForSeconds(waitTime);

        if (playerScore > aIScore)
        {
            result_AObject.SetActive(true);
        }
        else if (playerScore < aIScore)
        {
            result_BObject.SetActive(true);
        }
        else
        {
            //result_CObject.SetActive(true);
        }

        //ResetToggles();   //對戰模式沒有此按鈕
    }
}
