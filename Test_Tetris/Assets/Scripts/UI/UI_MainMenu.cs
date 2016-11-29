using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour {

    public Text FadeText;

    private float FadeSpeed = 1.0f;
	void Start () {
     

    }
	
	void Update () {
        if (FadeText.color.a > 0.1f)
        {
            FadeText.color= new Color(1.0f, 1.0f, 1.0f, FadeText.color.a- FadeSpeed*Time.deltaTime);
        }
        else
        {
            FadeText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
           
	}
}
