using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Net;
using UnityEngine;

namespace Common
{
    public class Transmitter
    {
        public Transmitter(TcpClient theClient = null)
        {
            m_theClient = theClient;
        }

        public delegate void OnHandleReceivedPacketEvent(Transmitter transmitter, int idCommand, byte[] aPacketBuffer, int iPos);

        TcpClient m_theClient = null;
        string m_sAddress;
        int m_iPort;

        public bool Connect(string sAddress, int iPort)
        {
            m_sAddress = sAddress;
            m_iPort = iPort;

            m_theClient = new TcpClient();
            try
            {
                IPHostEntry host = Dns.GetHostEntry(sAddress);
                var address = (from h in host.AddressList where h.AddressFamily == AddressFamily.InterNetwork select h).First();

                m_theClient.Connect(address.ToString(), iPort);
                //
                return true;
            }
            catch (Exception e)
            {
                //
                return false;
            }
        }

        public bool IsConnected()
        {
            if (m_theClient.Connected == false) return false;

            try
            {
                if (m_theClient.Client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (m_theClient.Client.Receive(buff, SocketFlags.Peek) == 0) return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            m_theClient.Close();
        }

        public void Send(Message msg)
        {
            msg.Serialize();

            int iLength;
            byte[] aPacketData = msg.SealPacketBuffer(out iLength);
            m_theClient.GetStream().Write(aPacketData, 0, iLength);
        }

        public void SetHandleReceivedPacketEvent(OnHandleReceivedPacketEvent fnOnHandleReceivedPacket)
        {
            m_fnOnHandleReceivedPacket = fnOnHandleReceivedPacket;
        }

        public void Run()
        {
            if (m_theClient.Available > 0)
            {
                _HandleRecciveMessage();
            }
        }

        void _HandleRecciveMessage()
        {
            int iNumBytes = m_theClient.Available;
            byte[] aPacketBuffer = new byte[iNumBytes];

            int iBytesRead = m_theClient.GetStream().Read(aPacketBuffer, 0, iNumBytes);
            if (iBytesRead != iNumBytes)
            {
                //
                return;
            }

            int iPos = 0;
            while(iPos < iBytesRead)
            {
                int iLength, idCommand;
                Message.FetchHeader(out iLength, out idCommand, aPacketBuffer, iPos);

                m_fnOnHandleReceivedPacket(this, idCommand, aPacketBuffer, iPos);

                iPos += iLength;
            }
        }

        OnHandleReceivedPacketEvent m_fnOnHandleReceivedPacket;
    }
}

