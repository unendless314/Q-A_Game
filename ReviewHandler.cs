using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class ReviewHandler : GameModeController
{
    public Question3[] questions_Array;
    public int[] answerNumbers_Array;
    public int[] playerChoiceNumbers_Array;
    public VerticalLayoutGroup reviewGridLayout;
    public GameObject objReviewCell;
    public GameObject toIndexButtonObj;
    public GameObject toMapButtonObj;
    public List<ReviewCellHandler> listCell = new List<ReviewCellHandler>();

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();

        if (toIndexButtonObj.GetComponent<Button>() != null)
        {
            toIndexButtonObj.GetComponent<Button>().onClick.AddListener(GoToIndex);
        }

        if (toMapButtonObj.GetComponent<Button>() != null)
        {
            toMapButtonObj.GetComponent<Button>().onClick.AddListener(GoToMap);
        }
    }

    public void AddReviewContent()
    {
        for (int i = 0; i < questions_Array.Length; i++)
        {
            GameObject reviewCell = UnityTool.AddUGUIChild(reviewGridLayout.transform, objReviewCell);
            ReviewCellHandler handler = reviewCell.GetComponent<ReviewCellHandler>();
            handler.SetCellContent(questions_Array[i], i + 1, answerNumbers_Array[i], playerChoiceNumbers_Array[i]);
            listCell.Add(handler);
        }
    }

    public override void OnNavigationDestroy()
    {
        base.OnNavigationDestroy();
    }

    public override void OnNavigationStop()
    {
        UnityTool.RemoveAllChild(reviewGridLayout.transform.gameObject);
        listCell.Clear();
        base.OnNavigationStop();
    }

    /// <summary>
    /// 下面要再改寫，暫時先這樣
    /// </summary>

    public void GoToIndex()
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;

        currentLifeCycleState = LifeCycleState.PAUSE;
        CurrectGamePlayMode = GameMode.INDEX;
    }

    public void GoToMap()
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;

        currentLifeCycleState = LifeCycleState.PAUSE;
        CurrectGamePlayMode = GameMode.MAP;
    }
}