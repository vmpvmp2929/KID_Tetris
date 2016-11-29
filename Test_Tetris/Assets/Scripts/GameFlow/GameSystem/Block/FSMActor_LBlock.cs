using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMActor_LBlock : FSMActor_Block
{
    protected override void InitBlockDataList()
    {
        List<SquareData> sList = new List<SquareData>();
        sList.Add(new SquareData(new Vector2(0, 1), true, true, false));
        sList.Add(new SquareData(new Vector2(1, 1), true, false, false));
        sList.Add(new SquareData(new Vector2(2, 1), true, false, true));
        sList.Add(new SquareData(new Vector2(2, 2), false, true, true));
        BlockData B1 = new BlockData(sList);
        _blockDataList.Add(B1);

        sList.Clear();
        sList.Add(new SquareData(new Vector2(1, 3), false, true, true));
        sList.Add(new SquareData(new Vector2(1, 2), false, true, true));
        sList.Add(new SquareData(new Vector2(1, 1), true, true));
        sList.Add(new SquareData(new Vector2(2, 3), true, false, true));
        BlockData B2 = new BlockData(sList);
        _blockDataList.Add(B2);

        sList.Clear();
        sList.Add(new SquareData(new Vector2(3, 2), true, false, true));
        sList.Add(new SquareData(new Vector2(2, 2), true));
        sList.Add(new SquareData(new Vector2(1, 2), false, true));
        sList.Add(new SquareData(new Vector2(1, 1), true, true, true));
        BlockData B3 = new BlockData(sList);
        _blockDataList.Add(B3);

        sList.Clear();
        sList.Add(new SquareData(new Vector2(2, 0), true, true, true));
        sList.Add(new SquareData(new Vector2(2, 1), false, true, true));
        sList.Add(new SquareData(new Vector2(2, 2), false, false, true));
        sList.Add(new SquareData(new Vector2(1, 2), true, true));
        BlockData B4 = new BlockData(sList);
        _blockDataList.Add(B4);
    }
}
