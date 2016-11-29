using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HFSMSystem;

public class FSMActor_GameSystemController : FSMActor
{
    private static FSMActor_GameSystemController _singleton;
    public static FSMActor_GameSystemController One { get { return _singleton; } }
    
    //Timer
    public const float ReadyTime = 3.5f;
    public const float LevelUpTime = 10.0f;
    private float _gameTimer = 0.0f;
    public float GameTimer { get { return _gameTimer; } set { _gameTimer = value; } }
    //Grid
    public const int GridWidth = 10;
    public const int GridHight = 24;
    private bool[][] _gridList = new bool[GridWidth][];
    //Down
    public const float DefalutMaxDownSpeed = 1.0f;
    public const float DefalutMinDownSpeed = 0.05f;
    public const int DefalutMaxLevel = 10;

    //Eliminate
    private List<int> _eliminateRowList = new List<int>();

    public void Awake()
    {
        _singleton = this;
        //SetFSM
        SetFSM(FSMManager.One.GetFSM((int)FSM_ID.GameSystemFSMState));
        InitGridList();
    }

    #region Get And Set
    public List<int> GetEliminateRowList  { get {return _eliminateRowList; }   }
    public bool[][] GetGridList() { return _gridList; }
    #endregion

    #region Function
    void InitGridList()
    {
        for (int i = 0; i < GridWidth; i++)
        {
            _gridList[i] = new bool[GridHight];
            for (int j = 0; j < GridHight; j++)
            {
                _gridList[i][j] = false;
            }
        }
    }
    public bool JudgeGameOverFunction()
    {
        return false;
    }
    public void JudgeEliminateFunction()
    {

    }

    public void DoEliminate()
    {

    }
    #endregion
}
