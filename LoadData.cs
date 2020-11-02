using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour
{
    public GameObject display;

    // Start is called before the first frame update
    void Start()
    {
        //display = GameObject.Find("DisplayJSON");

        //StreamReader reader = new StreamReader(Path.Combine("Assets/Resource", "test3.json"));

        //int num = reader.Read();
        //Debug.Log(num);

        //string helloIO = reader.ReadToEnd();

        //display.GetComponent<Text>().text = helloIO;

        JsonDataManager.Singleton.LoadJsonData(null, delegate()
        {
            Debug.Log("LoadData");
        });

        JsonDataManager.Singleton.LoadJsonData(null, delegate ()
        {
            Debug.Log("Test");
        });
    }
}
