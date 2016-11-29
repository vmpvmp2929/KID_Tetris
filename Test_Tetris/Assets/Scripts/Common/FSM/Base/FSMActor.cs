using UnityEngine;
using System.Collections;


namespace HFSMSystem
{
    public class FSMActor : MonoBehaviour
    {
        #region property
        protected int _stateID_Prev = FSM.StateID_Invalid;     // State ID of prevous state
        protected int _stateID_Curr = FSM.StateID_Invalid;     // State ID of current state
        protected int _stateID_Next = FSM.StateID_Invalid;     // State ID of next state

        protected float _stateTime = 0.0f;                    // accumulated time from entering state
        protected int _stateCounter_Frame = 0;                // count of frame from entering state
        protected int _stateCounter_Repeat = 0;               // count of entering the same state

        protected FSM _thisFSM = null;
        protected FSM_State _currentState= FSM.BlankState;    // current state
        #endregion

        #region Mono Function
        public virtual void Update()
        {
            StateUpdate();
        }
        public virtual void OnDestroy()
        {
            if (_thisFSM != null)
                _thisFSM.RemoveStateObject(this);
        }
        #endregion

        #region Get And Set
        public void SetFSM(FSM fsm)
        {
            if (fsm == null)
            {
                Debug.Log("FSM is NUll");
                return;
            }
            _thisFSM = fsm;
            fsm.AddActor(this);
        }
        public FSM_State GetCurrentState() { return _currentState; }
        public void SetCurrentState(FSM_State FSMState) { _currentState = FSMState; }
        public int GetNextStateID() { return _stateID_Next; }
        public int GetCurrentStateID() { return _stateID_Curr; }
        public float GetStateTime() { return _stateTime; }
        public int GetStateCounter_Frame() { return _stateCounter_Frame; }
        public int GetStateCounter_Repeat() { return _stateCounter_Repeat; }
        #endregion

        #region StateUpdate Function
        public bool ChangeState(int nextStateID)
        {
            // determine whether state id is valid or not
            if ((_thisFSM.GetState(nextStateID) ==null)|| (nextStateID == FSM.StateID_Invalid))
            {
                Debug.LogError("Invalid state ID");
                return false;
            }

            // compared with state priority
            if (this._stateID_Next != FSM.StateID_Invalid)
            {
                if (_thisFSM.GetState(this._stateID_Next).GetPriority() > _thisFSM.GetState(nextStateID).GetPriority())
                {
                    return true;
                }
            }
            _stateID_Next= nextStateID;

            return true;
        }
        public bool ChangeState(FSM_State nextState)
        {
            // determine whether state id is valid or not
            if ((nextState == null) || (nextState.GetID() == FSM.StateID_Invalid))
            {
                Debug.LogError("Invalid state ID");
                return false;
            }

            // compared with state priority
            if (this._stateID_Next != FSM.StateID_Invalid)
            {
                if (_thisFSM.GetState(this._stateID_Next).GetPriority() > nextState.GetPriority())
                {
                    return true;
                }
            }
            _stateID_Next = nextState.GetID();

            return true;
        }

        public void StateUpdate()
        {
            this._thisFSM.PreUpdate(this);

            _stateTime += Time.deltaTime;
            ++_stateCounter_Frame;

            if (this._stateID_Next != FSM.StateID_Invalid)
            {
                // do change state

                // do Leave
                _currentState.Leave(_thisFSM,this);

                // change state
                this._stateID_Prev = this._stateID_Curr;

                this._stateID_Curr = this._stateID_Next;
                this._currentState = _thisFSM.GetState(this._stateID_Next);

                this._stateID_Next = FSM.StateID_Invalid;

                // reset property
                this._stateTime = 0.0f;
                this._stateCounter_Frame = 1;
                this._stateCounter_Repeat = (this._stateID_Curr == this._stateID_Prev) ? this._stateCounter_Repeat + 1 : 1;

                // do Enter
                this._currentState.Enter(_thisFSM, this);
            }

            // do state update
            this._currentState.Update(_thisFSM, this);

            this._thisFSM.PostUpdate(this);
        }
        #endregion
    }
}