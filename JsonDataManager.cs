using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MiniJSON;
using MayCharm.Tools;
using System.Reflection;

public class JsonDataManager : MonoBehaviour
{

	public delegate void OnLoadDelegate (string aFileName, float aProcess);

    private const string JSON_SAMPLE = "question_test1";

	private string[] arrayJsonFileNames = new string[] {
        JSON_SAMPLE
	};

	private static JsonDataManager _Instance;

	public static JsonDataManager Singleton {
		get {
			if (_Instance == null)
				_Instance = FindObjectOfType<JsonDataManager> ();
			if (_Instance == null)
				Debug.LogError ("JsonDataManager didn't add on GameObject!!");
			return _Instance;
		}
	}

    public Dictionary<int, TestQuestion1> dictTestQuestionJsonData;

	//為了方便看資料
	[SerializeField]
	private List<TestQuestion1> listTestQuestionData;

    void Start ()
	{
		DontDestroyOnLoad (this.gameObject);
        //LoadJsonData();

    }

	
    public void LoadJsonData (OnLoadDelegate process = null, System.Action done = null)
	{
		StartCoroutine (_LoadJsonData (process, done));
	}

	private IEnumerator _LoadJsonData (OnLoadDelegate aProcess, System.Action done)
	{

        int count = 0;
		foreach (string fileName in arrayJsonFileNames) {

            if (aProcess != null)
				aProcess (fileName, count == 0 ? 0 : (float)count / (float)arrayJsonFileNames.Length);
			
			TextAsset jsonText = Resources.Load<TextAsset> (string.Format ("Json/{0}", fileName));
			if (jsonText != null) {
                Dictionary<string, object> dict = Json.Deserialize (jsonText.text) as Dictionary<string, object>;
				SetJsonData (fileName, dict);
			}
			count++;
			if (aProcess != null)
				aProcess (fileName, (float)count / (float)arrayJsonFileNames.Length);
		}
		if (done != null)
			done ();
		yield return null;
	}
	


	private void SetJsonData (string aJsonName, Dictionary<string, object> aDictData)
	{
        Debug.Log("aJsonName:"+ aJsonName);
		switch (aJsonName) {
            case JSON_SAMPLE:
				JsonDataTool.SetJsonDataToDictionary(aDictData, ref dictTestQuestionJsonData);
				listTestQuestionData = new List<TestQuestion1>(dictTestQuestionJsonData.Values);
                break;
		}
	}
}
