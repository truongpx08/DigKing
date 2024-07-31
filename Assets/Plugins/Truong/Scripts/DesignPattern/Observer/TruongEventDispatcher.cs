using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongEventDispatcher : MonoBehaviour
{
    /// Store all "listener"
    [ShowInInspector]
    private readonly Dictionary<string, Action<string, object, GameObject>>
        listeners = new(); // Dictionary<Key=EventType, Action<EventType, Attachment, Sender>>

    #region Singleton

    private static TruongEventDispatcher _instance;
    public static TruongEventDispatcher Instance
    {
        get
        {
            if (!Application.isPlaying) return null;
            // instance not exist, then create new one
            if (_instance == null)
            {
                CreateInstance();
            }

            return _instance;
        }
    }

    private static void CreateInstance()
    {
        GameObject singletonObject = new GameObject();
        _instance = singletonObject.AddComponent<TruongEventDispatcher>();
        DontDestroyOnLoad(_instance.gameObject);
        singletonObject.name = TruongConstant.TruongEventDispatcher;
    }

    protected virtual void Awake()
    {
        StartCoroutine(Check());

        IEnumerator Check()
        {
            yield return new WaitForSeconds(0.5f);
            if (gameObject.name.Contains(TruongConstant.TruongEventDispatcher))
            {
                yield break;
            }

            Destroy(gameObject);

            // check if there's another instance already exist in scene
            if (_instance != null && _instance.GetInstanceID() != this.GetInstanceID())
            {
                // Destroy this instances because already exist the singleton of EventsDispatcer
                //Common.Log("An instance of EventDispatcher already exist : <{1}>, So destroy this instance : <{2}>!!", s_instance.name, name);
                Destroy(gameObject);

                yield break;
            }

            // set instance
            _instance = this;
        }
    }


    protected virtual void OnDestroy()
    {
        // reset this static var to null if it's the singleton instance
        if (!Application.isPlaying) return;
        if (_instance != this) return;
        ClearAllListener();
        _instance = null;
    }

    #endregion

    #region Add Listeners, Post events, Remove listener

    /// <summary>
    /// Register to listen for eventID
    /// </summary>
    /// <param name="eventID">EventID that object want to listen</param>
    /// <param name="callback">Callback will be invoked when this eventID be raised</param>
    public void RegisterListener(string eventID, Action<string, object, GameObject> callback)
    {
        if (listeners.ContainsKey(eventID))
        {
            var callBacks = listeners[eventID];
            // add callback to our collection
            listeners[eventID] += callback;
            return;
        }

        // add new key-value pair
        listeners.Add(eventID, null);
        listeners[eventID] += callback;
    }

    /// <summary>
    /// Posts the event. This will notify all listener that register for this event
    /// </summary>
    /// <param username="eventID">EventID.</param>
    /// <param username="sender">Sender, in some case, the Listener will need to know who send this message.</param>
    /// <param username="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
    /// <param name="eventID"></param>
    /// <param name="param"></param>
    public virtual void PostEvent(string eventID, object param = null, GameObject sender = null)
    {
        Debug.Log($"Notify: {eventID}");
        if (!listeners.ContainsKey(eventID))
        {
            Debug.LogWarning($"No listeners for this event : {eventID}");
            return;
        }

        // posting event
        var callbacks = listeners[eventID];
        // if there's no listener remain, then do nothing
        if (callbacks == null)
        {
            Debug.Log($"PostEvent {eventID}, but no listener remain, Remove this key");
            listeners.Remove(eventID);
            return;
        }

        callbacks(eventID, param, sender);
    }

    /// <summary>
    /// Removes the listener. Use to Unregister listener
    /// </summary>
    /// <param name="eventID">EventID.</param>
    /// <param name="callback">Callback.</param>
    public void RemoveListener(string eventID, Action<string, object, GameObject> callback)
    {
        if (!listeners.ContainsKey(eventID)) return;
        listeners[eventID] -= callback;
    }

    /// <summary>
    /// Clears all the listener.
    /// </summary>
    protected virtual void ClearAllListener()
    {
        listeners.Clear();
    }

    #endregion
}