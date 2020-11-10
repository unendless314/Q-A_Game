using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evaluation : MonoBehaviour
{
    public string whatDidYouPlay;

    public StoryGamePlay lastStoryGamePlay;
    public PKGamePlay lastPKGamePlay;

    public Question1[] questions_Array;
    public Button[] expandButton_Array;
    public Button[] hideButton_Array;
    public Text[] questionContents_Array;
    public Text[] optionContents_Array;
    public Image[] favoriteImages_Array;
    public Image[] answerRecordImages_Array;
    public Sprite[] heartSprites_Array;
    public Sprite[] answerRecordSprites_Array;

    public void SetGameMode(string gamePlayMode)
    {
        whatDidYouPlay = gamePlayMode;
    }

    public void Initialize()
    {
        if (whatDidYouPlay == "StoryGamePlay")
        {
            ShowStoryGamePlayData();
        }

        if (whatDidYouPlay == "PKGamePlay")
        {
            ShowPKGamePlayData();
        }

        if (whatDidYouPlay != "StoryGamePlay" && whatDidYouPlay != "PKGamePlay")
        {
            Debug.Log("遊戲型別未定義");
            return;
        }
    }

    public void ShowStoryGamePlayData()
    {
        questions_Array = new Question1[8];

        if (lastStoryGamePlay.questions_Array.Length > 8)
        {
            Debug.Log("題目數量超過可顯示上限");
            return;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            questions_Array[i].i_id = lastStoryGamePlay.questions_Array[i].i_id;
            questions_Array[i].i_Grade = lastStoryGamePlay.questions_Array[i].i_Grade;
            questions_Array[i].i_BigCategory = lastStoryGamePlay.questions_Array[i].i_BigCategory;
            questions_Array[i].i_SmallCategory = lastStoryGamePlay.questions_Array[i].i_SmallCategory;

            questions_Array[i].s_QuestionContents = lastStoryGamePlay.questions_Array[i].s_QuestionContents;
            questions_Array[i].s_Answer = lastStoryGamePlay.questions_Array[i].s_Answer;
            questions_Array[i].s_Option1 = lastStoryGamePlay.questions_Array[i].s_Option1;
            questions_Array[i].s_Option2 = lastStoryGamePlay.questions_Array[i].s_Option2;
            questions_Array[i].s_Option3 = lastStoryGamePlay.questions_Array[i].s_Option3;
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
            answerRecordImages_Array[i].sprite = SetSpriteForStoryAnswerRecords(i);
            answerRecordImages_Array[i].color = SetColorForStoryAnswerRecords(i);
        }
    }

    public void ShowPKGamePlayData()
    {
        questions_Array = new Question1[8];

        if (lastPKGamePlay.questions_Array.Length > 8)
        {
            Debug.Log("題目數量超過可顯示上限");
            return;
        }

        for (int i = 0; i < questions_Array.Length; i++)
        {
            questions_Array[i].i_id = lastPKGamePlay.questions_Array[i].i_id;
            questions_Array[i].i_Grade = lastPKGamePlay.questions_Array[i].i_Grade;
            questions_Array[i].i_BigCategory = lastPKGamePlay.questions_Array[i].i_BigCategory;
            questions_Array[i].i_SmallCategory = lastPKGamePlay.questions_Array[i].i_SmallCategory;

            questions_Array[i].s_QuestionContents = lastPKGamePlay.questions_Array[i].s_QuestionContents;
            questions_Array[i].s_Answer = lastPKGamePlay.questions_Array[i].s_Answer;
            questions_Array[i].s_Option1 = lastPKGamePlay.questions_Array[i].s_Option1;
            questions_Array[i].s_Option2 = lastPKGamePlay.questions_Array[i].s_Option2;
            questions_Array[i].s_Option3 = lastPKGamePlay.questions_Array[i].s_Option3;
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
            answerRecordImages_Array[i].sprite = SetSpriteForPKAnswerRecords(i);
            answerRecordImages_Array[i].color = SetColorForPKAnswerRecords(i);
        }
    }

    /// <summary>

    /// </summary>

    public Sprite SetSpriteForStoryAnswerRecords(int index)  //這樣寫已經死了? 似乎可以
    {
        if (lastStoryGamePlay.playerAnswerRecords_Array[index])
        {
            return answerRecordSprites_Array[1];
        }
        else
        {
            return answerRecordSprites_Array[0];
        }
    }

    public Color SetColorForStoryAnswerRecords(int index)
    {
        if (lastStoryGamePlay.playerAnswerRecords_Array[index])
        {
            return new Color(1, 0.8061391f, 0.03301889f, 1);
        }
        else
        {
            return new Color(1, 0.03137255f, 0.6769125f, 1);
        }
    }


    public Sprite SetSpriteForPKAnswerRecords(int index)  //這樣寫已經死了? 似乎可以
    {
        if (lastPKGamePlay.playerAnswerRecords_Array[index])
        {
            return answerRecordSprites_Array[1];
        }
        else
        {
            return answerRecordSprites_Array[0];
        }
    }

    public Color SetColorForPKAnswerRecords(int index)
    {
        if (lastPKGamePlay.playerAnswerRecords_Array[index])
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
