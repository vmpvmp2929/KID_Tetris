using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HFSMSystem;

public class UI_GamePlaying : MonoBehaviour {

    public Text ScoreText;

    private int GameScore = 0;

    public EventProcesser GameOverEvent;

    void Start () {
        GameOverEvent = GameOverEventProcesser;
        FSM GameFlowFSM = FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState);
        if (!GameFlowFSM.IsEventRepeat("GameOverEvent"))
        {
            FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState).AddEventProcesser("GameOverEvent", GameOverEvent);
        }
    }

    void Update()
    {
        //GameOverEventProcesser(FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState), FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState).GetObjectList()[0].gameObject, null);

    }

    void GameOverEventProcesser(FSM FSM, GameObject Sender, object MsgData)
    {
        ((FSM_GameFlow)FSM).IsGameOver = true;
    }
}
