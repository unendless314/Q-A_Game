using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestionBoard
{
    public static TestQuestion1[] testQuestion1_Array;
    public static int Aloha;

    public static void Initialize(int count)
    {
        testQuestion1_Array = new TestQuestion1[count];
    }
}

