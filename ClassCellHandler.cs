using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClassCellHandler : MonoBehaviour
{
    public JsonClassData data;
    public Button button;
    public Text labelTitle;

    public void SetCellContent(JsonClassData aData, System.Action<JsonClassData> aCallback)
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
        labelTitle.text = aData.s_name;

        SetLockStatus(false);
    }

    public void SetLockStatus(bool isLock)
    {
        //arrayLock[0].SetActive(isLock);
        //arrayLock[1].SetActive(!isLock);
    }

}
