using System;
using UnityEngine;

public class PauseController : Singleton<PauseController> {

    private void OnEnable() {
        EventDispatcher.Add<EventDefine.OnLoadScene>(OnLoadScene);
    }

    private void OnDisable() {
        EventDispatcher.Remove<EventDefine.OnLoadScene>(OnLoadScene);
    }

    private void OnLoadScene(IEventParam param)
    {
        Resume();
    }

    public void Pause() {
        Time.timeScale = 0;
        EventDispatcher.Dispatch(new EventDefine.OnGamePaused { isPaused = true });
    }

    public void Resume() {
        Time.timeScale = 1;
        EventDispatcher.Dispatch(new EventDefine.OnGamePaused { isPaused = false });
    }
}
