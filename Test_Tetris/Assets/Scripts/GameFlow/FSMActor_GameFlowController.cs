using UnityEngine;
using System.Collections;
using HFSMSystem;

public class FSMActor_GameFlowController : FSMActor
{
    private static FSMActor_GameFlowController _singleton;
    public static FSMActor_GameFlowController One { get { return _singleton; }  } 

    public void Awake()
    {
        _singleton = this;
        //SetFSM
        SetFSM(FSMManager.One.GetFSM((int)FSM_ID.GameFlowFSMState));
    }
    public void Start()
    {
    }
}

