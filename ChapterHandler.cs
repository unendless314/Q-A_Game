using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class ChapterHandler : GameModeController
{
    public List<JsonChapterData> listChapterData;
    public ScrollRect scrollRect;
    public GameObject objChapterCell_Down;
    public GameObject objChapterCell_Up;

    public List<ChapterCellHandler> listCell = new List<ChapterCellHandler>();

    public int[] ai_chapter;

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();

        listChapterData.Clear();

        foreach (var id in ai_chapter)   //
        {
            listChapterData.Add(JsonDataManager.Singleton.dictJsonChapterData[id]);
        }

        AddChapterContent();
    }

    private void AddChapterContent()
    {
        /*
        foreach (JsonChapterData data in listChapterData)
        {
            GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objChapterCell_Down);
            ChapterCellHandler handler = cell.GetComponent<ChapterCellHandler>();
            handler.SetCellContent(data, OnCellClickCallback);
            listCell.Add(handler);
        }
        */

        for (int i = 0; i < listChapterData.Count; i++)
        {
            if (i % 2 == 0)
            {
                GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objChapterCell_Down);
                ChapterCellHandler handler = cell.GetComponent<ChapterCellHandler>();
                handler.SetCellContent(listChapterData[i], OnCellClickCallback);
                listCell.Add(handler);
            }
            else
            {
                GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objChapterCell_Up);
                ChapterCellHandler handler = cell.GetComponent<ChapterCellHandler>();
                handler.SetCellContent(listChapterData[i], OnCellClickCallback);
                listCell.Add(handler);
            }
        }
    }

    public override void OnNavigationDestroy()
    {
        UnityTool.RemoveAllChild(scrollRect.content.gameObject);
        listCell.Clear();
        base.OnNavigationDestroy();
    }

    public override void OnNavigationStop()
    {
        base.OnNavigationStop();
    }

    private void OnCellClickCallback(JsonChapterData aData)
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        //classHandler.ai_class = aData.ai_class; //  這行超重要!!

        CurrectGamePlayMode = GameMode.QUESTION;
    }


    /*
    private void OnCellClickCallback(JsonMapData aData)
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        //以下為自己追加 11/13，可能非必要

        postQuestionData.SetGrade(aData.i_Grade);
        postQuestionData.SetLesson(aData.i_Lesson);
        postQuestionData.SetCategory(aData.i_Category);
        postQuestionData.SetProcessedListWithoutArticle();

        SetStoryGamePlayData(aData);

        CurrectGamePlayMode = GameMode.QUESTION;
    }

    private void SetStoryGamePlayData(JsonMapData aData)
    {
        questionHandler.baseImage.color = SetBaseColor(aData.i_Category);
        questionHandler.baseText.text = SetBaseText(aData.i_Category);


        questionHandler.SelectCategory(aData.i_Category);

        if (GameDataManager.Singleton.categoryStatus == QuestionHandler.CategoryStatus.NoArticle)
        {
            //
        }
        else
        {
            //
        }

        postQuestionData.SetProcessedListWithCategoryNumber(aData.i_Category);

        questionHandler.processedList = postQuestionData.processedList;
        questionHandler.databaseQuestionNumbers = postQuestionData.selectedCount;
        questionHandler.questionNumbers = aData.i_QuestionNumbers;
        questionHandler.timeLimit = 10;
        questionHandler.optionNumbers = 4;

    }

    public Color SetBaseColor(int categoryNumber)
    {
        switch (categoryNumber)
        {
            case 1:
            case 2:
            case 3:
                return new Color(0, 0.6516128f, 0);
            case 4:
            case 5:
                return new Color(0.4811321f, 0.4811321f, 0.4811321f);
            case 6:
            case 7:
                return new Color(1, 0.6185567f, 0);
            case 8:
                return new Color(0.2532582f, 0.7924528f, 0);
            case 9:
            case 10:
            case 11:
                return new Color(0.8554854f, 0.3820755f, 0);
            case 12:
                return new Color(0.01103847f, 0, 0.6792453f);
            default:
                return new Color();
        }
    }

    public string SetBaseText(int categoryNumber)
    {
        switch (categoryNumber)
        {
            case 1:
            case 2:
            case 3:
                return "單字";
            case 4:
            case 5:
                return "文法";
            case 6:
            case 7:
                return "翻譯";
            case 8:
                return "聽力";
            case 9:
            case 10:
            case 11:
                return "閱讀";
            case 12:
                return "克漏字";
            default:
                return "其他類別";
        }
    }
    */
}
