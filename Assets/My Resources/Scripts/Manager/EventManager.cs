using System;
using System.Collections.Generic;

public class EventManager
{
    private static Dictionary<string, Delegate> eventDictionary;

    public static void Subscribe<T>(Action<T> listener) where T : IEvent
    {
        string eventName = typeof(T).Name;
        if (eventDictionary.TryGetValue(eventName, out Delegate existingDelegate))
        {
            eventDictionary[eventName] = Delegate.Combine(existingDelegate, listener);
        }
        else eventDictionary.Add(eventName, listener);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : IEvent
    {
        string eventName = typeof(T).Name;
        if (eventDictionary.TryGetValue(eventName, out Delegate existingDelegate))
        {
            Delegate currentDel = Delegate.Remove(existingDelegate, listener);
            if (currentDel == null) eventDictionary.Remove(eventName);
            else eventDictionary[eventName] = currentDel;
        }
    }

    public static void Notify<T>(T evt) where T : IEvent
    {
        string eventName = typeof(T).Name;
        if (eventDictionary.TryGetValue(eventName, out Delegate existingDelegate))
        {
            Action<T> listener = existingDelegate as Action<T>;
            listener?.Invoke(evt);
        }
    }
}

public interface IEvent { };

public readonly struct LeftRoomEvent : IEvent
{
    public bool IsHost { get; }
    public int PlayerCount { get; }

    public LeftRoomEvent(bool isHost, int playerCount)
    {
        IsHost = isHost;
        PlayerCount = playerCount;
    }
}


public readonly struct JoinRoomEvent : IEvent
{
    public string RoomId { get; }
    public bool IsHost { get; }
    public int PlayerCount { get; }

    public JoinRoomEvent(string roomId, bool isHost, int playerCount)
    {
        RoomId = roomId;
        IsHost = isHost;
        PlayerCount = playerCount;
    }
}




