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

    public static int TestValue;    // 多餘程式碼，練習時用到，日後待刪除
    
    public static void Initialize(int howManyQuestion, int howManyLives)
    {
        score = 0;
        lives = howManyLives;
        questionNumbers = howManyQuestion;
        wrongAnswerTimes = 0;
        rightAnswerTimes = 0;
        gameOver = false;
    }

    public static void AnswerRight(int plusScore)
    {
        rightAnswerTimes += 1;
        score += plusScore;
    }

    public static void AnswerWrong(int minusScore)
    {
        wrongAnswerTimes += 1;
        score -= minusScore;
        lives -= 1;
    }

    public static void SetGameover()
    {
        gameOver = true;
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
}
