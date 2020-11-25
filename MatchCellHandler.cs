using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCellHandler : MonoBehaviour
{
    public JsonMatchData data;
    public Button button;
    public Text ranking;
    public Text level;
    public Text idNickName;

    public void SetCellContent(JsonMatchData aData, System.Action<JsonMatchData> aCallback)
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

        ranking.text = "No. " + aData.i_Ranking.ToString(); 
        level.text = "LV. " + aData.i_Level.ToString();
        idNickName.text = aData.s_IDNickName;
    }
}
