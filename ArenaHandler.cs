using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;
using System.Linq;

public class ArenaHandler : GameModeController
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
    public float[] backendSeconds_Array;
    public bool[] playerAnswerRecords_Array;
    public int[] playerChoiceNumbers_Array;
    public int[] answerNumbers_Array;

    public bool[] aIAnswerRecords_Array;
    public int[] aIChoiceNumbers_Array;

    public int databaseQuestionNumbers; //資料庫的題目總數
    public int currentQuestionNumber;   //這個是 question array 的 index 不是真正的題號
    public int[] questionIDs_Array;
    public string[] optionContents_Array;
    public bool[] optionOrder_Array;

    public List<Question3> processedList;
    public Question3[] questions_Array;
    public Text questionContentsText;
    public Text[] optionContentsText_Array;
    public Slider timeSlider;
    public RawImage[] checkRawImage_Array;
    public RawImage[] squareRawImage_Array;
    public Image[] optionBtnImage_Array;
    public Text playerScoreText;
    public Text aIScoreText;

    public Text playerLevelText;
    public Text playerNickNameText;
    public Text aILevelText;
    public Text aINickNameText;

    public GameObject aI_CorrectObj;
    public GameObject aI_IncorrectObj;
    public GameObject player_CorrectObj;
    public GameObject player_IncorrectObj;
    public GameObject result_AObject;
    public GameObject result_BObject;

    public GridLayoutGroup squareGridLayout;
    public GridLayoutGroup checkGridLayout;
    public GameObject squareCellObj;
    public GameObject checkCellObj;

    public Button endResultBtn1, endResultBtn2;
    public ReviewHandler reviewHandler;

    public GameObject MisakiObj;
    public GameObject MisakiCamera;
    public GameObject AssetsObj;
    public GameObject upgradeObj;

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

                timeRemaing = timeLimit - playerSeconds_Array[currentQuestionNumber];

                if (timeRemaing > 0)
                {
                    timeSlider.value = timeRemaing / timeLimit;
                }

                if (timeRemaing < 7)    //我時間寫死了
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
        MisakiObj.SetActive(true);
        MisakiCamera.SetActive(true);
        AssetsObj.SetActive(false);

        playerScore = 0;
        aIScore = 0;
        rightAnswerTimes = 0;
        wrongAnswerTimes = 0;
        currentQuestionNumber = wrongAnswerTimes + rightAnswerTimes;
        playerFeverCounter = 0;
        aIFeverCounter = 0;
        startCountDown = false;

        playerSeconds_Array = new float[questionNumbers];
        backendSeconds_Array = new float[questionNumbers];
        playerAnswerRecords_Array = new bool[questionNumbers];
        playerChoiceNumbers_Array = new int[questionNumbers];
        answerNumbers_Array = new int[questionNumbers];
        aIAnswerRecords_Array = new bool[questionNumbers];
        aIChoiceNumbers_Array = new int[questionNumbers];
        questionIDs_Array = new int[questionNumbers];
        optionContents_Array = new string[optionNumbers];
        optionOrder_Array = new bool[optionNumbers];
        questions_Array = new Question3[questionNumbers];

        //以下是不好的寫法，沒有狀態機，但因為比較有把握所以先這麼寫

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

        /*
        for (int i = 0; i < checkRawImage_Array.Length; i++)
        {
            checkRawImage_Array[i].enabled = false;
        }
        */

        AddArenaContent();
        SetQuestionIndexes();
        SetQuestions();
        UpdateUI();
        ResetUIComponents();
        ShowScore();
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
                while (questionIDs_Array[j] == processedList[randomQuestionIndex].i_id)
                {
                    randomQuestionIndex = Random.Range(0, databaseQuestionNumbers) % databaseQuestionNumbers;
                    j = 0;
                }
            }

            questionIDs_Array[i] = processedList[randomQuestionIndex].i_id;
        }
    }

    public void SetQuestions()
    {
        for (int i = 0; i < questionIDs_Array.Length; i++)
        {
            questions_Array[i] = processedList.Find((Question3 obj) => obj.i_id == questionIDs_Array[i]);
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

        aI_CorrectObj.SetActive(false);
        aI_IncorrectObj.SetActive(false);
        player_CorrectObj.SetActive(false);
        player_IncorrectObj.SetActive(false);

        ResetToggles();   //對戰模式沒有此按鈕

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
        questionContentsText.text = "問題" + (currentQuestionNumber + 1).ToString() + ": " + questions_Array[currentQuestionNumber].s_topic;
    }

    public void SetOptionContents()
    {
        optionContents_Array[0] = questions_Array[currentQuestionNumber].s_answer;
        optionContents_Array[1] = questions_Array[currentQuestionNumber].s_item1;
        optionContents_Array[2] = questions_Array[currentQuestionNumber].s_item2;
        optionContents_Array[3] = questions_Array[currentQuestionNumber].s_item3;

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

        questions_Array[currentQuestionNumber].s_answer = optionContents_Array[0];
        questions_Array[currentQuestionNumber].s_item1 = optionContents_Array[1];
        questions_Array[currentQuestionNumber].s_item2 = optionContents_Array[2];
        questions_Array[currentQuestionNumber].s_item3 = optionContents_Array[3];
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

                backendSeconds_Array[currentQuestionNumber] = playerSeconds_Array[currentQuestionNumber];

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

                    backendSeconds_Array[currentQuestionNumber] = -1;   //秒數設為負 1，意味玩家答錯
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
                    aIScore += (int)(100 + (timeRemaing * 10) * Weight(feverCounter));  //這裡還要改
                }

                break;

            case "thisIsPlayer":
                if (rightAnswer)
                {
                    playerScore += (int)(100 + (timeRemaing * 10) * Weight(feverCounter));   //這裡還要改
                }

                break;

            default:
                break;
        }
    }

    public float Weight(int feverCounter)
    {
        switch (feverCounter)
        {
            case 0:
                return 1;

            case 1:
                return 1;

            case 2:
                return 1;

            case 3:
                return 1.25f;

            case 4:
                return 1.45f;

            case 5:
                return 1.65f;

            case 6:
                return 1.85f;

            case 7:
                return 2;

            default:
                return 2;
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
            upgradeObj.GetComponent<RankRise>().SetRankRiseContents("Rank Rise!", aILevelText.text, playerLevelText.text, playerNickNameText.text);
        }
        else if (playerScore < aIScore)
        {
            upgradeObj.GetComponent<RankRise>().SetRankRiseContents("Rank Fall!", aILevelText.text, playerLevelText.text, playerNickNameText.text);
            result_BObject.SetActive(true);
        }
        else
        {
            //result_CObject.SetActive(true);
        }

        ResetToggles();   //對戰模式沒有此按鈕

        if (endResultBtn1 != null)
        {
            endResultBtn1.onClick.AddListener(OnEndClick);
        }

        if (endResultBtn2 != null)
        {
            endResultBtn2.onClick.AddListener(OnEndClick);
        }
    }

    private void OnEndClick()
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        result_AObject.SetActive(false);
        result_BObject.SetActive(false);
        MisakiObj.SetActive(false);
        MisakiCamera.SetActive(true);
        AssetsObj.SetActive(true);

        upgradeObj.SetActive(true);
        upgradeObj.GetComponent<RankRise>().DisplayForFiveSeconds();

        SetReviewData();
        //PopControllerAndPlayMode(this, GameMode.REVIEW);


        upgradeObj.SetActive(true);


        CurrectGamePlayMode = GameMode.REVIEW;  //暫時先這樣寫
    }

    private void SetReviewData()
    {
        reviewHandler.questions_Array = questions_Array;  //這裡已經是換選項後的問題
        reviewHandler.answerNumbers_Array = answerNumbers_Array;
        reviewHandler.playerChoiceNumbers_Array = playerChoiceNumbers_Array;
        reviewHandler.AddReviewContent();
        reviewHandler.toMapButtonObj.SetActive(false);
    }

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();
        //AddArenaContent();
    }

    private void AddArenaContent()
    {
        checkRawImage_Array = new RawImage[questionNumbers];
        squareRawImage_Array = new RawImage[questionNumbers];

        for (int i = 0; i < questionNumbers; i++)
        {
            GameObject checkCell = UnityTool.AddUGUIChild(checkGridLayout.transform, checkCellObj);
            GameObject squareCell = UnityTool.AddUGUIChild(squareGridLayout.transform, squareCellObj);

            checkRawImage_Array[i] = checkCell.GetComponent<RawImage>();
            squareRawImage_Array[i] = squareCell.GetComponent<RawImage>();

            checkRawImage_Array[i].enabled = false;
        }
    }

    public override void OnNavigationDestroy()
    {
        base.OnNavigationDestroy();
    }

    public override void OnNavigationStop()
    {
        UnityTool.RemoveAllChild(checkGridLayout.transform.gameObject);
        UnityTool.RemoveAllChild(squareGridLayout.transform.gameObject);
        base.OnNavigationStop();
    }
}
