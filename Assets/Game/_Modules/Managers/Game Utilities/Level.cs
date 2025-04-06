using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int currentStars = 0;
    public int MaxStars = 3;

    public void IncreaseStar()
    {
        if (currentStars < MaxStars)
        {
            currentStars++;
            Debug.Log("Current Stars: " + currentStars);
        }
    }

    public void ResetStars()
    {
        currentStars = 0; // Reset số sao khi bắt đầu level mới
    }

    public void OnWinGame(Action callback)
    {
        callback?.Invoke();
    }

    public void OnLostGame(Action callback)
    {
        callback?.Invoke();
    }
}
