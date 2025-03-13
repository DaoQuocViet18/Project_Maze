using Cysharp.Threading.Tasks;
using UnityEngine;
using static EventDefine;

public class Person : MonoBehaviour, IInteractableObject
{
    [SerializeField] private bool IsShielding = false;

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnActiveShield>(onActiveShield);
        EventDispatcher.Add<EventDefine.OnDisableShield>(onDisableShield);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnActiveShield>(onActiveShield);
        EventDispatcher.Remove<EventDefine.OnDisableShield>(onDisableShield);
    }

    private void onActiveShield(IEventParam param) => IsShielding = true;

    private void onDisableShield(IEventParam param) => IsShielding = false;

    public void InteractGoal(GameObject targetObject)
    {
        GameManager.Instance.ActiveWinGame();
    }

    public void InteractGameOver(GameObject targetObject, bool CanBeShielded)
    {
        if (IsShielding == true && CanBeShielded == true)
        {
            EventDispatcher.Dispatch(new EventDefine.OnDisableShield());
            return;
        }
        EventDispatcher.Dispatch(new EventDefine.OnLoseGame());
        GameManager.Instance.TimeStop();
    }
    public void InteractStar(GameObject targetObject)
    {
        EventDispatcher.Dispatch(new EventDefine.OnIncreaseStar());
    }
    public void InteractMoney(GameObject targetObject)
    {
        EventDispatcher.Dispatch(new EventDefine.OnIncreaseMoney { money = 1 });
    }
}
