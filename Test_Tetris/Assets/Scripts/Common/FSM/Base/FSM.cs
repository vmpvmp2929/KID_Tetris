using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HFSMSystem
{
    public delegate void EventProcesser(FSM FSM, GameObject Sender, object MsgData);

    public abstract class FSM
    {
        #region property
        protected int _iD = StateID_Invalid;

        private class State_Invalid : FSM_State
        {
            public State_Invalid() : base(FSM.StateID_Invalid)
            {
                this.AddContent_Enter(FSM_State.BlankContent);
                this.AddContent_Update(FSM_State.BlankContent);
                this.AddContent_Leave(FSM_State.BlankContent);
            }
        }
        public static FSM_State BlankState = new State_Invalid();
        public const int StateID_Invalid = int.MinValue;
        private FSM_State _defaultInitState = BlankState;
        

        private Dictionary<int, FSM_State> StateMap = new Dictionary<int, FSM_State>();  // StateMap<StateID, State>
        private Dictionary<string, EventProcesser> EventMap = new Dictionary<string, EventProcesser>();

        protected List<FSMActor> _actorList = new List<FSMActor>();
               
        #endregion

        public FSM(int FSMID)
        {
            _iD = FSMID;
        }

        #region Add And Remove Function
        public bool AddState(FSM_State newState)
        {
            int stateID = newState.GetID();

            // determine whether state id is valid or not
            if (this.StateMap.ContainsKey(stateID))
            {
                Debug.LogError("StateID is repeated.");
                return false;
            }

            this.StateMap.Add(stateID, newState);

            // this state is InitState if it's the first one
            if (this.StateMap.Count == 1)
            {
                SetDefaultInitState(newState);
            }
            return true;
        }
        public bool AddActor(FSMActor newActor)
        {
            newActor.ChangeState(_defaultInitState);

            _actorList.Add(newActor);
            return true;
        }
        public void RemoveStateObject(FSMActor removeObject)
        {
            _actorList.Remove(removeObject);
        }
        public bool AddEventProcesser(string Type, EventProcesser eventProcesser)
        {
            if(IsEventRepeat(Type))
            { 
                Debug.LogError("EventProcesser is repeated.");
                return false;
            }
            this.EventMap.Add(Type, eventProcesser);
            return true;
        }
        #endregion

        #region function for FSM
        public virtual void PreUpdate(FSMActor Owner) { }
        public virtual void PostUpdate(FSMActor Owner) { }
        #endregion

        #region Get And Set
        public int GetID() { return _iD; }
        public List<FSMActor> GetActorList() { return _actorList; }
        public FSMActor GetActorFirst() { return _actorList[0]; }
        public FSM_State GetState(int stateID)
        {
            if(StateMap.ContainsKey(stateID))
                return StateMap[stateID];
            else
            {
                Debug.LogError("This StateID is Null");
                return null;
            }
        }
        
        public bool IsEventRepeat(string type)
        {
            return this.EventMap.ContainsKey(type) ;
        }
        protected bool SetDefaultInitState(FSM_State newState)
        {
            // determine whether state id is valid or not
            if (newState.GetID() == StateID_Invalid)
            {
                Debug.LogError("Invalid state ID");
                return false;
            }
            _defaultInitState = newState;
            return true;
        }
        #endregion
    }
}
