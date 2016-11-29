using UnityEngine;
using System.Collections;
using HFSMSystem;

public class FSM_Block : FSM {
    private enum BlockState_ID
    {
        Wait,
        UpdateDownTimer,
        JudgeDown,
    }
    private int _currentlevel = 1;
    private float _currentDownSpeed = FSMActor_GameSystemController.DefalutMaxDownSpeed;

    public FSM_Block(int fsmId) :base(fsmId)
    {
        //create State

        //UpdateDownTimerState
        UpdateDownTimerState updateDownTimerState = new UpdateDownTimerState((int)BlockState_ID.UpdateDownTimer);
        updateDownTimerState.Initialize(this, null, () => { });
        //JudgeDownState
        JudgeDownState judgeDownState = new JudgeDownState((int)BlockState_ID.JudgeDown);
        judgeDownState.Initialize(this, null, () => { });

        //WaitState
        FSM_State waitState = new FSM_State((int)BlockState_ID.Wait);
        waitState.Initialize(this, null, () => { });
    }
    #region Def State
    //UpdateDownTimerState
    private class UpdateDownTimerState : FSM_State
    {
        public UpdateDownTimerState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            FSM_Block curFsm = fsm as FSM_Block;
            FSMActor_Block curOwner = owner as FSMActor_Block;
            curOwner.InitDownTimer(curFsm._currentDownSpeed);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            base.Update(fsm, owner);
            FSMActor_Block curOwner = owner as FSMActor_Block;
            if(curOwner.UpdateDownTimer())
            {
                curOwner.ChangeState((int)BlockState_ID.JudgeDown);
            }
        }
    }
    //JudgeDownState
    private class JudgeDownState : FSM_State
    {
        public JudgeDownState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            FSM_Block curFsm = fsm as FSM_Block;
            FSMActor_Block curOwner = owner as FSMActor_Block;
            //Judge
            if(curOwner.JudgeDirectFunction(SquareFSM.MoveDirection.Down))
            {
                curOwner.DoMoveFunction(SquareFSM.MoveDirection.Down);
                //change state
                curOwner.ChangeState((int)BlockState_ID.UpdateDownTimer);
            }
            else
            {
                //change state
                curOwner.ChangeState((int)BlockState_ID.Wait);
            }
        }
    }

    #endregion

    #region Get And Set 
    public void SetLevel(int level){_currentlevel = level;}
    public void SetBlockDownSpeed(float downSpeed) { _currentDownSpeed = downSpeed; }
    #endregion

}
