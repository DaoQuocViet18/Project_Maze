using UnityEngine;

public partial class EventDefine : IEventParam
{

    public struct OnLoadScene : IEventParam { }

    public struct OnIncreaseStar : IEventParam { }
    public struct OnIncreaseMoney : IEventParam
    {
        public int money;
    }
    public struct OnDecreaseMoney : IEventParam 
    {
        public int money;
    }
    public struct OnActiveShield : IEventParam { }
    public struct OnDisableShield : IEventParam { }

    public struct OnTutorialGame : IEventParam
    {
        public bool isTutorial;
    }
    public struct OnGamePaused : IEventParam
    {
        public bool isPaused;
    }

    public struct OnWinGame : IEventParam { }
    public struct OnLoseGame : IEventParam { }
}