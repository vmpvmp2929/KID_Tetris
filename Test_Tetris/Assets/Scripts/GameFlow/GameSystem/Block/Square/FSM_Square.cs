using UnityEngine;
using System.Collections;
using HFSMSystem;


namespace SquareFSM
{
[System.Flags]
public enum MoveDirection
{
    Left = 1 << 0,
    Right = 1 << 1,
    Down = 1 << 2
}
public enum SquareState_ID
{
    Wait,
    Move,
    Disappear
}

    public class FSM_Square : FSM
    {

        private const float _squareDisappearTime = 0.5f;

        public FSM_Square(int fsmId) : base(fsmId)
        {
            //create State

            ////WaitState
            //FSM_State waitState = new FSM_State((int)SquareState_ID.Wait);
            //waitState.Initialize(this, null, () => { });

            FSM_State moveState = new FSM_State((int)SquareState_ID.Move);
            moveState.Initialize(this, null, () => { });

            DisappearState disappearState = new DisappearState((int)SquareState_ID.Disappear);
            disappearState.Initialize(this, null, () => { });
        }
        #region Def State
        //DisappearState
        private class DisappearState : FSM_State
        {
            public DisappearState(int id) : base(id) { }

            public override void Enter(FSM fsm, FSMActor owner)
            {
                base.Enter(fsm, owner);
                FSMActor_Square curOwner = owner as FSMActor_Square;
                curOwner.InitDisappearFunction(_squareDisappearTime);
            }
            public override void Update(FSM fsm, FSMActor owner)
            {
                base.Update(fsm, owner);
                FSMActor_Square curOwner = owner as FSMActor_Square;
                curOwner.DisappearFunction();
                if (!curOwner.IsAlife)
                {
                    //change state
                    curOwner.ChangeState((int)SquareState_ID.Wait);
                }
            }
        }
        #endregion

        #region Function

        #endregion
    }
}