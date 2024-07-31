using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Always try to send message after completing every action in the Monobehaviour first.
/// By my coach: Tee <3
/// </summary>
public class TruongObserver : TruongSingleton<TruongObserver>, ISerializationCallbackReceiver
{
    [SerializeField] private bool isLogNotify;
    [SerializeField] private List<string> keys = new List<string>();
    [ShowInInspector] private readonly Dictionary<string, List<IMessageHandler>> subscribers =
        new Dictionary<string, List<IMessageHandler>>();

    public void AddSubscriber(string type, IMessageHandler handler)
    {
        if (!subscribers.ContainsKey(type))
            subscribers[type] = new List<IMessageHandler>();
        if (!subscribers[type].Contains(handler))
            subscribers[type].Add(handler);
    }

    public void RemoveSubscriber(string type, IMessageHandler handler)
    {
        if (!subscribers.ContainsKey(type)) return;
        if (subscribers[type].Contains(handler))
            subscribers[type].Remove(handler);
    }

    public void Notify(Message message)
    {
        LogNotify(message.type);
        if (!subscribers.ContainsKey(message.type)) return;
        for (var i = subscribers[message.type].Count - 1; i > -1; i--)
            subscribers[message.type][i].HandleMessage(message);
    }

    public void Notify(string messageType, object[] data = null)
    {
        var message = new Message(messageType, data);
        Notify(message);
    }

    private void LogNotify(string messageType)
    {
        if (!this.isLogNotify) return;
        Debug.Log($"Observer notify: {messageType}");
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        foreach (var element in subscribers)
        {
            keys.Add(element.Key);
        }
    }

    public void OnAfterDeserialize()
    {
    }
}


public class Message
{
    public readonly object[] data;
    public readonly string type;

    public Message(string type)
    {
        this.type = type;
    }

    public Message(string type, object[] data)
    {
        this.type = type;
        this.data = data;
    }
}

public interface IMessageHandler
{
    void HandleMessage(Message message);
}