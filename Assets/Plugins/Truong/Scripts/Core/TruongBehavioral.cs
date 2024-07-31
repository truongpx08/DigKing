using System;
using UnityEngine;

public class TruongBehavioral : TruongStructural
{
    #region Observer

    protected void Subscribe(string eventID)
    {
        TruongEventDispatcher.Instance.RegisterListener(eventID, OnReceivingNotification);
    }

    protected void Unsubscribe(string eventID)
    {
        TruongEventDispatcher.Instance.RemoveListener(eventID, OnReceivingNotification);
    }

    protected void Notify(string eventID, object attachment = null)
    {
        TruongEventDispatcher.Instance.PostEvent(eventID, attachment, this.gameObject);
    }

    protected virtual void OnReceivingNotification(string eventID, object attachment, GameObject sender)
    {
        //For override
    }

    #endregion
}