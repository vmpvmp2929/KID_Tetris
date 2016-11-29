using UnityEngine;
using System.Collections;
using HFSMSystem;
using UnityEngine.SceneManagement;

public class FSM_GameFlow : FSM {
    
    private enum GameFlowState_ID
    {
        Init,
        MainMenu,
        
        SelectDiff,
        GamePlaying,
        GameOver,

        ShowScore
    }
    #region property
    private int _selectDifficulty=-1;
    private bool _isGameOver = false;
    public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }
    private bool _isBackMenu = false;
    public bool IsBackMenu { get { return _isBackMenu; } set { _isBackMenu = value; } }
    #endregion

    public FSM_GameFlow(int fsmID) : base(fsmID)
    {
        // create state

        //Init
        InitState state_Init = new InitState((int)GameFlowState_ID.Init);
        state_Init.Initialize(this, null,()=> { });

        //MainMenu
        MainMenuState state_MainMenu = new MainMenuState((int)GameFlowState_ID.MainMenu);
        state_MainMenu.Initialize(this, null, () => { });

        //SelectDiff
        SelectDifficultyState state_SelectDiff = new SelectDifficultyState((int)GameFlowState_ID.SelectDiff);
        state_SelectDiff.Initialize(this, null, () => { });

        //GamePlaying
        GamePlayingState state_GamePlaying = new GamePlayingState((int)GameFlowState_ID.GamePlaying);
        state_GamePlaying.Initialize(this, null, () => { });

        //GameOver
        GameOverState state_GameOver = new GameOverState((int)GameFlowState_ID.GameOver);
        state_GameOver.Initialize(this, null, () =>{ });

        //ShowScore
        ShowScoreState state_ShowScore = new ShowScoreState((int)GameFlowState_ID.ShowScore);
        state_ShowScore.Initialize(this, null, () => { });

    }

    public int GetDifficulty()
    {
        return _selectDifficulty;
    }
    public void SetDifficulty(int difficulty)
    {
        _selectDifficulty = difficulty;
    }

    #region Def State
    //InitState
    private class InitState:FSM_State
    {
        public InitState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            base.Enter(fsm, owner);
            //changeState
            owner.ChangeState((int)GameFlowState_ID.MainMenu);
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            base.Leave(fsm, owner);
            //changeScene
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    //MainMenuState
    private class MainMenuState : FSM_State
    {
        public MainMenuState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            Debug.Log(((GameFlowState_ID)GetID()).ToString());
            UIManager.One.OpenUI(UIManager.UIModule.GameFlow_MainMenu);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            if (Input.anyKeyDown)
            {
                //changeState
                owner.ChangeState((int)GameFlowState_ID.SelectDiff);
            }
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            UIManager.One.CloseUI(UIManager.UIModule.GameFlow_MainMenu);
            //changeScene
            SceneManager.LoadScene("InGameScene");
        }
    }
    //SelectDifficultyState
    private class SelectDifficultyState : FSM_State
    {
        public SelectDifficultyState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            Debug.Log(((GameFlowState_ID)GetID()).ToString());
            FSM_GameFlow curFsm = fsm as FSM_GameFlow;
            curFsm.SetDifficulty(-1);            

            //Open UI SelectDiff
            UIManager.One.OpenUI(UIManager.UIModule.GameFlow_GamePlaying);
            UIManager.One.GetUIObject(UIManager.UIModule.GameFlow_GamePlaying).gameObject.SetActive(false);
            UIManager.One.OpenUI(UIManager.UIModule.GameFlow_SelectDiff);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            if (((FSM_GameFlow)fsm).GetDifficulty() != -1)
            {
                //changeState
                owner.ChangeState((int)GameFlowState_ID.GamePlaying);
            }
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            //Close UI SelectDiff
            UIManager.One.CloseUI(UIManager.UIModule.GameFlow_SelectDiff);
        }
    }
    //GamePlayingState
    private class GamePlayingState : FSM_State
    {
        public GamePlayingState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            Debug.Log(((GameFlowState_ID)GetID()).ToString());
            UIManager.One.GetUIObject(UIManager.UIModule.GameFlow_GamePlaying).gameObject.SetActive(true);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            if (((FSM_GameFlow)fsm).IsGameOver)
            {
                //changeState
                owner.ChangeState((int)GameFlowState_ID.GameOver);
            }
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            //Close UI SelectDiff
            UIManager.One.GetUIObject(UIManager.UIModule.GameFlow_GamePlaying).gameObject.SetActive(false);
        }
    }
    //GameOverState
    private class GameOverState : FSM_State
    {
        public GameOverState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            Debug.Log(((GameFlowState_ID)GetID()).ToString());
            //Open UI GameOver
            UIManager.One.OpenUI(UIManager.UIModule.GameFlow_GameOver);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            if (Input.GetMouseButtonDown(0))
            {
                owner.ChangeState((int)GameFlowState_ID.ShowScore);
            }
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            FSM_GameFlow curFsm = fsm as FSM_GameFlow;

            UIManager.One.CloseUI(UIManager.UIModule.GameFlow_GameOver);
            UIManager.One.CloseUI(UIManager.UIModule.GameFlow_GamePlaying);
            curFsm._isGameOver = false;
            //changeScene
            SceneManager.LoadScene("ScoreScene");
        }
    }
    //ShowScoreState
    private class ShowScoreState : FSM_State
    {
        public ShowScoreState(int id) : base(id) { }

        public override void Enter(FSM fsm, FSMActor owner)
        {
            Debug.Log(((GameFlowState_ID)GetID()).ToString());
            //Open UI GameOver
            UIManager.One.OpenUI(UIManager.UIModule.GameFlow_ShowScore);
        }
        public override void Update(FSM fsm, FSMActor owner)
        {
            FSM_GameFlow curFsm = fsm as FSM_GameFlow;
            if (curFsm.IsBackMenu)
            {
                owner.ChangeState((int)GameFlowState_ID.MainMenu);
            }
        }
        public override void Leave(FSM fsm, FSMActor owner)
        {
            FSM_GameFlow curFsm = fsm as FSM_GameFlow;

            UIManager.One.CloseUI(UIManager.UIModule.GameFlow_ShowScore);
            curFsm.IsBackMenu = false;
            //changeScene
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    #endregion
}
