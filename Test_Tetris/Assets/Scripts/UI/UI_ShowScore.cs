using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HFSMSystem;

public class UI_ShowScore : MonoBehaviour {

    public EventProcesser BackMenuEvent;

    void Start () {
        BackMenuEvent = BackMenuEventProcesser;
        FSM GameFlowFSM = FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState);
        if (!GameFlowFSM.IsEventRepeat("GameOverEvent"))
        {
            FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState).AddEventProcesser("BackMenu", BackMenuEvent);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BackMenuBtnFunction()
    {
        BackMenuEventProcesser(FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState), FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState).GetActorFirst().gameObject, null);
    }

    void BackMenuEventProcesser(FSM FSM, GameObject Sender, object MsgData)
    {
        ((FSM_GameFlow)FSM).IsBackMenu = true;
    }
}
