using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : class, new()
{
    protected static WeakReference<T> _singleton;

    public static T Singleton //Alias
    {
        get { return _singleton.Target; }
        protected set { _singleton.Target = value; }
    }

    public static T One //Alias
    {
        get
        {
#if UNITY_EDITOR
            if ((_singleton == null || _singleton.Target == null) && !EditorApplication.isPlaying)
            {
                T target = GameObject.FindObjectOfType(typeof(T)) as T;
                if (target != null)
                    _singleton = new WeakReference<T>(target);
            }
#endif 
            return _singleton;
        }
        protected set { _singleton = value; }
    }

    public virtual void Awake()
    {
        if (_singleton == null || _singleton.Target == null)
            _singleton = new WeakReference<T>(this as T);
        else
        {
            T thisAsT = this as T;
            if (_singleton.Target != thisAsT)
                DestroyImmediate(this.gameObject);
        }
    }
}

public class UseSingleton<T> where T : class, new()
{
    private static WeakReference<T> _singleton = new WeakReference<T>(null);
    public static T Singleton
    {
        get { return _singleton.Target; }
    }
    public static T One //Alias
    {
        get { return _singleton.Target; }
    }

    public UseSingleton()
    {
        _singleton.Target = this as T;
    }
}