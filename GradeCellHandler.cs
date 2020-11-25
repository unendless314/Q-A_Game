using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeCellHandler : MonoBehaviour
{
    public JsonGradeData data;
    public Button button;
    public Text labelTitle;
    public Text labelStarCount;
    public GameObject[] arrayLock;

    public void SetCellContent(JsonGradeData aData, System.Action<JsonGradeData> aCallback)
    {
        data = aData;
        if (button != null)
        {
            button.onClick.AddListener(delegate()
            {
                if (aCallback != null)
                    aCallback(data);
            });
        }
        labelTitle.text = aData.s_name;
        labelStarCount.text = "100/150";
        SetLockStatus(false);
    }

    public void SetLockStatus(bool isLock)
    {
        arrayLock[0].SetActive(isLock);
        arrayLock[1].SetActive(!isLock);
    }
}
