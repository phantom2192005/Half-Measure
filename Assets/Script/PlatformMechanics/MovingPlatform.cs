using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : PlatformBase
{
    [Header("Waypoint Settings")]
    public GameObject path; // GameObject chứa các Transform con
    public float speed = 2f;

    private List<Transform> waypoints = new List<Transform>();
    private int currentIndex = 0;
    private Vector3 previousPosition;

    private List<Rigidbody> passengers = new List<Rigidbody>();

    void Start()
    {
        if (path != null)
        {
            // Lấy tất cả Transform con của path (trừ chính path)
            foreach (Transform child in path.transform)
            {
                waypoints.Add(child);
            }
        }

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned!");
            enabled = false;
        }

        previousPosition = transform.position;
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        AddForceOnPlayer();

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Count; // chia dư để đi 1 cách vòng lặp
        }
    }

    void AddForceOnPlayer()
    {
        Vector3 deltaMove = transform.position - previousPosition;

        foreach (var rb in passengers)
        {
            rb.MovePosition(rb.position + deltaMove);
        }

        previousPosition = transform.position;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = col.rigidbody;
            if (rb != null && !passengers.Contains(rb))
            {
                passengers.Add(rb);
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = col.rigidbody;
            if (rb != null && passengers.Contains(rb))
            {
                passengers.Remove(rb);
            }
        }
    }
}
