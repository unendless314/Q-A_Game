using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]  //要加這一行字
public class Question
{
    public int questionID;
    public string questionContents;
    public int optionNumbers;
    public string[] optionContents;
    public int answerNumber;
    public string answerContents;

    public void Initialize(int howManyOptions, int answerIndex) //初始化，重現原始問題和原始答案
    {
        optionContents = new string[howManyOptions];

        for (int i = 0; i < howManyOptions; i++)
        {
            optionContents[i] = "我是選項" + i.ToString();  //假的選項內容，實作要讀取資料庫
        }

        answerContents = optionContents[answerIndex];
    }

    public void Permutation(int howManyOptions) //改變答案及選項的順序
    {
        List<string> optionListA = new List<string>();
        List<string> optionListB = new List<string>();
        int howManyElementsInListA; //A 集合裡面的元素數量，其實是多餘程式碼，但閱讀性可能較佳

        for (int i = 0; i < howManyOptions; i++)
        {
            optionListA.Add(optionContents[i]); //A 集合塞了所有的選項
        }

        // optionListA = optionContents.ToList();  //Linq 函式庫寫法

        while (optionListA.Count > 0)
        {
            howManyElementsInListA = optionListA.Count; //算出 A 集合裡面有幾個元素
            int randomIndex = ((int)UnityEngine.Random.Range(0, howManyElementsInListA)) % howManyElementsInListA;    //取出 A 集合中的第幾個元素
            optionListB.Add(optionListA[randomIndex]); //將此元素塞進 B 集合中
            optionListA.Remove(optionListA[randomIndex]); //移除 A 集合中剛剛取出的元素
        }

        for (int i = 0; i < howManyOptions; i++)    //將新排序後的 B 集合元素丟回所有的選項中
        {
            optionContents[i] = optionListB[i];
        }

        //  optionContents = optionListB.ToArray(); //Linq 函式庫寫法
    }


    /*
    
    找答案的功能似乎目前沒用到

    */

    public void FindAnswerNumber(int howManyOptions)
    {
        for (int i = 0; i < howManyOptions; i++)
        {
            if (optionContents[i] == answerContents)
            {
                answerNumber = i;
                break;
            }
            else
            {
                continue;
            }
        }
    }
}
