using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
       
    public enum UIModule
    {
        GameFlow_MainMenu,
        GameFlow_SelectDiff,
        GameFlow_GamePlaying,
        GameFlow_GameOver,
        GameFlow_ShowScore,

        GamePlay_Ready,
        
    }
    protected static UIManager _singleton;
    public static UIManager One { get { return _singleton; } set { _singleton = value; } }
    public List<GameObject> UIPrefabList;
    private Dictionary<int, GameObject> _uiOpenMap = new Dictionary<int, GameObject>();  

    void Start () {
        One = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void OpenUI(UIModule Mode)
    {
        if(_uiOpenMap.ContainsKey((int)Mode))
        {
            if (!_uiOpenMap[(int)Mode])
            {
                GameObject UIObj = GameObject.Instantiate(UIPrefabList[(int)Mode], Vector3.zero, Quaternion.identity) as GameObject;
                UIObj.transform.parent = this.transform;
                RectTransform Rect = UIObj.GetComponent<RectTransform>();
                Rect.localPosition = Vector3.zero;
                _uiOpenMap[(int)Mode] = UIObj;
            }
        }
        else
        {            
            GameObject UIObj = GameObject.Instantiate(UIPrefabList[(int)Mode], Vector3.zero, Quaternion.identity) as GameObject;
            UIObj.transform.parent = this.transform;
            RectTransform Rect = UIObj.GetComponent<RectTransform>();
            Rect.localPosition = Vector3.zero;
            _uiOpenMap.Add((int)Mode, UIObj);
        }
    }
    public void CloseUI(UIModule Mode)
    {
        if (_uiOpenMap.ContainsKey((int)Mode))
        {
            if (_uiOpenMap[(int)Mode])
            {
                Destroy(_uiOpenMap[(int)Mode]);
                _uiOpenMap[(int)Mode] = null;
            }
        }
    }
    public GameObject GetUIObject(UIModule Mode)
    {
        if (_uiOpenMap.ContainsKey((int)Mode))
        {
            return _uiOpenMap[(int)Mode];
        }
        return null;
    }
}
