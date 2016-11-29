using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HFSMSystem
{
    public delegate void CreateState();
    public delegate void StateContent(FSM FSM,FSMActor Owner);

    public class FSM_State
    {
        #region property
        protected static StateContent BlankContent = (FSM FSM, FSMActor Owner) => { };

        protected int ID = FSM.StateID_Invalid;
        protected int Priority = 0;

        private event StateContent Content_Enter = null;
        private event StateContent Content_Update = null;
        private event StateContent Content_Leave = null;

        private Dictionary<int, EventProcesser> EventMap = new Dictionary<int, EventProcesser>();
        #endregion

        #region Initialize
        public FSM_State(int StateID)
        {
            ID = StateID;
        }

        public void Initialize(FSM FSM, Dictionary<string,object> InitParameter, CreateState Create)
        {
            // add state to fsm
            FSM.AddState(this);

            // parse state parameter
            if (InitParameter != null)
            {
                SetParameter(InitParameter);
            }

            // create for AddContent_Enter, AddContent_Update, AddContent_Leave, AddEventProcesser
            Create();

            // set blank content if no content
            if (this.Content_Enter == null) { this.Content_Enter = BlankContent; }
            if (this.Content_Update == null) { this.Content_Update = BlankContent; }
            if (this.Content_Leave == null) { this.Content_Leave = BlankContent; }
        }

        protected virtual void SetParameter(Dictionary<string, object> Parameter)
        {
            if (Parameter.ContainsKey("Priority"))
            {
                this.Priority = (int)Parameter["Priority"];
            }
        }

        public void AddContent_Enter(StateContent Content)
        {
            this.Content_Enter += Content;
        }
        public void AddContent_Update(StateContent Content)
        {
            this.Content_Update += Content;
        }
        public void AddContent_Leave(StateContent Content)
        {
            this.Content_Leave += Content;
        }

        public bool AddEventProcesser(int Type, EventProcesser EventProcesser)
        {
            if (this.EventMap.ContainsKey(Type) == true)
            {
                Debug.LogError("EventProcesser is repeated.");
                return false;
            }
            this.EventMap.Add(Type, EventProcesser);
            return true;
        }
        #endregion

        #region Get
        public int GetID() { return ID; }
        public int GetPriority() { return Priority; }
        #endregion
        
        #region function for FSM
        public virtual void Enter( FSM fsm, FSMActor owner)
        {
            this.Content_Enter(fsm, owner);
        }
        public virtual void Update(FSM fsm, FSMActor owner)
        {
            this.Content_Update(fsm, owner);
        }
        public virtual void Leave(FSM fsm, FSMActor owner)
        {
            this.Content_Leave(fsm, owner);
        }

        public virtual void ProcessEvent(int Type, FSM FSM, GameObject Sender, object UserData)
        {
            if (this.EventMap.ContainsKey(Type) == true)
            {
                this.EventMap[Type](FSM, Sender, UserData);
            }
        }
        #endregion
    }
}