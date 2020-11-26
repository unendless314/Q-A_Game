using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class ChangeSkinHandler : GameModeController
{
    public GameObject misakiCamera;
    public GameObject closeButton;

    public Button hairDecorationButton;
    public Button hairStyleButton;
    public Button eyesStyleButton;
    public Button clothStyleButton;
    public Button setMaleButton;
    public Button setFemaleButton;
    public Button buyButton;

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();
        misakiCamera.SetActive(true);
        closeButton.SetActive(true);
    }

    public override void OnNavigationDestroy()
    {
        base.OnNavigationDestroy();
    }

    public override void OnNavigationStop()
    {
        base.OnNavigationStop();
        misakiCamera.SetActive(false);
        closeButton.SetActive(false);
    }
}
