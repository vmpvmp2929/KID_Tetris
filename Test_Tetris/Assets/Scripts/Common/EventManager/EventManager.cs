//C# Unity event manager that uses strings in a hashtable over delegates and events in order to
//allow use of events without knowing where and when they're declared/defined.
//by Billy Fletcher of Rubix Studios
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IEventListener
{
    bool HandleEvent(IEvent evt);
}

public abstract class IEvent
{
    protected string _name;
    protected object[] _data;
    protected IEventListener[] _targetlistener;

    public string GetName()
    {
        return _name;
    }
    public int GetDataLength()
    {
        return _data.Length;
    }
    public object[] GetData()
    {
        return _data;
    }
    public T GetData<T>(int index)
    {
        if (index >= _data.Length) return default(T);
        else return (T)_data[index];
    }
    public IEventListener[] GetTargetListener()
    {
        return _targetlistener;
    }
}

public class EventManager :MonoSingleton<EventManager>
{
    public enum EventType
    {
        Broadcast,
        Send,
        Trigger,
    }
    struct ListenerData
    {
        public IEventListener Listener;
        public EventHandler Handler;
        public string EventName;
    }
    public delegate bool EventHandler(IEvent evt);
    public bool LimitQueueProcesing = false;
    public float QueueProcessTime = 0.0f;
    //protected static WeakReference<EventManager> _singleton;
    //public static EventManager One { get { return _singleton; } protected set { _singleton = value; } }

    /*public void Awake()
    {
        if (One == null)
        {
            One = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }*/

    public Dictionary<string, Dictionary<IEventListener, EventHandler>> m_listenerTable = new Dictionary<string, Dictionary<IEventListener, EventHandler>>();
    private Queue m_eventQueue = new Queue();
    private Stack<string> m_eventTriggeredStack = new Stack<string>();
    private List<ListenerData> m_listenerToBeAttached = new List<ListenerData>(256);
    private List<ListenerData> m_listenerToBeDetached = new List<ListenerData>(256);
    private uint m_eventTriggeringDepth = 0;

    private bool IsTriggeringEvent(string eventName)
    {
        return m_eventTriggeredStack.Count > 0 && m_eventTriggeredStack.Contains(eventName);
    }
    //Add a listener to the event manager that will receive any events of the supplied event name.
    public bool AddListener(IEventListener iEventListener, string eventName)
    {
        if (iEventListener == null || eventName == null)
        {
            Debug.Log("Event Manager: AddListener failed due to no listener or event name specified.");
            return false;
        }
        Dictionary<IEventListener, EventHandler> listeners = null;
        if (!m_listenerTable.TryGetValue(eventName, out listeners))
        {
            listeners = new Dictionary<IEventListener, EventHandler>();
            m_listenerTable.Add(eventName, listeners);
        }

        if (IsTriggeringEvent(eventName))
        {
            m_listenerToBeAttached.Add(new ListenerData() { Listener = iEventListener, Handler = null, EventName = eventName });
        }
        else
        {
            EventHandler handler = null;
            if (listeners.TryGetValue(iEventListener, out handler))
            {
                handler -= iEventListener.HandleEvent;
                handler += iEventListener.HandleEvent;
            }
            else
            {
                handler += iEventListener.HandleEvent;
                listeners.Add(iEventListener, handler);
            }
        }
        return true;
    }

    //Add a listener to the event manager that will receive any events of the supplied event name and use handler to deal with.
    public bool AddListener(IEventListener iEventListener, EventHandler eventHandleFunction, string eventName)
    {
        if (iEventListener == null || eventName == null)
        {
            Debug.Log("Event Manager: AddListener failed due to no listener or event name specified.");
            return false;
        }

        if (IsTriggeringEvent(eventName))
        {
            m_listenerToBeAttached.Add(new ListenerData() { Listener = iEventListener, Handler = eventHandleFunction, EventName = eventName });
        }
        else
        {
            Dictionary<IEventListener, EventHandler> listeners = null;
            if (!m_listenerTable.TryGetValue(eventName, out listeners))
            {
                listeners = new Dictionary<IEventListener, EventHandler>();
                m_listenerTable.Add(eventName, listeners);
            }

            EventHandler handler = null;
            if (listeners.TryGetValue(iEventListener, out handler))
            {
                handler += eventHandleFunction;
            }
            else
            {
                handler += eventHandleFunction;
                listeners.Add(iEventListener, handler);
            }
            return true;
        }
        return false;
    }

    //Remove a listener from the subscribed to event.
    public bool DetachListener(IEventListener listener, string eventName)
    {
        if (IsTriggeringEvent(eventName))
        {
            m_listenerToBeDetached.Add(new ListenerData() { Listener = listener, Handler = null, EventName = eventName });
        }
        else
        {
            if (eventName == null)
                return false;
            else
            {
                Dictionary<IEventListener, EventHandler> listeners;
                if (m_listenerTable.TryGetValue(eventName, out listeners))
                {
                    listeners.Remove(listener);
                    if (listeners.Count == 0)
                    {
                        m_listenerTable.Remove(eventName);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public bool DetachListener(IEventListener listener, EventHandler handleFunction, string eventName)
    {
        if (!m_listenerTable.ContainsKey(eventName))
            return false;

        if (IsTriggeringEvent(eventName))
        {
            m_listenerToBeDetached.Add(new ListenerData() { Listener = listener, Handler = handleFunction, EventName = eventName });
        }
        else
        {
            if (eventName == null)
                return false;
            else
            {
                Dictionary<IEventListener, EventHandler> listeners;
                if (m_listenerTable.TryGetValue(eventName, out listeners))
                {
                    EventHandler handler;
                    if (listeners.TryGetValue(listener, out handler))
                    {
                        handler -= handleFunction;

                        if (handler == null)
                        {
                            listeners.Remove(listener);
                        }
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //public bool DetachListener(IEventListener listener)
    //{
    //    if (IsTriggeringEvent())
    //    {
    //        m_listenerToBeDetached.Add(new ListenerData() { Listener = listener, Handler = null, EventName = null });
    //    }
    //    else
    //    {
    //        var item = m_listenerTable.GetEnumerator();
    //        while (item.MoveNext())
    //        {
    //            item.Current.Value.Remove(listener);
    //        }
    //        return true;
    //    }
    //    return false;
    //}

    //Trigger the event instantly, this should only be used in specific circumstances,
    //the QueueEvent function is usually fast enough for the vast majority of uses.
    public bool TriggerEvent(IEvent evt)
    {
        string eventName = evt.GetName();
        Dictionary<IEventListener, EventHandler> listenerList;
        if (m_listenerTable.TryGetValue(eventName, out listenerList))
        {
            m_eventTriggeredStack.Push(eventName);
            IEventListener[] targetlistener = evt.GetTargetListener();
            if (targetlistener == null)
            {
#if DEV_BUILD
                try
                {
#endif
                foreach (var listenerItem in listenerList)
                {
                    if (listenerItem.Value != null)
                    {
                        listenerItem.Value(evt);
                    }
                }
#if DEV_BUILD
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Exception thrown when event: " + eventName + " is triggered.\n" + e.Message + e.StackTrace.ToString());
                }
#endif
            }
            else
            {
#if DEV_BUILD
                try
                {
#endif
                foreach (IEventListener listenertarget in targetlistener)
                {
                    EventHandler handler;
                    if (listenerList.TryGetValue(listenertarget, out handler))
                    {
                        if (handler != null)
                        {
                            handler(evt);
                        }
                    }
                }
#if DEV_BUILD
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Exception thrown when event: " + eventName + " is triggered.\n" + e.Message + e.StackTrace.ToString());
                }
#endif
            }
            m_eventTriggeredStack.Pop();
            return true;
        }
        else
        {
            Debug.LogWarning("Event Manager: Event \"" + eventName + "\" triggered has no listeners!");
            return false; //No listeners for event so ignore it
        }
    }

    //Inserts the event into the current queue.
    public bool QueueEvent(IEvent evt)
    {
        if (!m_listenerTable.ContainsKey(evt.GetName()))
        {
            //Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetName());
            return false;
        }

        m_eventQueue.Enqueue(evt);
        return true;
    }

    //Every update cycle the queue is processed, if the queue processing is limited,
    //a maximum processing time per update can be set after which the events will have
    //to be processed next update loop.
#if USING_FIXED_UPDATE
    public void FixedUpdate()
#else
    public void Update()
#endif
    {
        float timer = 0.0f;
        while (m_eventQueue.Count > 0)
        {
            //Debug.Log(LimitQueueProcesing + ", " + QueueProcessTime);
            if (LimitQueueProcesing)
            {
                if (timer > QueueProcessTime)
                    return;
            }

            IEvent evt = m_eventQueue.Dequeue() as IEvent;
            if (!TriggerEvent(evt))
                Debug.Log("Error when processing event: " + evt.GetName());

            if (LimitQueueProcesing)
#if USING_FIXED_UPDATE
                timer += Time.fixedDeltaTime;
#else
                timer += Time.deltaTime;
#endif
        }

        ProcessListenersToBeAttached();
        ProcessListenersToBeDetached();
    }

    private void ProcessListenersToBeDetached()
    {
        //Process listeners to be detached
        if (m_listenerToBeDetached.Count > 0)
        {
            for (int i = 0; i < m_listenerToBeDetached.Count; ++i)
            {
                ListenerData data = m_listenerToBeDetached[i];
                if (data.Listener != null)
                {
                    if (data.Handler == null)
                        DetachListener(data.Listener, data.EventName);
                    else
                        DetachListener(data.Listener, data.Handler, data.EventName);
                }
            }
            m_listenerToBeDetached.Clear();
        }
    }
    private void ProcessListenersToBeAttached()
    {
        //Process listeners to be attached
        if (m_listenerToBeAttached.Count > 0)
        {
            for (int i = 0; i < m_listenerToBeAttached.Count; ++i)
            {
                ListenerData data = m_listenerToBeAttached[i];
                if (data.Listener != null)
                {
                    if (data.Handler == null)
                        AddListener(data.Listener, data.EventName);
                    else
                        AddListener(data.Listener, data.Handler, data.EventName);
                }
            }
            m_listenerToBeAttached.Clear();
        }
    }
    public void OnApplicationQuit()
    {
        m_listenerTable.Clear();
        m_eventQueue.Clear();
    }

    public void OnDestroy()
    {
#if UNITY_EDITOR
        Debug.LogWarning("EventManager OnDestroy.");
        foreach (var iter in m_listenerTable)
        {
            string eventName = iter.Key;
            foreach (var iter2 in iter.Value)
            {
                MonoBehaviour listener = iter2.Key as MonoBehaviour;
                Debug.LogWarning(listener.GetType() + "( " + listener.name + " ) is still listening event: " + eventName);
            }
        }
#endif
    }

    // Send event globally.
    static public void Broadcast(string name, params object[] ob)
    {
        One.QueueEvent(new UserEvent(name, ob));
    }

    // Send event to assigned IEventListener(s)
    static public void Send(string name, IEventListener targetlistener)
    {
        One.QueueEvent(new UserEvent(name, targetlistener, null));
    }
    static public void Send(string name, IEventListener[] targetlistener)
    {
        One.QueueEvent(new UserEvent(name, targetlistener, null));
    }
    static public void Send(string name, IEventListener targetlistener, params object[] ob)
    {
        One.QueueEvent(new UserEvent(name, targetlistener, ob));
    }
    static public void Send(string name, IEventListener[] targetlistener, params object[] ob)
    {
        One.QueueEvent(new UserEvent(name, targetlistener, ob));
    }

    /*
    // Send event right away instead of queuing by event system.
    static public void Trigger(string name, GameObject targetGameObject, params object[] ob)
    {
        Component[] listenedComps = targetGameObject.GetComponents(typeof(IEventListener));
        Debug.Log(listenedComps.Length);
        //IEvent evt = new UserEvent(name, listenedComps, ob);
        //if (!One.TriggerEvent(evt))
        //    Debug.Log("Error when processing event: " + evt.GetName());
    }*/

    // Send event right away instead of queuing by event system.
    static public void Trigger(string name, IEventListener targetlistener, params object[] ob)
    {
        IEvent evt = new UserEvent(name, targetlistener, ob);
        if (!One.TriggerEvent(evt))
            Debug.Log("Error when processing event: " + evt.GetName());
    }

    static public void Trigger(string name, IEventListener[] targetlistener, params object[] ob)
    {
        IEvent evt = new UserEvent(name, targetlistener, ob);
        if (!One.TriggerEvent(evt))
            Debug.Log("Error when processing event: " + evt.GetName());
    }

    static public void Get(IEventListener listener, string name)
    {
        One.AddListener(listener, name);
    }
    static public void Get(IEventListener listener, EventHandler handleFunction, string name)
    {
        One.AddListener(listener, handleFunction, name);
    }
    static public void DontGet(IEventListener listener, string name)
    {
        One.DetachListener(listener, name);
    }
    static public void DontGet(IEventListener listener, EventHandler handleFunction, string name)
    {
        One.DetachListener(listener, handleFunction, name);
    }

    /*static public void DontGet(IEventListener listener)
    {
        One.DetachListener(listener);
    }*/
}

public class UserEvent : IEvent
{
    public UserEvent(string name, params object[] ob)
    {
        _name = name;
        _data = ob;
    }

    public UserEvent(string name, IEventListener targetlistener, params object[] ob)
    {
        _name = name;
        this._targetlistener = new IEventListener[1] { targetlistener };
        _data = ob;
    }

    public UserEvent(string name, IEventListener[] targetlistener, params object[] ob)
    {
        _name = name;
        this._targetlistener = targetlistener;
        _data = ob;
    }
}