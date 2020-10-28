using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]  //要加這一行字
public class Question
{
    public int questionID;
    public string questionContents;
    public string[] optionContents;
    public int answerNumber;
    public string answerContents;
    public bool[] optionOrder;

    public void Initialize(int howManyOptions) //初始化，重現原始問題和原始答案
    {
        optionOrder = new bool[howManyOptions];

        for (int i = 0; i < howManyOptions; i++)
        {
            optionOrder[i] = false;

            if (i == answerNumber)
            {
                optionOrder[i] = true;
            }
        }
    }

    public void Permutation(int howManyOptions) //改變答案及選項的順序
    {
        List<string> optionListA = new List<string>();
        List<string> optionListB = new List<string>();

        List<bool> optionOrderA = new List<bool>();
        List<bool> optionOrderB = new List<bool>();

        int howManyElementsInListA; //A 集合裡面的元素數量，其實是多餘程式碼，但閱讀性可能較佳

        for (int i = 0; i < howManyOptions; i++)
        {
            optionListA.Add(optionContents[i]); //A 集合塞了所有的選項
            optionOrderA.Add(optionOrder[i]);   //A 集合塞了所有的布林
        }

        // optionListA = optionContents.ToList();  //Linq 函式庫寫法
        // optionOrderA = optionOrder.ToList();  //Linq 函式庫寫法

        while (optionListA.Count > 0)
        {
            howManyElementsInListA = optionListA.Count; //算出 A 集合裡面有幾個元素
            int randomIndex = ((int)UnityEngine.Random.Range(0, howManyElementsInListA)) % howManyElementsInListA;    //取出 A 集合中的第幾個元素
            optionListB.Add(optionListA[randomIndex]); //將此元素塞進 B 集合中
            optionOrderB.Add(optionOrderA[randomIndex]); //將此布林塞進 B 集合中

            optionListA.Remove(optionListA[randomIndex]); //移除 A 集合中剛剛取出的元素
            optionOrderA.Remove(optionOrderA[randomIndex]); //移除 A 集合中剛剛取出的布林
        }

        for (int i = 0; i < howManyOptions; i++)    //將新排序後的 B 集合元素丟回所有的選項中
        {
            optionContents[i] = optionListB[i];
            optionOrder[i] = optionOrderB[i];
        }

        //  optionContents = optionListB.ToArray(); //Linq 函式庫寫法
        // optionOrderA = optionOrderB.ToArray();  //Linq 函式庫寫法
    }

    public void FindAnswerNumber(int howManyOptions)
    {
        for (int i = 0; i < howManyOptions; i++)
        {
            if (optionOrder[i] == true)
            {
                answerNumber = i;
            }
            else
            {
                continue;
            }
        }
    }
}
