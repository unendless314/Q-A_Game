using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectSQL : JsonDataManager
{
    public int i_Grade, i_BigCategory, i_SmallCategory, selectedCount;
    public List<TestQuestion1> OriginalList;
    public List<TestQuestion1> ProcessedList;

    public void SetOriginalList()
    {
        OriginalList.Clear();

        foreach (var item in GetComponent<JsonDataManager>().dictTestQuestionJsonData.Values)
        {
            OriginalList.Add(item);
        }
    }

    public void SetProcessedList()
    {
        ProcessedList.Clear();

        //ProcessedList = OriginalList.FindAll((TestQuestion1 obj) => obj.i_BigCategory == i_Grade);
        IEnumerable<TestQuestion1> kk = from obj in OriginalList where obj.i_Grade == i_Grade select obj;
        foreach (var item in kk)
        {
            ProcessedList.Add(item);
        }

        selectedCount = ProcessedList.Count();
    }

    public void SetProcessedList1()
    {
        ProcessedList.Clear();
        //SQL 指令會比較好用，因為不需要層層篩選，Find 使用兩個條件時找到的結果為聯集而非交集
        //ProcessedList = OriginalList.FindAll((TestQuestion1 obj) => obj.i_BigCategory == i_Grade && obj.i_BigCategory == i_BigCategory);
        
        IEnumerable<TestQuestion1> kk = from obj in OriginalList where obj.i_Grade == i_Grade && obj.i_BigCategory == i_BigCategory select obj;
        foreach (var item in kk)
        {
            ProcessedList.Add(item);
        }

        selectedCount = ProcessedList.Count();
    }

    public void SetProcessedList2()
    {
        ProcessedList.Clear();

        IEnumerable<TestQuestion1> kk = from obj in OriginalList where obj.i_Grade == i_Grade && obj.i_BigCategory == i_BigCategory && obj.i_SmallCategory == i_SmallCategory select obj;
        foreach (var item in kk)
        {
            ProcessedList.Add(item);
        }

        selectedCount = ProcessedList.Count();
    }

    public void UpdateQuestionBoard()
    {
        QuestionBoard.Initialize(selectedCount);
        QuestionBoard.testQuestion1_Array = ProcessedList.ToArray();
    }

    public void UpdateProcessedList()
    {
        ProcessedList.Clear();
        ProcessedList = QuestionBoard.testQuestion1_Array.ToList();
    }
}
