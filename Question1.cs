using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct Question1
{
    public int i_id;
    public int i_Grade;
    public int i_BigCategory;
    public int i_SmallCategory;
    public string s_QuestionContents;
    public string s_Answer, s_Option1, s_Option2, s_Option3;

    public string[] optionContents_Array;

}
