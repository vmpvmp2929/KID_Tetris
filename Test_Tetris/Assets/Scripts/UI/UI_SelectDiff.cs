using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HFSMSystem;

public class UI_SelectDiff : MonoBehaviour {

    public Text SelectDiffect;
    private int DifficultyValue = 1;

    public EventProcesser OkEvent;

    void Start () {
        DifficultyValue = 1;
        OkEvent = OkEventProcesser;
        FSM GameFlowFSM = FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState);
        if (!GameFlowFSM.IsEventRepeat("_selectDifficulty"))
        {
            FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState).AddEventProcesser("_selectDifficulty", OkEvent);
        }
    }	

    public void AddDiffFunction()
    {
        if (DifficultyValue < 9)
        {
            DifficultyValue++;
            UpdateText();
        }
    }
    public void ReduceDiffFunction()
    {
        if(DifficultyValue>1)
        {
            DifficultyValue--;
            UpdateText();
        }
    }
    private void UpdateText()
    {
        SelectDiffect.text = "" + DifficultyValue;
    }

    void OkEventProcesser(FSM FSM, GameObject Sender, object MsgData)
    {
        ((FSM_GameFlow)FSM).SetDifficulty((int)MsgData);
    }
    public void OkFunction()
    {
        OkEventProcesser(FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState), FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState).GetActorFirst().gameObject, DifficultyValue);
    }
}
