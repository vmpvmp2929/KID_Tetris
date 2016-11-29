using UnityEngine;
using System.Collections;

public class GlobalObjectController : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(this.transform);
    }
}
