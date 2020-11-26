using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//UI manager // Start App Controller
//GamemodeControler
//Using Maycharm.wireframe

public class PostQuestionData : MonoBehaviour
{
    public GameObject JSONDataManager;
    public List<Question3> originalList;
    public List<Question3> processedList;

    public int i_Grade, i_Category, i_Lesson, selectedCount;
    public string s_Audio;

	// Start is called before the first frame update
	void Start()
    {
        JSONDataManager = GameObject.Find("JSONDataManager");
		originalList = JSONDataManager.GetComponent<JsonDataManager>().listQuestionData;
    }

    public void SetOriginalList()
    {
		originalList = JSONDataManager.GetComponent<JsonDataManager>().listQuestionData;
	}

	public void SetGrade(int grade)
	{
		i_Grade = grade;
	}

	public void SetCategory(int category)
	{
		i_Category = category;
	}

	public void SetLesson(int lesson)
	{
		i_Lesson = lesson;
	}

	public void SetAudio(string audio)
	{
		s_Audio = audio;
	}

	/*
	public void SetProcessedList()
	{
		processedList.Clear();

		IEnumerable<Question2> selection = from obj in originalList where obj.i_Grade == (i_Grade * 2) || obj.i_Grade == (i_Grade * 2 + 1) select obj;
		foreach (var item in selection)
		{
			processedList.Add(item);
		}

		selectedCount = processedList.Count();
	}
	*/

	public void SetProcessedListWithoutArticle()
	{
		processedList.Clear();
		IEnumerable<Question3> selection = from obj in originalList select obj;

		foreach (var item in selection)
		{
			processedList.Add(item);
		}

		selection = from obj in originalList where obj.i_category == 9 || obj.i_category == 10 || obj.i_category == 11 || obj.i_category == 12 select obj;

		foreach (var item in selection)
		{
			processedList.Remove(item);
		}

		selectedCount = processedList.Count();
	}

	public void SetProcessedListWithCategoryNumber(int categoryNumber)
    {
		processedList.Clear();
		IEnumerable<Question3> selection = from obj in originalList where obj.i_category == categoryNumber select obj;

		foreach (var item in selection)
		{
			processedList.Add(item);
		}

		selectedCount = processedList.Count();
	}

	public void SetProcessedListWithArticle()
	{
		processedList.Clear();
		IEnumerable<Question3> selection = from obj in originalList where obj.i_category == 9 || obj.i_category == 10 || obj.i_category == 11 || obj.i_category == 12 select obj;

		foreach (var item in selection)
		{
			processedList.Add(item);
		}

		selectedCount = processedList.Count();
	}

	public void UseOriginalListAsProcessedList()
	{
		processedList.Clear();
		IEnumerable<Question3> selection = from obj in originalList select obj;

		foreach (var item in selection)
		{
			processedList.Add(item);
		}

		selectedCount = processedList.Count();
	}
}
