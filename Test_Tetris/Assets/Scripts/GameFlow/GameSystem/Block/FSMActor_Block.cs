using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HFSMSystem;
using SquareFSM;


public abstract class FSMActor_Block : FSMActor
{
    protected class SquareData
    {
        public SquareData(Vector2 defalutCoordinate,bool judgeDown=false,bool judgeLeft=false,bool judgeRight=false)
        {
            SquareDefalutCoordinate = defalutCoordinate;           
            SetNeedToJudgeDirection(judgeDown, judgeLeft, judgeRight);
        }
        public Vector2 SquareDefalutCoordinate;
        public MoveDirection SquareNeedToJudgeDirection;

        private void SetJudgeDirection(MoveDirection direct) { SquareNeedToJudgeDirection |= direct; }
        private void ClearJudgeDirection(MoveDirection direct) { SquareNeedToJudgeDirection &= ~direct; }
        public void ClearAllJudgeDirection()
        {
            ClearJudgeDirection(MoveDirection.Down);
            ClearJudgeDirection(MoveDirection.Left);
            ClearJudgeDirection(MoveDirection.Right);
        }
        public void SetNeedToJudgeDirection(bool judgeDown = false, bool judgeLeft = false, bool judgeRight = false)
        {
            ClearAllJudgeDirection();
            if (judgeDown) SetJudgeDirection(MoveDirection.Down);
            if (judgeLeft) SetJudgeDirection(MoveDirection.Left);
            if (judgeRight) SetJudgeDirection(MoveDirection.Right);
        }
    }
    protected class BlockData
    {
        public BlockData(List<SquareData> dataList)
        {
            SquareDataList = dataList;
        }
        public List<SquareData> SquareDataList=new List<SquareData>();
    }

    protected List<BlockData> _blockDataList = new List<BlockData>();
    public List<GameObject> SquareList;
    private List<FSMActor_Square> _squareList = new List<FSMActor_Square>();
    private Vector2 _coordinate=new Vector2(-1,-1);
    private int _currentRotationIndex = 0;

    private float _downTimer = 0.0f;

    void Awake()
    {
        //SetFSM
        SetFSM(FSMManager.One.GetFSM((int)FSM_ID.BlockState));
    }
    void Start () {
        InitBlockDataList();
        InitSquare();
    }

    #region Get And Set
    public Vector2 GetCoordinate() { return _coordinate; }

    #endregion

    #region Function
    protected abstract void InitBlockDataList(); 
    protected void InitSquare()
    {
        for (int i = 0; i < SquareList.Count; i++)
        {
            FSMActor_Square s = SquareList[i].GetComponent<FSMActor_Square>();
            s.SetBlock(this);
            s.UpdateCoordinateFunction(_blockDataList[_currentRotationIndex].SquareDataList[i].SquareDefalutCoordinate);
            s.SetNeedToJudgeDirection(_blockDataList[_currentRotationIndex].SquareDataList[i].SquareNeedToJudgeDirection);           
            _squareList.Add(s);
        }
    }
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
    private void UpdatePosition()
    {
        this.gameObject.transform.localPosition = BlockManager.One.GetWorldPositionUseCoordinate(_coordinate);
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
    public void SetCoordinate(Vector2 newCoordinate)
    {
        _coordinate = newCoordinate;
        UpdatePosition();
        SquareUpdateCoordinateFunction();

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
        UpdatePosition();
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
