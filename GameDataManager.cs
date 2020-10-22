using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager _Instance;

    private GameDataManager()
    {

    }   //自己追加

    public static GameDataManager Singleton
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameDataManager>();
            }
            return _Instance;
        }

        
    }

    public List<Question> listQuestion = new List<Question>();
}
