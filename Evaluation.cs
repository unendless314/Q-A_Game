using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evaluation : MonoBehaviour
{
    //public StoryGamePlay dataFromLastStoryGamePlay;
    public PKGamePlay dataFromLastPKGamePlay;

    public Question1[] questions_Array;
    public Button[] expandButton_Array;
    public Button[] hideButton_Array;
    public Text[] questionContents_Array;
    public Text[] optionContents_Array;
    public Image[] favoriteImages_Array;
    public Image[] answerRecordImages_Array;
    public Sprite[] heartSprites_Array;
    public Sprite[] answerRecordSprites_Array;

    public void Initialze()
    {
        questions_Array = new Question1[8];

        if (dataFromLastPKGamePlay.questions_Array.Length > 8)
        {
            Debug.Log("題目數量超過可顯示上限");
            return;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            questions_Array[i].s_QuestionContents = dataFromLastPKGamePlay.questions_Array[i].s_QuestionContents;
            questions_Array[i].s_Answer = dataFromLastPKGamePlay.questions_Array[i].s_Answer;
            questions_Array[i].s_Option1 = dataFromLastPKGamePlay.questions_Array[i].s_Option1;
            questions_Array[i].s_Option2 = dataFromLastPKGamePlay.questions_Array[i].s_Option2;
            questions_Array[i].s_Option3 = dataFromLastPKGamePlay.questions_Array[i].s_Option3;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            questionContents_Array[i].text = "題目" + (i + 1).ToString() + ": " + questions_Array[i].s_QuestionContents;

            optionContents_Array[i].text = "Q. " + questions_Array[i].s_QuestionContents + "\n" + "\n" +
            "A. " + questions_Array[i].s_Answer + "\n" +
            "B. " + questions_Array[i].s_Option1 + "\n" +
            "C. " + questions_Array[i].s_Option2 + "\n" +
            "D. " + questions_Array[i].s_Option3;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            answerRecordImages_Array[i].sprite = SetSpriteForAnswerRecords(i);
            answerRecordImages_Array[i].color = SetColorForAnswerRecords(i);
        }
    }

    public Sprite SetSpriteForAnswerRecords(int index)
    {
        if (dataFromLastPKGamePlay.playerAnswerRecords_Array[index])
        {
            return answerRecordSprites_Array[1];
        }
        else
        {
            return answerRecordSprites_Array[0];
        }
    }

    public Color SetColorForAnswerRecords(int index)
    {
        if (dataFromLastPKGamePlay.playerAnswerRecords_Array[index])
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
