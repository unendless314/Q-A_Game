using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateCellHandler : MonoBehaviour
{
    public JsonGateData data;
    public Button button;
    public GameObject startObj;
    public GameObject baseAObj;
    public GameObject baseBObj;
    public GameObject baseCObj;
    public GameObject baseDObj;
    public GameObject baseEObj;
    public GameObject baseFObj;
    public GameObject baseGObj;
    public GameObject baseHObj;
    public GameObject baseIObj;
    public GameObject baseJObj;
    public GameObject baseKObj;
    public GameObject baseLObj;
    public GameObject baseMObj;
    public GameObject baseNObj;
    public GameObject baseOObj;
    public GameObject basePObj;
    public GameObject baseQObj;
    public GameObject baseRObj;
    public GameObject baseSObj;
    public GameObject goalObj;

    public void SetCellContent(JsonGateData aData, System.Action<JsonGateData> aCallback)
    {
        data = aData;
        if (button != null)
        {
            button.onClick.AddListener(delegate ()
            {
                if (aCallback != null)
                    aCallback(data);
            });
        }

        SetBase(aData.i_id);
        SetLockStatus(false);
    }

    public void SetLockStatus(bool isLock)
    {
        //arrayLock[0].SetActive(isLock);
        //arrayLock[1].SetActive(!isLock);
    }

    public void SetBase(int gateNumber)
    {
        switch (gateNumber)
        {
            case 0:
                startObj.SetActive(true);
                break;
            case 1:
                baseAObj.SetActive(true);
                break;
            case 2:
                baseBObj.SetActive(true);
                break;
            case 3:
                baseCObj.SetActive(true);
                break;
            case 4:
                baseDObj.SetActive(true);
                break;
            case 5:
                baseEObj.SetActive(true);
                break;
            case 6:
                baseFObj.SetActive(true);
                break;
            case 7:
                baseGObj.SetActive(true);
                break;
            case 8:
                baseHObj.SetActive(true);
                break;
            case 9:
                baseIObj.SetActive(true);
                break;
            case 10:
                baseJObj.SetActive(true);
                break;
            case 11:
                baseKObj.SetActive(true);
                break;
            case 12:
                baseLObj.SetActive(true);
                break;
            case 13:
                baseMObj.SetActive(true);
                break;
            case 14:
                baseNObj.SetActive(true);
                break;
            case 15:
                baseOObj.SetActive(true);
                break;
            case 16:
                basePObj.SetActive(true);
                break;
            case 17:
                baseQObj.SetActive(true);
                break;
            case 18:
                baseRObj.SetActive(true);
                break;
            case 19:
                baseSObj.SetActive(true);
                break;
            default:
                break;
        }
    }
}
