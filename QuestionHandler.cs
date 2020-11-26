using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;
using System.Linq;

public class QuestionHandler : GameModeController
{
    public int questionNumbers;
    public int optionNumbers;
    public int wrongAnswerTimes;
    public int rightAnswerTimes;
    public float timeLimit;
    public float timeRemaing;
    public bool startCountDown;

    public float[] playerSeconds_Array;
    public bool[] playerAnswerRecords_Array;
    public int[] playerChoiceNumbers_Array;
    public int[] answerNumbers_Array;

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
    public RawImage[] heartRawImage_Array;
    public Texture[] heartImageTexture_Array;
    public Image[] optionBtnImage_Array;

    public GameObject player_CorrectObj;
    public GameObject player_IncorrectObj;
    public GameObject result_AObject;
    public GameObject result_BObject;
    public GameObject readAgainBtn;
    public GameObject readFinishBtn;

    public GridLayoutGroup squareGridLayout;
    public GridLayoutGroup checkGridLayout;
    public GameObject squareCellObj;
    public GameObject checkCellObj;

    public Button endResultBtn1, endResultBtn2;
    public ReviewHandler reviewHandler;
    public Image baseImage;
    public Text baseText;

    public GameObject questionBGObj;
    public GameObject uI3D_CameraObj;
    public GameObject assetsObj;

    public GameObject questionPageObj;
    public GameObject articlePageObj;
    public GameObject hintWindowObj;
    public GameObject pauseGameButtonObj;
    public Button returnGameBtn;
    public Button quitGameBtn;
    public Button [] choiceBtn_Array;

    public Color color;

    public enum CategoryStatus
    {
        Article,
        NoArticle
    }

    public enum GamePlayStage
    {
        //只做一次
        Initialize,
        SetAILevel,
        SelectQuestion,
        SetQuestion,
        SetArticle,

        //會做很多次
        UndoPreviousUIChange,
        SetNextUIElements,
        ShowUIElements,
        AIAnswer,
        PlayerAnswer,
        ShowAnswer,

        //可以做很多次
        Pause,

        //只做一次
        EndGame
    }

    public void SelectCategory(int i_category)
    {
        if (i_category < 9)
        {
            GameDataManager.Singleton.categoryStatus = CategoryStatus.NoArticle;
            Debug.Log("遊戲類型: " + CategoryStatus.NoArticle);
        }
        else
        {
            GameDataManager.Singleton.categoryStatus = CategoryStatus.Article;
            Debug.Log("遊戲類型: " + CategoryStatus.Article);
        }
    }

    public void SelectGamePlayStage(GamePlayStage gamePlayStageType)
    {
        GameDataManager.Singleton.gamePlayStage = gamePlayStageType;
        Debug.Log("遊戲階段: " + gamePlayStageType);
    }

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();
        AddStoryContent();
    }

    private void AddStoryContent()
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

        if (GameDataManager.Singleton.categoryStatus == CategoryStatus.Article)
        {
            readAgainBtn.SetActive(true);
            readAgainBtn.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                ShowArticle();
            });

            readFinishBtn.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                CloseArticle();
            });
        }
        else
        {
            readAgainBtn.SetActive(false);
        }

        Initialize();
    }

    public void ShowArticle()
    {
        SelectGamePlayStage(GamePlayStage.Pause);
        questionPageObj.SetActive(false);
        articlePageObj.SetActive(true);
    }

    public void CloseArticle()
    {
        SelectGamePlayStage(GamePlayStage.ShowUIElements);
        questionPageObj.SetActive(true);
        articlePageObj.SetActive(false);
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

    public void Initialize()
    {
        SelectGamePlayStage(GamePlayStage.Initialize);

        assetsObj.SetActive(false);
        uI3D_CameraObj.SetActive(true);
        questionBGObj.SetActive(true);

        rightAnswerTimes = 0;
        wrongAnswerTimes = 0;
        currentQuestionNumber = wrongAnswerTimes + rightAnswerTimes;
        startCountDown = false;

        playerSeconds_Array = new float[questionNumbers];
        playerAnswerRecords_Array = new bool[questionNumbers];
        playerChoiceNumbers_Array = new int[questionNumbers];
        answerNumbers_Array = new int[questionNumbers];
        questionIDs_Array = new int[questionNumbers];
        optionContents_Array = new string[optionNumbers];
        optionOrder_Array = new bool[optionNumbers];
        questions_Array = new Question3[questionNumbers];

        for (int i = 0; i < playerChoiceNumbers_Array.Length; i++)
        {
            playerChoiceNumbers_Array[i] = 100;
        }

        questionContentsText.text = "";
        for (int i = 0; i < optionContentsText_Array.Length; i++)
        {
            optionContentsText_Array[i].text = "";
        }

        timeSlider.value = 1;

        pauseGameButtonObj.GetComponent<Button>().onClick.AddListener(PauseGame);
        returnGameBtn.onClick.AddListener(ReturnGame);
        quitGameBtn.onClick.AddListener(QuitGame);

        //居然不能用迴圈，真是奇妙
        choiceBtn_Array[0].onClick.AddListener(delegate { MakePlayerChoice(0); });
        choiceBtn_Array[1].onClick.AddListener(delegate { MakePlayerChoice(1); });
        choiceBtn_Array[2].onClick.AddListener(delegate { MakePlayerChoice(2); });
        choiceBtn_Array[3].onClick.AddListener(delegate { MakePlayerChoice(3); });

        UndoPreviousUIChange(); //這樣好嗎?
        SetToggles();

        if (GameDataManager.Singleton.categoryStatus == CategoryStatus.Article)
        {
            SetArticleIndexes();
            return;
        }
        else
        {
            SetQuestionIndexes();
            SetQuestions();
            UpdateUI();
        }
    }

    public void PauseGame()
    {
        SelectGamePlayStage(GamePlayStage.Pause);
        startCountDown = false;
        questionPageObj.SetActive(false);
        DialogManager.Singleton.ShowConfirmDialog("是否離開故事模式回到主選單？", delegate
        {
            QuitGame();
        }
        ,
        delegate (BaseDialogHandler aHandler)
        {
            ReturnGame();
        });
        //hintWindowObj.SetActive(true);
    }

    public void ReturnGame()
    {
        SelectGamePlayStage(GamePlayStage.ShowUIElements);
        startCountDown = true;
        questionPageObj.SetActive(true);
        //hintWindowObj.SetActive(false);
    }

    public void QuitGame()
    {
        startCountDown = false;
        hintWindowObj.SetActive(false);
        uI3D_CameraObj.SetActive(false);
        questionBGObj.SetActive(false);
        questionPageObj.SetActive(true);
        SelectGamePlayStage(GamePlayStage.EndGame);
        ShowNextQuestionOrEndGame();

        //   這裏少顯示了結算頁面，必須要是輸的那一種   //

        /*
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;
        CurrectGamePlayMode = GameMode.MAP;
        */
    }

    private void Update()
    {
        if (GameDataManager.Singleton.categoryStatus == CategoryStatus.Article)
        {
            return;
        }
        else if (currentQuestionNumber >= questionNumbers)
        {
            return;
        }
        else if (startCountDown == false)
        {
            return;
        }
        else if (wrongAnswerTimes < 3)
        {
            playerSeconds_Array[currentQuestionNumber] += Time.deltaTime;

            timeRemaing = timeLimit - playerSeconds_Array[currentQuestionNumber];

            if (timeRemaing > 0)
            {
                timeSlider.value = timeRemaing / timeLimit;
            }

            if (timeRemaing <= 0 && playerChoiceNumbers_Array[currentQuestionNumber] == 100)
            {
                MakePlayerChoice(99);
            }
        }
    }

    public void SetArticleIndexes()
    {
        /*

        假的 function!

        */

        Debug.Log("設定文章編號");
    }

    public void SetQuestionIndexes()
    {
        SelectGamePlayStage(GamePlayStage.SelectQuestion);

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
        SelectGamePlayStage(GamePlayStage.SetQuestion);

        for (int i = 0; i < questionIDs_Array.Length; i++)
        {
            questions_Array[i] = processedList.Find((Question3 obj) => obj.i_id == questionIDs_Array[i]);
        }
    }

    public void UpdateUI()
    {
        IEnumerator coroutine = SetUIComponents();
        StartCoroutine(coroutine);
    }

    public IEnumerator SetUIComponents()
    {
        float waitTime = 2;
        yield return new WaitForSeconds(waitTime);

        UndoPreviousUIChange();
        SetOptionContents();
        Permutation();
        FindAnswerNumber();
        ShowUIComponents();

    }

    public void UndoPreviousUIChange()
    {
        SelectGamePlayStage(GamePlayStage.UndoPreviousUIChange);

        for (int i = 0; i < optionBtnImage_Array.Length; i++)
        {
            optionBtnImage_Array[i].color = new Color(0.9622642f, 0.3850217f, 0.2950338f);
        }

        pauseGameButtonObj.SetActive(true);
        player_CorrectObj.SetActive(false);
        player_IncorrectObj.SetActive(false);

        if (GameDataManager.Singleton.categoryStatus == CategoryStatus.NoArticle)
        {
            readAgainBtn.SetActive(false);
        }
    }

    public void SetOptionContents()
    {
        SelectGamePlayStage(GamePlayStage.SetNextUIElements);

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

    public void ShowUIComponents()
    {
        SelectGamePlayStage(GamePlayStage.ShowUIElements);
        ShowQuestion();
        ShowOptions();
        startCountDown = true;
    }

    public void ShowQuestion()
    {
        questionContentsText.text = "問題" + (currentQuestionNumber + 1).ToString() + ": " + questions_Array[currentQuestionNumber].s_topic;
    }

    public void ShowOptions()   //這裡隱含寫死只有 4 選項
    {
        for (int i = 0; i < optionContentsText_Array.Length; i++)
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
        SelectGamePlayStage(GamePlayStage.PlayerAnswer);    //狀態機的轉換是否都要有前置條件?

        if (currentQuestionNumber == questionNumbers)
        {
        }
        else
        {
            if (startCountDown == true && playerChoiceNumbers_Array[currentQuestionNumber] == 100) //倒數計時過程中才能選取答案
            {
                playerChoiceNumbers_Array[currentQuestionNumber] = chooseNumber;
                CheckAnswer();
            }
        }
    }

    public void CheckAnswer()
    {
        if (playerChoiceNumbers_Array[currentQuestionNumber] == answerNumbers_Array[currentQuestionNumber])
        {
            player_CorrectObj.SetActive(true);
            playerAnswerRecords_Array[currentQuestionNumber] = true;
            rightAnswerTimes += 1;
        }
        else
        {
            player_IncorrectObj.SetActive(true);
            playerAnswerRecords_Array[currentQuestionNumber] = false;
            wrongAnswerTimes += 1;
        }

        pauseGameButtonObj.SetActive(false);
        ShowAnswer();

        startCountDown = false;
        currentQuestionNumber = rightAnswerTimes + wrongAnswerTimes;
        
        SetToggles();
        ShowNextQuestionOrEndGame();
    }

    public void ShowAnswer()
    {
        SelectGamePlayStage(GamePlayStage.ShowAnswer);

        readAgainBtn.SetActive(false);

        for (int i = 0; i < optionBtnImage_Array.Length; i++)
        {
            optionBtnImage_Array[i].color = new Color(0.3396226f, 0.2685777f, 0.257921f, 0.6f);
        }

        optionBtnImage_Array[answerNumbers_Array[currentQuestionNumber]].color = new Color(0.9811321f, 0.6248419f, 0.01388392f);
    }

    public void SetToggles()
    {
        for (int i = 0; i < 3; i++) //命只有 3 條
        {
            heartRawImage_Array[i].texture = heartImageTexture_Array[1];
        }

        for (int i = 0; i < wrongAnswerTimes; i++) //命只有 3 條
        {
            heartRawImage_Array[i].texture = heartImageTexture_Array[0];
        }

        for (int i = 0; i < questionNumbers; i++)
        {
            if (checkRawImage_Array.Length <= questionNumbers)
            {
                checkRawImage_Array[i].enabled = false;
            }
        }

        for (int i = 0; i < rightAnswerTimes; i++)
        {
            if (checkRawImage_Array.Length <= questionNumbers)
            {
                checkRawImage_Array[i].enabled = true;
            }
        }
    }

    public void ShowNextQuestionOrEndGame()
    {
        if (GameDataManager.Singleton.gamePlayStage == GamePlayStage.EndGame)
        {
            IEnumerator coroutine = ShowGameResult();
            StartCoroutine(coroutine);
            return;
        }

        if (wrongAnswerTimes >= 3)
        {
            IEnumerator coroutine = ShowGameResult();
            StartCoroutine(coroutine);
            return;
        }

        if (currentQuestionNumber == questionNumbers)
        {
            IEnumerator coroutine = ShowGameResult();
            StartCoroutine(coroutine);
            return;
        }
        else
        {
            UpdateUI();
        }
    }

    public IEnumerator ShowGameResult()
    {
        SelectGamePlayStage(GamePlayStage.EndGame);

        float waitTime = 2;
        yield return new WaitForSeconds(waitTime);

        if (rightAnswerTimes > 4)
        {
            result_AObject.SetActive(true);
        }
        else
        {
            result_BObject.SetActive(true);
        }

        if (endResultBtn1 != null)
        {
            endResultBtn1.onClick.RemoveAllListeners();
            endResultBtn1.onClick.AddListener(OnEndClick);
        }

        if (endResultBtn2 != null)
        {
            endResultBtn2.onClick.AddListener(OnEndClick);
        }
    }

    public void OnEndClick()
    {
        Debug.LogFormat("OnEndClick");
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        result_AObject.SetActive(false);
        result_BObject.SetActive(false);
        questionBGObj.SetActive(false);
        uI3D_CameraObj.SetActive(false);
        assetsObj.SetActive(true);

        SetReviewData();
        //PopControllerAndPlayMode(this, GameMode.REVIEW);
        CurrectGamePlayMode = GameMode.REVIEW;  //暫時先這樣寫
    }

    private void SetReviewData()
    {
        reviewHandler.questions_Array = questions_Array;  //這些選項已經換過順序
        reviewHandler.answerNumbers_Array = answerNumbers_Array;
        reviewHandler.playerChoiceNumbers_Array = playerChoiceNumbers_Array;
        reviewHandler.AddReviewContent();
        reviewHandler.toMapButtonObj.SetActive(true);
    }
}
