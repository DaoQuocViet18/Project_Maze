using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialogue : MonoBehaviour {
    public static ConfirmDialogue Create() {
        return Instantiate(Resources.Load<GameObject>("pfConfirmDialogue")).GetComponent<ConfirmDialogue>();
    }

    [SerializeField] private Transform confirmDialogueUI;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private CanvasGroup canvasGroup;

    private UniTaskCompletionSource<bool> dialogResult;


    private void OnEnable() {
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDisable() {
        yesButton.onClick.RemoveListener(OnYesButtonClick);
        noButton.onClick.RemoveListener(OnNoButtonClick);
    }

    private void OnYesButtonClick() {
        if (!dialogResult.TrySetResult(true)) {
            Debug.LogError("Failed to set dialog result");
        }
        Hide();
    }

    private void OnNoButtonClick() {
        if (!dialogResult.TrySetResult(false)) {
            Debug.LogError("Failed to set dialog result");
        }
        Hide();
    }

    public UniTask<bool> Show() {
        dialogResult = new UniTaskCompletionSource<bool>();
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.one;
        return dialogResult.Task;
    }

    public void Hide() {
        UtilClass.PlayTransformFadeOutAnimation(confirmDialogueUI, canvasGroup, () => {
            Destroy(gameObject);
        }, 1.15f, 0.15f, 0.15f);
    }
}
