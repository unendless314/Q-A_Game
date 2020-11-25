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
    public int i_correct;
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

    public List<Question3> processedList;   //要改
    public Question3[] questions_Array; //要改
    public Text questionContentsText;
    public Text[] optionContentsText_Array;
    public Slider timeSlider;
    public GameObject[] checkRawObj_Array;
    public RawImage[] squareRawImage_Array;
    public RawImage[] eraserRawImage_Array;
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
    public GameObject textCellObj;

    public Button endResultBtn1, endResultBtn2;
    public ReviewHandler reviewHandler;
    //  public Image baseImage; //暫時用不到
    public Text baseChineseText;
    public Text baseEnglishText;
    public Text progressText;

    public GameObject profileObj;
    public GameObject storyObj;
    public GameObject arenaObj;
    public GameObject assetsHeartsObj;
    public GameObject assetsDimondsObj;

    public GameObject questionPageObj;
    public GameObject articlePageObj;
    public Button[] choiceBtn_Array;


    public GameObject pauseGameButtonObj;
    public Button returnGameBtn;
    public Button quitGameBtn;
    public GameObject hintWindowObj;
    public Color originalBtnColor, fadeBtnColor, wrongBtnColor, rightBtnColor;
    public Color originalTextColor, fadeTextColor, wrongTextColor, rightTextColor;
    public Sprite[] sprite_Array;
    public List<JsonConfigData> listConfigData;

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
        //AddStoryContent();    //寫在這裡不會自己執行，原因不明，只好從外面呼叫
    }

    public void AddStoryContent()
    {
        checkRawObj_Array = new GameObject[i_correct];    // 2020/11/25
        squareRawImage_Array = new RawImage[i_correct];   // 測試只生成正確題數量

        GameObject textCell = UnityTool.AddUGUIChild(squareGridLayout.transform, textCellObj);
        progressText = textCell.transform.GetChild(0).GetComponent<Text>();

        for (int i = 0; i < i_correct; i++) // 測試只生成正確題數量
        {
            GameObject checkCell = UnityTool.AddUGUIChild(checkGridLayout.transform, checkCellObj); //要改
            GameObject squareCell = UnityTool.AddUGUIChild(squareGridLayout.transform, squareCellObj);  //要改

            squareRawImage_Array[i] = squareCell.GetComponent<RawImage>();
            checkRawObj_Array[i] = checkCell;
            checkRawObj_Array[i].SetActive(false);
        }

        if (GameDataManager.Singleton.categoryStatus == CategoryStatus.Article)
        {
            readAgainBtn.SetActive(true);
            readAgainBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            readAgainBtn.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                ShowArticle();
            });

            readFinishBtn.GetComponent<Button>().onClick.RemoveAllListeners();
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
        UnityTool.RemoveAllChild(squareGridLayout.transform.gameObject);    //要改
        UnityTool.RemoveAllChild(checkGridLayout.transform.gameObject); //要改

        base.OnNavigationDestroy();
    }

    public override void OnNavigationStop()
    {
        UnityTool.RemoveAllChild(squareGridLayout.transform.gameObject);    //要改
        UnityTool.RemoveAllChild(checkGridLayout.transform.gameObject); //要改
        base.OnNavigationStop();
    }

    public void Initialize()
    {
        SelectGamePlayStage(GamePlayStage.Initialize);

        LoadConfigData();

        assetsHeartsObj.SetActive(false);
        assetsDimondsObj.SetActive(false);
        profileObj.SetActive(false);
        storyObj.SetActive(true);
        arenaObj.SetActive(false);

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

        //pauseGameButtonObj.GetComponent<Button>().onClick.RemoveAllListeners();
        //returnGameBtn.onClick.RemoveAllListeners();
        //quitGameBtn.onClick.RemoveAllListeners();

        //pauseGameButtonObj.GetComponent<Button>().onClick.AddListener(PauseGame);
        //returnGameBtn.onClick.AddListener(ReturnGame);
        //quitGameBtn.onClick.AddListener(QuitGame);

        //居然不能用迴圈，真是奇妙
        choiceBtn_Array[0].onClick.RemoveAllListeners();
        choiceBtn_Array[1].onClick.RemoveAllListeners();
        choiceBtn_Array[2].onClick.RemoveAllListeners();
        choiceBtn_Array[3].onClick.RemoveAllListeners();

        choiceBtn_Array[0].onClick.AddListener(delegate { MakePlayerChoice(0); });
        choiceBtn_Array[1].onClick.AddListener(delegate { MakePlayerChoice(1); });
        choiceBtn_Array[2].onClick.AddListener(delegate { MakePlayerChoice(2); });
        choiceBtn_Array[3].onClick.AddListener(delegate { MakePlayerChoice(3); });

        UndoPreviousUIChange();
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
        storyObj.SetActive(false);
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
        else if (rightAnswerTimes >= i_correct)
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
        else
        {
            return;
        }
    }

    public void LoadConfigData()
    {
        for (int i = 1; i <= JsonDataManager.Singleton.dictJsonConfigData.Count; i++)
        {
            listConfigData.Add(JsonDataManager.Singleton.dictJsonConfigData[i]);
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
        SetTimeLimit(questions_Array[currentQuestionNumber].i_category);
        SetOptionContents();
        Permutation();
        FindAnswerNumber();
        ShowUIComponents();

    }

    public void UndoPreviousUIChange()
    {
        SelectGamePlayStage(GamePlayStage.UndoPreviousUIChange);

        //選項按鈕圖檔及顏色初始化

        for (int i = 0; i < optionBtnImage_Array.Length; i++)
        {
            optionBtnImage_Array[i].sprite = sprite_Array[0];
            optionBtnImage_Array[i].color = originalBtnColor;
            optionContentsText_Array[i].color = originalTextColor;
        }

        //pauseGameButtonObj.SetActive(true);
        player_CorrectObj.SetActive(false);
        player_IncorrectObj.SetActive(false);

        if (GameDataManager.Singleton.categoryStatus == CategoryStatus.NoArticle)
        {
            readAgainBtn.SetActive(false);
        }

        baseChineseText.text = "";
        baseEnglishText.text = "";
        progressText.text = rightAnswerTimes.ToString() + "/" + i_correct.ToString();
    }

    public void SetTimeLimit(int questionCategory)
    {
        switch (questionCategory)
        {
            case 1:
                timeLimit = listConfigData[22].ai_data[0];
                break;
            case 2:
                timeLimit = listConfigData[22].ai_data[1];
                break;
            case 3:
                timeLimit = listConfigData[22].ai_data[2];
                break;
            case 4:
                timeLimit = listConfigData[22].ai_data[3];
                break;
            case 5:
                timeLimit = listConfigData[22].ai_data[4];
                break;
            case 6:
                timeLimit = listConfigData[22].ai_data[5];
                break;
            case 7:
                timeLimit = listConfigData[22].ai_data[6];
                break;
            case 8:
                timeLimit = listConfigData[22].ai_data[7];
                break;
            case 9:
                timeLimit = listConfigData[22].ai_data[8];
                break;
            case 10:
                timeLimit = listConfigData[22].ai_data[9];
                break;
            case 11:
                timeLimit = listConfigData[22].ai_data[10];
                break;
            case 12:
                timeLimit = listConfigData[22].ai_data[11];
                break;
            default:
                timeLimit = 10;
                break;
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
        SetBaseUI(questions_Array[currentQuestionNumber].i_category);
        startCountDown = true;
    }

    //這裡的套用字型只支援大寫英文字母，小寫英文字母無法顯示
    public void SetBaseUI(int category)
    {
        switch (category)
        {
            case 1:
            case 2:
            case 3:
                baseChineseText.text = "單字";
                baseEnglishText.text = "VOCABULARY";
                break;
            case 4:
            case 5:
                baseChineseText.text = "文法";
                baseEnglishText.text = "GRAMMAR";
                break;
            case 6:
            case 7:
                baseChineseText.text = "翻譯";
                baseEnglishText.text = "TRANSLATION";
                break;
            case 8:
                baseChineseText.text = "聽力";
                baseEnglishText.text = "LISTENING";
                break;
            case 9:
            case 10:
            case 11:
                baseChineseText.text = "閱讀";
                baseEnglishText.text = "READING";
                break;
            case 12:
                baseChineseText.text = "克漏字";
                baseEnglishText.text = "CLOZE";
                break;
            default:
                baseChineseText.text = "";
                baseEnglishText.text = "";
                break;
        }
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

        if (rightAnswerTimes < i_correct && wrongAnswerTimes < 3)
        {
            if (startCountDown == true && playerChoiceNumbers_Array[currentQuestionNumber] == 100) //倒數計時過程中才能選取答案
            {
                playerChoiceNumbers_Array[currentQuestionNumber] = chooseNumber;
                CheckAnswer();
            }
        }
        else
        {
            return;
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

        //pauseGameButtonObj.SetActive(false);    //目前有這顆按鈕嗎?
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

        for (int i = 0; i < optionBtnImage_Array.Length; i++)   //先把每一顆按鈕圖片，色彩，及文字淡化
        {
            optionBtnImage_Array[i].sprite = sprite_Array[1];
            optionBtnImage_Array[i].color = fadeBtnColor;
            optionContentsText_Array[i].color = fadeTextColor;
        }

        if (playerChoiceNumbers_Array[currentQuestionNumber] < 3)   //把每玩家選擇的按鈕圖片，色彩，及文字標示錯誤
        {
            optionBtnImage_Array[playerChoiceNumbers_Array[currentQuestionNumber]].sprite = sprite_Array[2];
            optionBtnImage_Array[playerChoiceNumbers_Array[currentQuestionNumber]].color = wrongBtnColor;
            optionContentsText_Array[playerChoiceNumbers_Array[currentQuestionNumber]].color = wrongTextColor;
        }

        //把解答的按鈕圖片，色彩，及文字標示正確，如果與玩家的選項相同，就會蓋掉玩家錯誤的效果
        optionBtnImage_Array[answerNumbers_Array[currentQuestionNumber]].sprite = sprite_Array[3];
        optionBtnImage_Array[answerNumbers_Array[currentQuestionNumber]].color = rightBtnColor;
        optionContentsText_Array[answerNumbers_Array[currentQuestionNumber]].color = rightTextColor;
    }

    public void SetToggles()
    {
        for (int i = 0; i < 3; i++) //橡皮擦只有 3 條，先把橡皮擦全部顯示出來
        {
            eraserRawImage_Array[i].enabled = true;
        }

        for (int i = 0; i < wrongAnswerTimes; i++) //再依照答錯的次數把橡皮擦隱藏起來
        {
            eraserRawImage_Array[i].enabled = false;
        }

        for (int i = 0; i < i_correct; i++)
        {
            checkRawObj_Array[i].SetActive(false);  //先把所有勾勾隱藏起來
        }

        for (int i = 0; i < rightAnswerTimes; i++)
        {
            checkRawObj_Array[i].SetActive(true);   //再依照答對的數量，把勾勾顯示出來
        }

        progressText.text = rightAnswerTimes.ToString() + "/" + i_correct.ToString();
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

        if (rightAnswerTimes >= i_correct)
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

        if (rightAnswerTimes >= i_correct)
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
            endResultBtn2.onClick.RemoveAllListeners();
            endResultBtn2.onClick.AddListener(OnEndClick);
        }

        UndoPreviousUIChange();
    }

    public void OnEndClick()
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        storyObj.SetActive(false);
        assetsHeartsObj.SetActive(true);
        assetsDimondsObj.SetActive(true);

        result_AObject.SetActive(false);
        result_BObject.SetActive(false);

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
//      reviewHandler.toMapButtonObj.SetActive(true);   //要改寫
    }
}
