using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evaluation : MonoBehaviour
{
    public GameLogic gameLogic;

    public int questionNumbers;
    public float[] seconds_Array;
    public bool[] answerRecords_Array;
    public int[] choiceNumbers_Array;
    public int[] answerNumbers_Array;
    public float totalSeconds;

    public GameObject[] answerRecordsToggles_Array;
    public Toggle[] selectToggles_Array;

    public TestQuestion1[] questions_Array;
    public Text questionContentsText;
    public Text[] optionContentsText_Array;
    public Text answeNumberText;
    public Text choiceNumberText;
    public Text secondsText;
    public Text totalSecondsText;


    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();

        ///////// 上限 5 問題 4 選項，暫時先寫死
        selectToggles_Array = new Toggle[5];
        optionContentsText_Array = new Text[4];
        answerRecordsToggles_Array = new GameObject[5];
        ///////// 以上先寫死，日後再修改優化

        questionNumbers = gameLogic.questionNumbers;
        seconds_Array = gameLogic.seconds_Array;
        answerRecords_Array = gameLogic.answerRecords_Array;
        choiceNumbers_Array = gameLogic.choiceNumbers_Array;
        answerNumbers_Array = gameLogic.answerNumbers_Array;
        questions_Array = gameLogic.questions_Array;

        for (int i = 0; i < questionNumbers; i++)
        {
            totalSeconds += seconds_Array[i];
        }

        GetUIComponents();
        ShowContents(0);
    }

    private void GetUIComponents()
    {
        questionContentsText = transform.Find("Question").GetComponent<Text>();
        secondsText = transform.Find("Seconds").GetComponent<Text>();
        totalSecondsText = transform.Find("TotalSeconds").GetComponent<Text>();

        for (int i = 0; i < 4; i++) //暫時寫死為 4 選項
        {
            optionContentsText_Array[i] = transform.Find("Option" + (i + 1).ToString()).GetComponent<Text>();
        }

        for (int i = 0; i < questionNumbers; i++)
        {
            selectToggles_Array[i] = transform.Find("SelectToggles").transform.Find("Question" + (i + 1).ToString()).GetComponent<Toggle>();   //可以直接用 Toggle，因為只是要點選
        }

        for (int i = 0; i < answerRecordsToggles_Array.Length; i++)
        {
            answerRecordsToggles_Array[i] = GameObject.Find("AnswerRecord" + (i + 1).ToString());   //如果有要改背景顏色的話，變數創立時只能用 GameObject
            answerRecordsToggles_Array[i].GetComponent<Toggle>().isOn = answerRecords_Array[i];
        }

        answeNumberText = transform.Find("AnswerNumber").GetComponent<Text>();
        choiceNumberText = transform.Find("ChoiceNumber").GetComponent<Text>();
    }

    public void ShowContents(int ChooseQuestion)
    {
        questionContentsText.text = "問題" + (ChooseQuestion + 1).ToString() + ": " + questions_Array[ChooseQuestion].s_QuestionContents;
        secondsText.text = seconds_Array[ChooseQuestion].ToString();
        totalSecondsText.text = totalSeconds.ToString();

        for (int i = 0; i < 4; i++)
        {
            optionContentsText_Array[i].text = ShowPrefix(i) + " " + gameLogic.optionContents_Array[i];
        }

        answeNumberText.text = "正確答案為: " + ShowPrefix(answerNumbers_Array[ChooseQuestion]);
        choiceNumberText.text = "您的選項為: " + ShowPrefix(choiceNumbers_Array[ChooseQuestion]);
    }

    private string ShowPrefix(int OptionNumber)
    {
        switch (OptionNumber)
        {
            case 0:
                return "(A)";
            case 1:
                return "(B)";
            case 2:
                return "(C)";
            case 3:
                return "(D)";
            default:
                return "(未作答)";
        }
    }
}
