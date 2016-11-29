using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HFSMSystem
{
    public enum FSM_ID
    {
        GameFlowFSMState,
        GameSystemFSMState,
        SquareState,
        Max
    }
    public class FSMManager : MonoBehaviour
    {
        #region property
        private static FSMManager _singleton;
        public static FSMManager One { get { return _singleton; } set { _singleton = value; } }
        private Dictionary<int, FSM> FSMMap = new Dictionary<int, FSM>();  // FSMMap<ID, FSM>
        #endregion

        #region Mono Function
        void Awake()
        {
            One = this;           
            AddFSM(new FSM_GameFlow((int)FSM_ID.GameFlowFSMState));
            AddFSM(new FSM_GameSystem((int)FSM_ID.GameSystemFSMState));
        }
        #endregion

        #region Add Function
        private bool AddFSM(FSM NewFSM)
        {
            int FSMID = NewFSM.GetID();

            // determine whether state id is valid or not
            if (this.FSMMap.ContainsKey(FSMID))
            {
                Debug.LogError("FSMID is repeated.");
                return false;
            }

            this.FSMMap.Add(FSMID, NewFSM);            
            return true;
        }
        #endregion

        #region Get
        public FSM GetFSM(int FSMID)
        {
            return FSMMap[FSMID];
        }
        #endregion
    }
}