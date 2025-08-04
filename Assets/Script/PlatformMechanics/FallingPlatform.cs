using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : PlatformBase
{
    public float delayBeforeFall = 1f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    protected override void OnPlayerEnter()
    {
        Invoke(nameof(Fall), delayBeforeFall);
    }

    void Fall()
    {
        rb.isKinematic = false;
    }
}

