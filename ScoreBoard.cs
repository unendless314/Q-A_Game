using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreBoard
{
    private static int score;
    private static int lives;
    private static int questionNumbers;
    private static int wrongAnswerTimes;
    private static int rightAnswerTimes;
    private static bool gameOver;

    private static int feverCounter;
    private static float timeLimit;
    private static float[] seconds;
    private static bool[] answerRecords;

    public static int TestValue;    // 多餘程式碼，練習時用到，日後待刪除
    
    public static void Initialize(int howManyQuestion, int howManyLives, float countDownSeconds)
    {
        score = 0;
        lives = howManyLives;
        questionNumbers = howManyQuestion;
        wrongAnswerTimes = 0;
        rightAnswerTimes = 0;
        gameOver = false;

        feverCounter = 0;
        timeLimit = countDownSeconds;
        seconds = new float[howManyQuestion];
        answerRecords = new bool[howManyQuestion];

        for (int i = 0; i < howManyQuestion; i++)
        {
            seconds[i] = 0;
            answerRecords[i] = false;
        }
    }

    public static void AnswerRight(int plusScore, int currentQuestionNumber)
    {
        rightAnswerTimes += 1;
        score += plusScore;
        SetAnswerRecords(currentQuestionNumber);
        SetFeverCounter();
        GetAnswerRecords(currentQuestionNumber);
        GetFeverCounter();
    }

    public static void AnswerWrong(int minusScore)
    {
        wrongAnswerTimes += 1;
        score -= minusScore;
        lives -= 1;
        ResetFeverCounter();
        GetFeverCounter();
    }

    public static void SetGameover()
    {
        gameOver = true;
    }

    public static void SetFeverCounter()
    {
        feverCounter += 1;
    }

    public static void ResetFeverCounter()
    {
        feverCounter = 0;
    }

    public static void SetSeconds(int currentQuestionNumber, float timeDelataTime)
    {
        seconds[currentQuestionNumber] += timeDelataTime;
    }

    public static void SetAnswerRecords(int currentQuestionNumber)
    {
        answerRecords[currentQuestionNumber] = true;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetLives()
    {
        return lives;
    }

    public static int GetQuestionNumbers()
    {
        return questionNumbers;
    }

    public static int GetWrongAnswerTimes()
    {
        return wrongAnswerTimes;
    }

    public static int GetRightAnswerTimes()
    {
        return rightAnswerTimes;
    }

    public static bool IsGameOver()
    {
        return gameOver;
    }

    public static int GetFeverCounter()
    {
        return feverCounter;
    }

    public static float GetTimeLimit()
    {
        return timeLimit;
    }

    public static float GetSeconds(int currentQuestionNumber)
    {
        return seconds[currentQuestionNumber];
    }

    public static bool GetAnswerRecords(int currentQuestionNumber)
    {
        return answerRecords[currentQuestionNumber];
    }
}
