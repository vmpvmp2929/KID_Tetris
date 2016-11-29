using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HFSMSystem;

public class UI_GamePlay_Ready : MonoBehaviour {

    public Text ReadyTimeText;
    private FSMActor_GameSystemController _thisGameSystem;

    void Start () {
        _thisGameSystem=FSMManager.One.GetFSM((int)FSM_ID.GameSystemFSMState).GetActorFirst() as FSMActor_GameSystemController;
        UpdateText();
    }
	
	// Update is called once per frame
	void Update () {

        UpdateText();
    }
    void UpdateText()
    {
        ReadyTimeText.text = (int)(_thisGameSystem.GameTimer+0.5f)>0? ((int)(_thisGameSystem.GameTimer + 0.5f)).ToString() : "GO!";
    }

}
