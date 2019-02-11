using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ClientApp
{

    public ClientApp()
    {
    }

    Transmitter m_theTansmitter = new Transmitter();
    PlayerManager playerManager = PlayerManager.instance;
    string m_sName = "";

    public bool Connect(string sAddress, int iPort, string sName)
    {
        if (m_theTansmitter.Connect(sAddress, iPort) == false)
        {
            Debug.Log("Connect to server failed!");
            return false;
        }
        m_theTansmitter.SetHandleReceivedPacketEvent(_OnHandleReceivedPacket);
        m_sName = sName;

        LoginMessage msgLogin = new LoginMessage();
        msgLogin.m_sName = m_sName;
        m_theTansmitter.Send(msgLogin);

        Debug.Log("Connect to server Successed!");
        return true;
    }

    public void Run()
    {
        m_theTansmitter.Run();
    }

    public void SendMovementMessage(Vector3 fVelocity, Vector3 fPosition, float fEularAngle, int iAnimInfo)
    {
        MovementMessage msgMovement = new MovementMessage();
        msgMovement.m_fVelocityX = fVelocity.x;
        msgMovement.m_fVelocityY = fVelocity.y;
        msgMovement.m_fVelocityZ = fVelocity.z;
        msgMovement.m_fPositionX = fPosition.x;
        msgMovement.m_fPositionY = fPosition.y;
        msgMovement.m_fPositionZ = fPosition.z;
        msgMovement.m_fEularAngle = fEularAngle;
        msgMovement.m_iAnimInfo = iAnimInfo;

        m_theTansmitter.Send(msgMovement);
    }

    void _OnHandleReceivedPacket(Transmitter transmitter, int idCommand, byte[] aPacketBuffer, int iPos)
    {
        ECommand eCommand = (ECommand)idCommand;
        if (eCommand == ECommand.LOGIN)
        {
            LoginMessage msg = new LoginMessage();
            msg.UnSealPacketBuffer(aPacketBuffer, iPos);
            msg.Unserialize();

            OnLoginMessage(transmitter, msg);
        }
        else if (eCommand == ECommand.EXIT)
        {
            ExitMessage msg = new ExitMessage();
            msg.UnSealPacketBuffer(aPacketBuffer, iPos);
            msg.Unserialize();

            OnExitMessage(transmitter, msg);
        }
        else if (eCommand == ECommand.MOVEMENT)
        {
            MovementMessage msg = new MovementMessage();
            msg.UnSealPacketBuffer(aPacketBuffer, iPos);
            msg.Unserialize();

            OnMovementMessage(transmitter, msg);
        }
    }

    void OnLoginMessage(Transmitter transmitter, LoginMessage msg)
    {
        Debug.Log("player " + msg.m_sName + " login");
        playerManager.AddPlayer(msg.m_iPlayerIndex, msg.m_sName);
    }

    void OnExitMessage(Transmitter transmitter, ExitMessage msg)
    {
        Debug.Log("player offline");
        playerManager.RemovePlayer(msg.m_iPlayerIndex);
    }

    void OnMovementMessage(Transmitter transmitter, MovementMessage msg)
    {
        Vector3 velocity = new Vector3(msg.m_fVelocityX, msg.m_fVelocityY, msg.m_fVelocityZ);
        Vector3 position = new Vector3(msg.m_fPositionX, msg.m_fPositionY, msg.m_fPositionZ);
        float eularAngle = msg.m_fEularAngle;
        int animInfo = msg.m_iAnimInfo;
        playerManager.dictOtherPlayerMotor[msg.m_iPlayerIndex].HandleMovementMessage(velocity, position, eularAngle, animInfo);
    }
}
