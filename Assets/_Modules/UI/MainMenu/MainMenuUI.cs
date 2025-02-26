﻿using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    private void Start()
    {
        playButton.onClick.AddListener(() => {
            Loader.Instance.LoadWithFade(SceneName.SelectLevelScene);
            //Loader.Instance.LoadWithFade(SceneName.GameScene);
        });
        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });

    }
}