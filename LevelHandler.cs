using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class LevelHandler : GameModeController
{
    public enum LevelStatus
    {
        ELE,
        JUN,
        SEN,
    }

    public Button btn_Junior;
    public Button btn_ele;

    protected override void Init()
    {
        base.Init();
        if (btn_Junior != null)
        {
            btn_Junior.onClick.AddListener(delegate()
            {
                SelectLevel(LevelStatus.JUN);
            });
        }

        if (btn_ele != null)
        {
            btn_ele.onClick.AddListener(delegate ()
            {
                SelectLevel(LevelStatus.ELE);
            });
        }
    }

    private void SelectLevel(LevelStatus aLevel)
    {
        //GameDataManager.Singleton.levelStatus = aLevel;
        //CurrectGamePlayMode = GameMode.GRADE;
    }
}
