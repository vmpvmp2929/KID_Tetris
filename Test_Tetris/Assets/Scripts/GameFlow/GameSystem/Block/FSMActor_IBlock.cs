using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMActor_IBlock : FSMActor_Block
{
    protected override void InitBlockDataList()
    {
        List<SquareData> sList1 = new List<SquareData>();
        sList1.Add(new SquareData(new Vector2(0, 1), true, true));
        sList1.Add(new SquareData(new Vector2(1, 1), true));
        sList1.Add(new SquareData(new Vector2(2, 1), true));
        sList1.Add(new SquareData(new Vector2(3, 1), true, false, true));
        BlockData B1 = new BlockData(sList1);
        _blockDataList.Add(B1);


        List<SquareData> sList2 = new List<SquareData>();
        sList2.Add(new SquareData(new Vector2(2, 0), true, true, true));
        sList2.Add(new SquareData(new Vector2(2, 1), false, true, true));
        sList2.Add(new SquareData(new Vector2(2, 2), false, true, true));
        sList2.Add(new SquareData(new Vector2(2, 3), false, true, true));
        BlockData B2 = new BlockData(sList2);
        _blockDataList.Add(B2);

        List<SquareData> sList3 = new List<SquareData>();
        sList3.Add(new SquareData(new Vector2(0, 2), true, true));
        sList3.Add(new SquareData(new Vector2(1, 2), true));
        sList3.Add(new SquareData(new Vector2(2, 2), true));
        sList3.Add(new SquareData(new Vector2(3, 2), true, false, true));
        BlockData B3 = new BlockData(sList3);
        _blockDataList.Add(B3);

        List<SquareData> sList4 = new List<SquareData>();
        sList4.Add(new SquareData(new Vector2(1, 0), true, true, true));
        sList4.Add(new SquareData(new Vector2(1, 1), false, true, true));
        sList4.Add(new SquareData(new Vector2(1, 2), false, true, true));
        sList4.Add(new SquareData(new Vector2(1, 3), false, true, true));
        BlockData B4 = new BlockData(sList4);
        _blockDataList.Add(B4);
    }

}
