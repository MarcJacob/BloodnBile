﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityControl : MonoBehaviour
{

    NetworkSocketInfo NetworkInfo;
    LinkTo EntityLink;
    Rigidbody ControlledEntityRigidbody;

    public void Initialize(NetworkSocketInfo netInfo)
    {
        NetworkInfo = netInfo;
        EntityLink = GetComponent<LinkTo>();
        EntityLink.TrackLocation = false;
        EntityLink.TrackRotation = false;
    }

    private void Start()
    {
        Camera.main.transform.position = transform.position + new Vector3(0f, 2.5f, -4f);
        GameObject pivot = new GameObject("CameraPivot");
        pivot.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        pivot.transform.rotation = transform.rotation;
        pivot.transform.parent = transform;
        Camera.main.transform.parent = pivot.transform;
        ControlledEntityRigidbody = gameObject.AddComponent<Rigidbody>();
        ControlledEntityRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        gameObject.AddComponent<CapsuleCollider>();
    }

    Vector3 DirectionVector;
    bool Changed = false;

    float ClientEntityUpdatesToServerPerSecond = 20f;
    float cd_ClientEntityUpdateToServer = 0f;
    private void Update()
    {
        if (NetworkInfo != null)
        {
            Mouselook();
            HandleInput();
            if (1 / ClientEntityUpdatesToServerPerSecond < cd_ClientEntityUpdateToServer)
            {
                new NetworkMessage(12, new EntityPositionRotationUpdate(EntityLink.LinkedEntity.ID, transform.position, transform.rotation)).Send(NetworkInfo, NetworkInfo.ConnectionIDs[0], NetworkInfo.UnreliableChannelID);
                cd_ClientEntityUpdateToServer = 0f;
                Debugger.LogMessage("Envoie de la nouvelle position et nouvelle rotation au serveur !");
            }
            else
            {
                cd_ClientEntityUpdateToServer += Time.deltaTime;
            }
        }

        transform.Translate(EntityLink.LinkedEntity.GetSpeed() * DirectionVector * Time.deltaTime);
    }

    void Mouselook()
    {
        float rawX = Input.GetAxis("Mouse X");
        float rawY = Input.GetAxis("Mouse Y");

        transform.eulerAngles += new Vector3(0, rawX, 0);
        transform.Find("CameraPivot").eulerAngles += new Vector3(-rawY, 0, 0);
    }

    void HandleInput()
    {
        Changed = false;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DirectionVector.z += 1f;
            Changed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            DirectionVector.z += -1f;
            Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DirectionVector.z += -1f;
            Changed = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            DirectionVector.z += 1f;
            Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DirectionVector.x += 1f;
            Changed = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            DirectionVector.x += -1f;
            Changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Changed = true;
            DirectionVector.x += -1f;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            DirectionVector.x += 1f;
            Changed = true;
        }

        //Debugger.LogMessage(DirectionVector);
    }
}