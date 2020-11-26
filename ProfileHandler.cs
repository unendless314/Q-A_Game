using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;

public class ProfileHandler : GameModeController
{
    public GameObject misakiCamera;
    public GameObject closeButton;
    public GameObject changeSkinButton;

    public override void OnNavigationStart()
    {
        base.OnNavigationStart();
        misakiCamera.SetActive(true);
        closeButton.SetActive(true);

        if (changeSkinButton.GetComponent<Button>() != null)
        {
            changeSkinButton.GetComponent<Button>().onClick.AddListener(OnChangeSkinClick);
        }
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

    private void OnChangeSkinClick()
    {
        if (currentLifeCycleState != LifeCycleState.RESUME)
            return;
        currentLifeCycleState = LifeCycleState.PAUSE;

        CurrectGamePlayMode = GameMode.CHANGESKIN;
    }
}
