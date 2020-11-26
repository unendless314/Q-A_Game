using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;
using System;
using MayCharm.Tools;


public class GradeHandler : GameModeController
{

    public List<JsonGradeData> listGradData;
    public ScrollRect scrollRect;
    public GameObject objGradeCell;
    public ClassHandler classHandler;

    public List<GradeCellHandler> listCell = new List<GradeCellHandler>();

    public int[] education = new[] { 7, 8, 9, 10, 11 }; //這邊寫死了，以後應該要改，從上層傳來的資料
    // 可以參考 ClassHandler 的寫法

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();

        listGradData.Clear();

        foreach (int id in education)
        {
             listGradData.Add(JsonDataManager.Singleton.dictJsonGradeData[id]);

            /* Jeffery 寫法，少了一個 foreach 迴圈

            JsonGradeData data = DictionaryTool.GetValue(JsonDataManager.Singleton.dictJsonGradeData, id, new JsonGradeData()); 
            
            if (data.i_id != 0)
            {
                AddGradeContent(data);
            }

            listGradData.Add(data);
            */
        }

        AddGradeContent();
    }

    private void AddGradeContent()
    {
        foreach (var item in listGradData)
        {
            GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objGradeCell);
            GradeCellHandler handler = cell.GetComponent<GradeCellHandler>();
            handler.SetCellContent(item, OnCellClickCallback);
            listCell.Add(handler);
        }
    }

    /*  Jeffery 寫法，少了一個 foreach 迴圈
    private void AddGradeContent(JsonGradeData data)
    {
        GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objGradeCell);
        GradeCellHandler handler = cell.GetComponent<GradeCellHandler>();
        handler.SetCellContent(data, OnCellClickCallback);
        listCell.Add(handler);
    }
    */

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

    private void OnCellClickCallback(JsonGradeData aData)
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        classHandler.ai_class = aData.ai_class; //  這行超重要!!

        CurrectGamePlayMode = GameMode.CLASS;
    }
}
