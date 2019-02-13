using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading;

public class ClientMain : MonoBehaviour{

    ClientApp clientApp;
    bool bConnent;

    public GameObject player;
    PlayerMotor playerMotor;
    Vector3 sVelocity;
    Vector3 sPosition;
    float sEularAngle;
    int sAnimInfo;

    float delayTime = 0.1f;
    float currentTime = 0;

    void Awake()
    {
        playerMotor = player.GetComponent<PlayerMotor>();
    }

    void Start()
    {
        clientApp = new ClientApp();
        bConnent = clientApp.Connect("127.0.0.1", 4099, "kevin");
        Debug.Log(bConnent);
    }

    void Update()
    {
        if (bConnent)
        {
            clientApp.Run();

            if (currentTime < 0)
            {
                SendMovementMessage();
                currentTime += delayTime;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }        
    }

    void SendMovementMessage()
    {
        sVelocity = playerMotor.targetPoint;
        sPosition = player.transform.position;
        sEularAngle = player.transform.eulerAngles.y;
        sAnimInfo = playerMotor.GetAnimParameters();
        
        clientApp.SendMovementMessage(sVelocity, sPosition, sEularAngle, sAnimInfo);
    }
}
