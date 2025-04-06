using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class LoadingbarUI : MonoBehaviour
{
    public Slider progressBar; // Thanh tiến trình UI

    public async UniTask LoadSceneAsync(SceneName sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName.ToString());
        operation.allowSceneActivation = false; // Chặn chuyển cảnh ngay lập tức

        while (operation.progress < 0.9f)
        {
            progressBar.value = Mathf.Clamp01(operation.progress / 0.9f); // Chuẩn hóa giá trị
            await UniTask.Yield(); // Chờ frame tiếp theo
        }

        progressBar.value = 1f; // Đảm bảo thanh đầy 100%
        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true);  // Chờ 1s trước khi vào cảnh mới

        operation.allowSceneActivation = true; // Chuyển sang cảnh mới
    }
}
