using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Level : MonoBehaviour
{
    [SerializeField] public int MaxPoint = 3;
    //[SerializeField] BubbleCharacter character;

    public void onWinGame(Action callback)
    {
        callback?.Invoke();
        //character.animGoUp(callback);
    }

    public void onLostGame(Action callback)
    {
        callback?.Invoke();
        //character.animGoUp(callback);
    }
}
