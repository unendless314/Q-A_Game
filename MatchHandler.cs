using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;
using System;

public class MatchHandler : GameModeController
{
    public JsonMatchData[] arrayMatchData;
    public ScrollRect scrollRect;
    public GameObject objMatchCell;

    // 不知道這樣寫好不好 11/13
    public PostQuestionData postQuestionData;
    public ArenaHandler arenaHandler;

    public List<MatchCellHandler> listCell = new List<MatchCellHandler>();

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();
        AddMatchContent();
    }

    private void AddMatchContent()
    {
        foreach (var item in arrayMatchData)
        {
            GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objMatchCell);
            MatchCellHandler handler = cell.GetComponent<MatchCellHandler>();
            handler.SetCellContent(item, OnCellClickCallback);
            listCell.Add(handler);
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

    private void OnCellClickCallback(JsonMatchData aData)
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;
        CurrectGamePlayMode = GameMode.ARENA;

        SetArenaGamePlayData(aData);
    }

    private void SetArenaGamePlayData(JsonMatchData aData)
    {
        arenaHandler.playerLevelText.text = "啦啦啦";   //這裡要接資料庫
        arenaHandler.playerNickNameText.text = "LV.33";    //這裡要接資料庫
        arenaHandler.aILevelText.text = "LV." + aData.i_Level.ToString();
        arenaHandler.aINickNameText.text = aData.s_IDNickName;

        postQuestionData.SetProcessedListWithoutArticle();
        arenaHandler.processedList = postQuestionData.processedList;
        arenaHandler.databaseQuestionNumbers = postQuestionData.selectedCount;
        arenaHandler.questionNumbers = 7;
        arenaHandler.timeLimit = 10;
        arenaHandler.optionNumbers = 4;
        arenaHandler.Initialize();
    }
}
