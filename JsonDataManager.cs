using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MiniJSON;
using MayCharm.Tools;
using System.Reflection;
using System.Linq;

public class JsonDataManager : MonoBehaviour
{

	/// <summary> 自創變數

	public int i_Grade, i_BigCategory, i_SmallCategory, selectedCount;
	public List<TestQuestion1> processedList;
	public GameObject pageObject1, pageObject2, pageObject3;

	/// </summary>

	public delegate void OnLoadDelegate(string aFileName, float aProcess);
	private const string JSON_SAMPLE = "question_test1";
	private string[] arrayJsonFileNames = new string[] {
		JSON_SAMPLE
	};

	private static JsonDataManager _Instance;
	public static JsonDataManager Singleton
	{
		get
		{
			if (_Instance == null)
				_Instance = FindObjectOfType<JsonDataManager>();
			if (_Instance == null)
				Debug.LogError("JsonDataManager didn't add on GameObject!!");
			return _Instance;
		}
	}

	public Dictionary<int, TestQuestion1> dictTestQuestionJsonData;

	//為了方便看資料
	[SerializeField]
	public List<TestQuestion1> listTestQuestionData;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		//LoadJsonData();

	}

	public void LoadJsonData(OnLoadDelegate process = null, System.Action done = null)
	{
		StartCoroutine(_LoadJsonData(process, done));
	}

	private IEnumerator _LoadJsonData(OnLoadDelegate aProcess, System.Action done)
	{

		int count = 0;
		foreach (string fileName in arrayJsonFileNames)
		{

			if (aProcess != null)
				aProcess(fileName, count == 0 ? 0 : (float)count / (float)arrayJsonFileNames.Length);

			TextAsset jsonText = Resources.Load<TextAsset>(string.Format("Json/{0}", fileName));
			if (jsonText != null)
			{
				Dictionary<string, object> dict = Json.Deserialize(jsonText.text) as Dictionary<string, object>;
				SetJsonData(fileName, dict);
			}
			count++;
			if (aProcess != null)
				aProcess(fileName, (float)count / (float)arrayJsonFileNames.Length);
		}
		if (done != null)
			done();
		yield return null;
	}

	private void SetJsonData(string aJsonName, Dictionary<string, object> aDictData)
	{
		Debug.Log("aJsonName:" + aJsonName);
		switch (aJsonName)
		{
			case JSON_SAMPLE:
				JsonDataTool.SetJsonDataToDictionary(aDictData, ref dictTestQuestionJsonData);
				listTestQuestionData = new List<TestQuestion1>(dictTestQuestionJsonData.Values);
				break;
		}
	}

	//	以下為自行追加內容

	public void SetGrade(int grade)
	{
		i_Grade = grade;

		pageObject2.SetActive(false);
		pageObject3.SetActive(true);
	}

	public void SetBigCategory(int bigCategory)
	{
		i_BigCategory = bigCategory;
	}

	public void SetSmallCategory(int smallCategory)
	{
		i_SmallCategory = smallCategory;
	}

	public void SetProcessedList()
	{
		processedList.Clear();

		IEnumerable<TestQuestion1> selection = from obj in listTestQuestionData where obj.i_Grade == i_Grade select obj;
		foreach (var item in selection)
		{
			processedList.Add(item);
		}

		selectedCount = processedList.Count();

	}

	public void StoryMode()
	{
		processedList.Clear();

		pageObject1.SetActive(false);
		pageObject2.SetActive(true);
	}

	//
}
