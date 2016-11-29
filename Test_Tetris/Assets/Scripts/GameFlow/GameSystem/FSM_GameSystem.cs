using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HFSMSystem;

public class FSM_GameSystem : FSM {

    private enum GameSystemState_ID
    {
        Init,
        Ready,
        SpawnNewBlock,
        Wait,
        JudgeEliminate,
        Eliminate
    }

    #region property

    private bool _isGameOver = false;
    public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }
    #endregion


    public FSM_GameSystem(int fsmID) : base(fsmID)
    {
        //create State

        //Init
        InitState state_Init = new InitState((int)GameSystemState_ID.Init);
        state_Init.Initialize(this, null, () => { });

        //Ready
        ReadyState state_Ready = new ReadyState((int)GameSystemState_ID.Ready);
        state_Ready.Initialize(this, null, () => { });

        //SpawnNewBlock
        SpawnNewBlockState state_SpawnNewBlock = new SpawnNewBlockState((int)GameSystemState_ID.SpawnNewBlock);
        state_SpawnNewBlock.Initialize(this, null, () => { });

        //Wait
        WaitState state_Wait = new WaitState((int)GameSystemState_ID.Wait);
        state_Wait.Initialize(this, null, () => { });

        //JudgeEliminate
        JudgeEliminateState state_JudgeEliminate = new JudgeEliminateState((int)GameSystemState_ID.JudgeEliminate);
        state_JudgeEliminate.Initialize(this, null, () => { });

        //Eliminate
        EliminateState state_Eliminate = new EliminateState((int)GameSystemState_ID.Eliminate);
        state_Eliminate.Initialize(this, null, () => { });
    }

    #region Def State
    //InitState
    private class InitState : FSM_State
    {
        public InitState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            //Open UI GameFlow_GamePlaying
            GameObject GamePlayUIObj = UIManager.One.GetUIObject(UIManager.UIModule.GameFlow_GamePlaying);
            if (GamePlayUIObj != null)
                GamePlayUIObj.gameObject.SetActive(true);
            else
                UIManager.One.OpenUI(UIManager.UIModule.GameFlow_GamePlaying);
            //Change State
            owner.ChangeState((int)GameSystemState_ID.Ready);
        }
    }
    //ReadyState
    private class ReadyState : FSM_State
    {
        public ReadyState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            FSM_GameSystem curFsm = fsm as FSM_GameSystem;
            FSMActor_GameSystemController curOwner = owner as FSMActor_GameSystemController;

            curOwner.GameTimer = FSMActor_GameSystemController.ReadyTime;
            //Open UI GamePlay_Ready
            UIManager.One.OpenUI(UIManager.UIModule.GamePlay_Ready);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            base.Update(fsm, owner);
            FSM_GameSystem curFsm = fsm as FSM_GameSystem;
            FSMActor_GameSystemController curOwner = owner as FSMActor_GameSystemController;
            if (curOwner.GameTimer > 0)
                curOwner.GameTimer -= Time.deltaTime;
            if(curOwner.GameTimer<0)
            {
                curOwner.GameTimer = 0;
                //Change State
                owner.ChangeState((int)GameSystemState_ID.SpawnNewBlock);
            }
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            base.Leave(fsm, owner);
            //Close UI GamePlay_Ready
            UIManager.One.CloseUI(UIManager.UIModule.GamePlay_Ready);
        }
    }
    //SpawnNewBlockState
    private class SpawnNewBlockState : FSM_State
    {
        public SpawnNewBlockState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            //SpawnNewBlock

            //UpdateNextBlockUI

            //Change State
            owner.ChangeState((int)GameSystemState_ID.Wait);
        }
    }
    //WaitState
    private class WaitState : FSM_State
    {
        public WaitState(int id) : base(id) { }

        public override void Update(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            //Wait for Block Down

            //Change State
            owner.ChangeState((int)GameSystemState_ID.JudgeEliminate);
        }
    }
    //JudgeEliminateState
    private class JudgeEliminateState : FSM_State
    {
        public JudgeEliminateState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            FSM_GameSystem curFsm = fsm as FSM_GameSystem;
            FSMActor_GameSystemController curOwner = owner as FSMActor_GameSystemController;
            //Judge GameOver
            if(curOwner.JudgeGameOverFunction())
            {
                curFsm.IsGameOver = true;
                return;
            }

            //Do Judge Eliminate
            curOwner.JudgeEliminateFunction();
            if (curOwner.GetEliminateRowList.Count > 0)
            {
                //Change State
                owner.ChangeState((int)GameSystemState_ID.Eliminate);
            }
            else
            {
                //Change State
                owner.ChangeState((int)GameSystemState_ID.SpawnNewBlock);
            }
        }
    }
    //EliminateState
    private class EliminateState : FSM_State
    {
        public EliminateState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            FSM_GameSystem curFsm = fsm as FSM_GameSystem;
            FSMActor_GameSystemController curOwner = owner as FSMActor_GameSystemController;
            //Do Eliminate
            curOwner.DoEliminate();
            //Change State
            owner.ChangeState((int)GameSystemState_ID.SpawnNewBlock);
        }
    }
    #endregion



    #region Function 

    #endregion
}
