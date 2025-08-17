using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Pool Manager:")]
    [SerializeField] protected List<Transform> prefabList = new();
    [SerializeField] protected Dictionary<string, Queue<Transform>> objectPool = new();
    [SerializeField] protected List<Transform> objectActive = new();
    [SerializeField] private Transform prefabs;
    [SerializeField] private Transform holder;

    protected override void LoadComponents()
    {
        LoadPrefabs();
        LoadPrefabList();
        LoadHolder();
    }

    private void LoadPrefabs()
    {
        if (prefabs != null) return;
        prefabs = transform.Find("Prefabs");
        if (prefabs != null) return;
        prefabs = new GameObject("Prefabs").transform;
        prefabs.SetParent(transform);
    }

    private void LoadPrefabList()
    {
        if (prefabList.Count == prefabs.childCount) return;
        prefabList.Clear();
        foreach (Transform prefab in prefabs)
        {
            prefabList.Add(prefab);
            prefab.gameObject.SetActive(false);
        }
    }

    private void LoadHolder()
    {
        if (holder != null) return;
        holder = transform.Find("Holder");
        if (holder != null) return;
        holder = new GameObject("Holder").transform;
        holder.SetParent(transform);
    }

    private Transform GetPrefabByName(string prefabName)
    {
        foreach (var prefab in prefabList)
        {
            if (prefab.name == prefabName) return prefab;
        }
        return null;
    }

    private Transform GetObjectFromPool(Transform prefab)
    {
        string prefabName = prefab.name;
        if (objectPool.TryGetValue(prefabName, out var queue) && queue.Count > 0)
        {
            return queue.Dequeue();
        }
        Transform newObject = Instantiate(prefab);
        newObject.name = prefabName;
        return newObject;
    }

    public Transform Spawn(Enum spawmType, Vector2 spawnPosition, Quaternion spawnRotation)
    {
        string prefabName = Utilities.EnumToString(spawmType);
        Transform prefab = GetPrefabByName(prefabName);
        Transform obj = GetObjectFromPool(prefab);
        obj.SetPositionAndRotation(spawnPosition, spawnRotation);
        obj.SetParent(holder);
        obj.gameObject.SetActive(true);
        objectActive.Add(obj);
        return obj;
    }

    public void Despawn(Transform objectDeSpawn)
    {
        string prefabName = objectDeSpawn.name;
        if (!objectPool.ContainsKey(prefabName))
        {
            objectPool[prefabName] = new Queue<Transform>();
        }
        objectPool[prefabName].Enqueue(objectDeSpawn);
        objectDeSpawn.gameObject.SetActive(false);
        objectActive.Remove(objectDeSpawn);
    }
}
