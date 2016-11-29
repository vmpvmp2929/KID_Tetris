using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour {

    private static BlockManager _singleton;
    public static BlockManager One { get { return _singleton; } }

    public List<GameObject> BlockPrefabList;
    public RectTransform StartPoint;
    public RectTransform EndPoint;
    private Transform SpawnListObjectTransform;

    public float GridWidth;
    public float GridHight;

    private List<int> _readySpawnTypeList=new List<int>();
    private List<FSMActor_Block> _blockList=new List<FSMActor_Block>();

	void Awake () {
        _singleton = this;
    }
    void Start()
    {
        //Calculate Width And Hight
        GridWidth = (EndPoint.transform.localPosition.x - StartPoint.transform.localPosition.x) / (FSMActor_GameSystemController.SceneWidthGridNumber-1);
        GridHight = (EndPoint.transform.localPosition.y - StartPoint.transform.localPosition.y) / (FSMActor_GameSystemController.SceneHightGridNumber-1);
        SpawnListObjectTransform=this.transform.FindChild("SquareList");
    }
	
	void Update () {
	
	}

    #region Function
    public void SpawnNewBlock(int blockType = -1)
    {
        int newBlockIndex= (blockType==-1)? Random.Range(0,BlockPrefabList.Count) : blockType;
        GameObject NewBlock = Instantiate(BlockPrefabList[newBlockIndex], Vector3.zero, Quaternion.identity) as GameObject;
        NewBlock.transform.parent = SpawnListObjectTransform;
        FSMActor_Block B = NewBlock.GetComponent<FSMActor_Block>();
        B.SetCoordinate(new Vector2(3, 20));
        NewBlock.transform.SetSiblingIndex(-1);
        _blockList.Add(B);

    }

    public Vector3 GetWorldPositionUseCoordinate(Vector2 coordinate)
    {
        return (new Vector3((coordinate.x * GridWidth), (coordinate.y * GridHight)));
    }
    #endregion

}
