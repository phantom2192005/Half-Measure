using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    protected bool isPlayerOnPlatform = false;

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            OnPlayerEnter();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            OnPlayerExit();
        }
    }
}

