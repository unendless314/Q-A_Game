using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class ClassHandler : GameModeController
{
    public List<JsonClassData> listClassData;
    public ScrollRect scrollRect;
    public GameObject objLessonCell;
    public GateHandler gateHandler;

    public List<ClassCellHandler> listCell = new List<ClassCellHandler>();

    public int[] ai_class;

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();

        listClassData.Clear();

        foreach (var id in ai_class)   //
        {
            listClassData.Add(JsonDataManager.Singleton.dictJsonClassData[id]);
        }

        AddClassContent();
    }

    private void AddClassContent()
    {
        foreach (JsonClassData data in listClassData)
        {
            GameObject cell = UnityTool.AddUGUIChild(scrollRect.content, objLessonCell);
            ClassCellHandler handler = cell.GetComponent<ClassCellHandler>();
            handler.SetCellContent(data, OnCellClickCallback);
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

    private void OnCellClickCallback(JsonClassData aData)
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        gateHandler.ai_gate = aData.ai_gate; //  這行超重要!!

        CurrectGamePlayMode = GameMode.GATE;
    }
}
