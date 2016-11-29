using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HFSMSystem;
using SquareFSM;


public class FSMActor_Square : FSMActor
{   
    private FSMActor_Block _thisBlock;
    private Vector2 _localCoordinate;
    private Vector2 _worldCoordinate = new Vector2(-1, -1);
    public bool IsAlife = true;
    
    private MoveDirection _needToJudgeDirect;
    private float _disappearTimer = 0.0f;
    private Vector3 _disappearScaleSpeed;

    public void Awake()
    {
        //SetFSM
        SetFSM(FSMManager.One.GetFSM((int)FSM_ID.SquareState));
    }
    public void Start()
    {
        
    }
    #region Get And Set
    public Vector2 GetLocalCoordinate() { return _localCoordinate; }
    public void SetNeedToJudgeDirection(MoveDirection direct) { _needToJudgeDirect = direct; }
    public void SetJudgeDirection(MoveDirection direct) { _needToJudgeDirect |= direct; }
    public void ClearJudgeDirection(MoveDirection direct) { _needToJudgeDirect &= ~direct; }
    public void SetBlock(FSMActor_Block block) { _thisBlock = block; }
    private Vector2 GetWorldCoordinate() { return (_thisBlock.GetCoordinate() + _localCoordinate); }
    public void SetLocalPosition(Vector3 Position)
    {
        this.gameObject.transform.localPosition = Position;
    }
    #endregion

    #region Function
    public bool DoJudgeDirect(MoveDirection judgeDirect)
    {
        if(_stateID_Curr  != (int)SquareState_ID.Move)
        {
            Debug.Log("current isn't Move");
            return false;
        }
        if (!((_needToJudgeDirect & judgeDirect) == judgeDirect))
        {
            Debug.Log("ThisSquare" + _worldCoordinate + ",Don't need to judge " + judgeDirect.ToString());
            return true;
        }
        switch (judgeDirect)
        {
            case MoveDirection.Left:
                if (_worldCoordinate.x == 0)
                    return false;
                return !(FSMActor_GameSystemController.One.GetGridList()[(int)_worldCoordinate.x - 1][(int)_worldCoordinate.y]);
            case MoveDirection.Right:
                if (_worldCoordinate.x == FSMActor_GameSystemController.SceneWidthGridNumber - 1)
                    return false;
                return !(FSMActor_GameSystemController.One.GetGridList()[(int)_worldCoordinate.x + 1][(int)_worldCoordinate.y]);
            default:
                if (_worldCoordinate.y == 0)
                    return false;
                return !(FSMActor_GameSystemController.One.GetGridList()[(int)_worldCoordinate.x][(int)_worldCoordinate.y - 1]);
        }
    }
    public void UpdateCoordinateFunction()
    {
        _worldCoordinate = GetWorldCoordinate();
    }
    public void UpdateCoordinateFunction(Vector2 Newcoordinate)
    {
        _localCoordinate = Newcoordinate;
        _worldCoordinate = GetWorldCoordinate();
    }

    public void InitDisappearFunction(float disappearTime)
    {
        _disappearTimer = disappearTime;
        _disappearScaleSpeed = (-Vector3.one) / disappearTime;
    }
    public void DisappearFunction()
    {
        Vector3 Speed = _disappearScaleSpeed * Time.deltaTime;
        if (Vector3.Distance(Vector3.zero,this.transform.localScale) > Vector3.Distance(Vector3.zero, Speed))
        {
            this.transform.localScale += Speed;
        }
        else
        {
            this.transform.localScale = Vector3.zero;
            IsAlife = false;
        }
    }
    #endregion
}
