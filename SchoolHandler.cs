using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class SchoolHandler : GameModeController
{
    public enum SchoolStatus
    {
        ELEMENTARY,
        JUNIOR,
        SENIOR,
    }

    public Button button_Elementary;
    public Button button_Junior;
    public Button button_Senior;

    protected override void Init()
    {
        base.Init();

        if (button_Elementary != null)
        {
            button_Elementary.onClick.AddListener(delegate ()
            {
                SelectSchool(SchoolStatus.ELEMENTARY);
            });
        }

        if (button_Junior != null)
        {
            button_Junior.onClick.AddListener(delegate ()
            {
                SelectSchool(SchoolStatus.JUNIOR);
            });
        }

        if (button_Senior != null)
        {
            button_Senior.onClick.AddListener(delegate ()
            {
                SelectSchool(SchoolStatus.SENIOR);
            });
        }
    }

    private void SelectSchool(SchoolStatus aSchoolType)
    {
        GameDataManager.Singleton.schoolStatus = aSchoolType;
        CurrectGamePlayMode = GameMode.GRADE; 
    }
}
