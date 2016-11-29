using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HFSMSystem;
using SquareFSM;


public class FSMActor_Block : FSMActor
{
    private float _downTimer = 0.0f;
    private List<FSMActor_Square> _squareList;
    private Vector2 _coordinate=new Vector2(-1,-1);
    

    void Start () {
        //SetFSM
        SetFSM(FSMManager.One.GetFSM((int)FSM_ID.SquareState));
    }

    #region Get And Set
    public Vector2 GetCoordinate() { return _coordinate; }
    #endregion

    #region Function
    public void InitDownTimer(float downspeed)
    {
        _downTimer = downspeed;
    }   
    public bool UpdateDownTimer()
    {
        if (_downTimer > 0)
            _downTimer -= Time.deltaTime;
        if (_downTimer < 0)
        {
            _downTimer = 0;
            return true;
        }
        return false;
    }
    public bool JudgeDirectFunction(MoveDirection direct)
    {
        for(int i=0;i<_squareList.Count;i++)
        {
            if(!_squareList[i].DoJudgeDirect(direct))
                return false;
        }
        return true;
    }
    public void DoMoveFunction(MoveDirection direct)
    {
        switch(direct)
        {
            case MoveDirection.Left:
                _coordinate.x -= 1;
                break;
            case MoveDirection.Right:
                _coordinate.x += 1;
                break;
            case MoveDirection.Down:
                _coordinate.y -= 1;
                break;
        }
        SquareUpdateCoordinateFunction();
    }
    public void SquareUpdateCoordinateFunction()
    {
        for(int i=0;i<_squareList.Count;i++)
        {
            _squareList[i].UpdateCoordinateFunction();
        }
    }
    #endregion
}
