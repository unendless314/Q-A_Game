using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evaluation : MonoBehaviour
{
    public StoryGamePlay dataFromLastStoryGamePlay;

    public Question1[] questions_Array;
    public Button[] expandButton_Array;
    public Button[] hideButton_Array;
    public Text[] questionContents_Array;
    public Text[] optionContents_Array;
    public Image[] favoriteImages_Array;
    public Image[] answerRecordImages_Array;
    public Sprite[] heartSprites_Array;
    public Sprite[] answerRecordSprites_Array;

    void Start()
    {

    }

    public void Initialze()
    {
        questions_Array = new Question1[5];

        for (int i = 0; i < questions_Array.Length; i++)
        {
            questions_Array[i].s_QuestionContents = dataFromLastStoryGamePlay.questions_Array[i].s_QuestionContents;
            questions_Array[i].s_Answer = dataFromLastStoryGamePlay.questions_Array[i].s_Answer;
            questions_Array[i].s_Option1 = dataFromLastStoryGamePlay.questions_Array[i].s_Option1;
            questions_Array[i].s_Option2 = dataFromLastStoryGamePlay.questions_Array[i].s_Option2;
            questions_Array[i].s_Option3 = dataFromLastStoryGamePlay.questions_Array[i].s_Option3;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            questionContents_Array[i].text = "題目: " + questions_Array[i].s_QuestionContents;

            optionContents_Array[i].text = "Q. " + questions_Array[i].s_QuestionContents + "\n" + "\n" +
            "A. " + questions_Array[i].s_Answer + "\n" +
            "B. " + questions_Array[i].s_Option1 + "\n" +
            "C. " + questions_Array[i].s_Option2 + "\n" +
            "D. " + questions_Array[i].s_Option3;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            answerRecordImages_Array[i].sprite = SetSpriteForAnswerRecords(i);
            answerRecordImages_Array[i].color = SetColorForAnserRecords(i);
        }
    }

    public Sprite SetSpriteForAnswerRecords(int index)
    {
        if (dataFromLastStoryGamePlay.answerRecords_Array[index])
        {
            return answerRecordSprites_Array[1];
        }
        else
        {
            return answerRecordSprites_Array[0];
        }
    }

    public Color SetColorForAnserRecords(int index)
    {
        if (dataFromLastStoryGamePlay.answerRecords_Array[index])
        {
            return new Color(1, 0.8061391f, 0.03301889f, 1);
        }
        else
        {
            return new Color(1, 0.03137255f, 0.6769125f, 1);
        }
    }

    public void ChangeFavorites(int questionIndex)
    {

        if (favoriteImages_Array[questionIndex].sprite == heartSprites_Array[0])
        {
            favoriteImages_Array[questionIndex].sprite = heartSprites_Array[1];
        }
        else
        {
            favoriteImages_Array[questionIndex].sprite = heartSprites_Array[0];
        }
        
    }
}
