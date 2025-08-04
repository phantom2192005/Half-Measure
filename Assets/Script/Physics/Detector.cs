using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundLayerMask;
    void Update()
    {
        //ground check
        HandleTrigger();
        HandleDrag();
    }

    private void HandleDrag()
    {
        if (playerController.onGround || playerController.OnSlope())
        {
            playerController.rb.drag = playerController.groundDrag;
        }
        else
        {
            playerController.rb.drag = 0;
        }
    }

    private void HandleTrigger()
    {
        if (playerController.onGround)
        {
            playerController.onGround = true;
            playerController.isGravityEffect = false;
            playerController.jumpCounter = 0;
            playerController.dashCounter = 0;
        }
        else if (!playerController.onGround)
        {
            playerController.isGravityEffect = true;
        }

        if (playerController.OnSlope() && !playerController.onGround)
        {
            playerController.isGravityEffect = false;
            playerController.jumpCounter = 0;
            playerController.dashCounter = 0;
        }
        else if (!playerController.OnSlope() && !playerController.onGround)
        {
            playerController.isGravityEffect = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        int objLayer = other.gameObject.layer;

        if (((1 << objLayer) & GroundLayerMask) != 0)
        {
            playerController.onGround = true;
            playerController.rb.velocity = new Vector3(0, 0, 0);
        }

        if (playerController.OnSlope())
        {
            playerController.rb.velocity = new Vector3(0, 0, 0);
            playerController.isGravityEffect = false;

        }

    }
    void OnTriggerStay(Collider other)
    {
        int objLayer = other.gameObject.layer;
        if (((1 << objLayer) & GroundLayerMask) != 0)
        {
            playerController.onGround = true;
        }
        if (other.tag == "Ceiling")
        {
            if (!playerController.overHeadChecker)
            {
                playerController.overHeadChecker = true;
            }
        }

    }
    void OnTriggerExit(Collider other)
    {
        int objLayer = other.gameObject.layer;

        if (((1 << objLayer) & GroundLayerMask) != 0)
        {
            playerController.onGround = false;
        }

        if (other.tag == "Ceiling")
        {
            playerController.overHeadChecker = false;
        }
    }
}
