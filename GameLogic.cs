using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StoryGamePlay : MonoBehaviour
{
    public int score;   //目前用不到
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
    public string[] optionContents_Array;
    public bool[] optionOrder_Array;

    public Question1[] questions_Array;
    public Text questionContentsText;
    public Text[] optionContentsText_Array;
    public Slider timeSlider;
    public GameObject[] heartImagesObj;
    public RawImage[] checkRawImages;
    public Texture[] iconTextures;

    public GameObject correctObj;
    public GameObject incorrectObj;

    public GameObject result_AObject;
    public GameObject result_BObject;
    public GameObject readAgainObj;
    
    void Start()
    {
        Initilize();
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
                //timeRemaing = Mathf.Ceil(timeLimit - ScoreBoard.GetSeconds(currentQuestionNumber));
                timeRemaing = timeLimit - ScoreBoard.GetSeconds(currentQuestionNumber);
                timeSlider.value = (float)(timeRemaing / timeLimit);

                if (timeRemaing <= 0)
                {
                    MakeYourChoice(100);
                }
            }
        }
    }

    public void Initilize()
    {
        answerRecords_Array = new bool[5];
        answerNumbers_Array = new int[5];
        choiceNumbers_Array = new int[] { 100, 100, 100, 100, 100 }; //new int[5]; 沒有回答的問題，都會變成選 100
        seconds_Array = new float[5];
        optionContents_Array = new string[4];
        optionOrder_Array = new bool[4];
        ///////// 以上先寫死，日後再修改優化

        /// 可以用這種寫法，但是很爛，因為 coroutine 在物件 inactive 時執行會報錯，可以用 try catch 繞過這個問題，但一樣很爛

        questionContentsText.text = "QUESTION";
        for (int i = 0; i < optionContentsText_Array.Length; i++)
        {
            optionContentsText_Array[i].text = "ANSWER";
        }

        timeSlider.value = 1;

        for (int i = 0; i < checkRawImages.Length; i++)
        {
            checkRawImages[i].texture = iconTextures[0];
        }

        ///

        GetUIComponents();

        ScoreBoard.Initialize(questionNumbers, 3, timeLimit);   //設定每場遊戲有幾個題目 + 幾條命 + 倒數幾秒

        UpdatePlayStatus();
        databaseQuestionNumbers = JsonDataManager.Singleton.processedList.Count; //這裏要接資料庫

        SetQuestionIndexes(questionNumbers, databaseQuestionNumbers);    //獲取問題編號陣列，questionIDs 陣列初始化結束
        SetQuestions();

        /*
         如果要動態產生選項按鈕的話，程式碼要寫在這裡
         */

        UpdateUI(currentQuestionNumber);
    }

    public void GetUIComponents()
    {

    }

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

    public void SetQuestionIndexes(int questionNumbers, int databaseQuestionNumbers)  //預設取幾道題目，總題庫量多少題，這裏要接資料庫
    {
        //會需要取題目編號範圍
        if (questionNumbers > databaseQuestionNumbers)   //笨笨的防呆?
        {
            Debug.Log("出題數量大於資料表題目筆數");
            return;
        }

        questionIDs_Array = new int[questionNumbers];
        questions_Array = new Question1[questionNumbers];

        for (int i = 0; i < questionNumbers; i++)
        {
            // 因為極大值會超出 index 範圍，所以取餘數，讓極大值發生時視同為 0;
            int randomQuestionIndex = ((int)UnityEngine.Random.Range(0, databaseQuestionNumbers)) % databaseQuestionNumbers;

            for (int j = 0; j < i; j++)
            {
                while (questionIDs_Array[j] == JsonDataManager.Singleton.processedList[randomQuestionIndex].i_id)  //題數必須要大於題庫數量，否則 while 迴圈解不了 2020/10/27
                {
                    randomQuestionIndex = (int)UnityEngine.Random.Range(0, databaseQuestionNumbers) % databaseQuestionNumbers;
                    j = 0;
                }
            }

            questionIDs_Array[i] = JsonDataManager.Singleton.processedList[randomQuestionIndex].i_id;
        }
    }

    public void SetQuestions()     //設定題目內容
    {
        for (int i = 0; i < questionIDs_Array.Length; i++)
        {
            questions_Array[i] = JsonDataManager.Singleton.processedList.Find((Question1 obj) => obj.i_id == questionIDs_Array[i]);
        }
    }

    public void UpdateUI(int counter)
    {
        ShowToggles(counter);
        ShowScoreBoard();

        IEnumerator coroutine = ShowNextPage(counter);
        StartCoroutine(coroutine);
    }

    public void ShowToggles(int counter)   //性質不一樣，答 1 題才更新 1 題
    {

        ////// 有點爛的寫法，先把所有的愛心關掉，再根據目前生命數量把愛心開起來

        for (int i = 0; i < 3; i++)
        {
            heartImagesObj[i].SetActive(false);
        }

        for (int i = 0; i < lives; i++)
        {
            heartImagesObj[i].SetActive(true);
        }

        if (counter < 1)    //回答第一題的時候，第一題答案根本沒出來，所以不用顯示答對與否
        {
            return;
        }

        if (answerRecords_Array[counter - 1])   //從第二題開始，更新上一題的回答結果
        {
            checkRawImages[counter - 1].texture = iconTextures[1];
        }
        else
        {
            checkRawImages[counter - 1].texture = iconTextures[2];
        }
        

        //////

        for (int i = 0; i < counter; i++)
        {
            //answerRecordsToggles_Array[i].GetComponent<Toggle>().isOn = answerRecords_Array[i];
            //answerRecordsToggles_Array[i].transform.Find("Background").GetComponent<Image>().color = Color.yellow;
        }
    }

    public void ShowScoreBoard()
    {
        
    }

    public IEnumerator ShowNextPage(int counter)
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

        correctObj.SetActive(false);
        incorrectObj.SetActive(false);

        if (gameOver)
        {
            ShowGameResult();
        }
        else
        {
            ShowQuestion(currentQuestionNumber);
            ShowOptions(currentQuestionNumber);
            startCountDown = true;
        }
    }

    public void ShowAnswer()
    {
        int choice = choiceNumbers_Array[currentQuestionNumber - 1];
        int answer = answerNumbers_Array[currentQuestionNumber - 1];

        if (choice == 100)
        {

        }
        else
        {
            
        }
    }

    public void ShowGameResult()
    {
        if (rightAnswerTimes >= 4)
        {
            result_AObject.SetActive(true);
        }
        else
        {
            result_BObject.SetActive(true);
        }
    }

    public void ShowQuestion(int counter)
    {
        questionContentsText.text = "問題" + (currentQuestionNumber + 1).ToString() + ": " + questions_Array[counter].s_QuestionContents;
    }

    public void ShowOptions(int counter)
    {
        optionContents_Array[0] = questions_Array[counter].s_Option1;
        optionContents_Array[1] = questions_Array[counter].s_Option2;
        optionContents_Array[2] = questions_Array[counter].s_Option3;
        optionContents_Array[3] = questions_Array[counter].s_Answer;

        optionOrder_Array[0] = false;
        optionOrder_Array[1] = false;
        optionOrder_Array[2] = false;
        optionOrder_Array[3] = true;

        Permutation(4); //先創布林陣列，才能開始進行亂數排序

        for (int i = 0; i < 4; i++)
        {
            optionContentsText_Array[i].text = AddPrefix(i) + optionContents_Array[i];
        }
    }

    public void Permutation(int howManyOptions) //改變答案及選項的順序
    {
        List<string> optionListA = new List<string>();
        List<string> optionListB = new List<string>();

        List<bool> optionOrderA = new List<bool>();
        List<bool> optionOrderB = new List<bool>();

        int howManyElementsInListA; //A 集合裡面的元素數量，其實是多餘程式碼，但閱讀性可能較佳

        for (int i = 0; i < howManyOptions; i++)
        {
            optionListA.Add(optionContents_Array[i]); //A 集合塞了所有的選項
            optionOrderA.Add(optionOrder_Array[i]);   //A 集合塞了所有的布林
        }

        while (optionListA.Count > 0)
        {
            howManyElementsInListA = optionListA.Count; //算出 A 集合裡面有幾個元素
            int randomIndex = ((int)UnityEngine.Random.Range(0, howManyElementsInListA)) % howManyElementsInListA;    //取出 A 集合中的第幾個元素
            optionListB.Add(optionListA[randomIndex]); //將此元素塞進 B 集合中
            optionOrderB.Add(optionOrderA[randomIndex]); //將此布林塞進 B 集合中

            optionListA.Remove(optionListA[randomIndex]); //移除 A 集合中剛剛取出的元素
            optionOrderA.Remove(optionOrderA[randomIndex]); //移除 A 集合中剛剛取出的布林
        }

        for (int i = 0; i < howManyOptions; i++)    //將新排序後的 B 集合元素丟回所有的選項中
        {
            optionContents_Array[i] = optionListB[i];
            optionOrder_Array[i] = optionOrderB[i];
        }

        //// 1103 追加
        questions_Array[currentQuestionNumber].setOptionContentsArray(optionContents_Array[0], optionContents_Array[1], optionContents_Array[2], optionContents_Array[3]);
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
        int answerNumber = -1;  //答案 -1 不存在

        for (int i = 0; i < optionOrder_Array.Length; i++)
        {
            if (optionOrder_Array[i] == true)
            {
                answerNumber = i;
                break;
            }
        }

        answerNumbers_Array[counter] = answerNumber;

        if (choiceNumbers_Array[currentQuestionNumber] == answerNumbers_Array[counter])
        {
            correctObj.SetActive(true);
            //Debug.Log("回答正確");

            ScoreBoard.AnswerRight(100, currentQuestionNumber);
            CheckIsGameOver();
            UpdateUI(currentQuestionNumber);
        }
        else
        {
            incorrectObj.SetActive(true);
            //Debug.Log("回答錯誤");

            ScoreBoard.AnswerWrong(100);
            CheckIsGameOver();
            UpdateUI(currentQuestionNumber);
        }
    }

}
