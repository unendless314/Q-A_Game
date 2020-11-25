using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterCellHandler : MonoBehaviour
{
    public JsonChapterData data;
    public Button button;
    public GameObject startObj;
    public GameObject baseAObj;
    public GameObject baseBObj;
    public GameObject baseCObj;
    public GameObject baseDObj;
    public GameObject baseEObj;
    public GameObject baseFObj;
    public GameObject goalObj;

    public void SetCellContent(JsonChapterData aData, System.Action<JsonChapterData> aCallback)
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
        

        SetBase(aData.ai_category[0]);
        SetLockStatus(false);
    }

    public void SetLockStatus(bool isLock)
    {
        //arrayLock[0].SetActive(isLock);
        //arrayLock[1].SetActive(!isLock);
    }

    public void SetBase(int categoryNumber)
    {
        switch (categoryNumber)
        {
            case 0:
                startObj.SetActive(true);
                break;
            case 1:
            case 2:
            case 3:
                baseAObj.SetActive(true);
                break;
            case 4:
            case 5:
                baseBObj.SetActive(true);
                break;
            case 6:
            case 7:
                baseCObj.SetActive(true);
                break;
            case 8:
                baseDObj.SetActive(true);
                break;
            case 9:
            case 10:
            case 11:
                baseEObj.SetActive(true);
                break;
            case 12:
                baseFObj.SetActive(true);
                break;
            case 13:
                goalObj.SetActive(true);
                break;
            default:
                break;
        }
    }
}
