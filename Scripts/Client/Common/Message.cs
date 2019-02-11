using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class Message
    {
        public Message(int iCommand)
        {
            m_iCommand = iCommand;
        }

        static Message m_theHeader = new Message(0);

        protected int m_iCommand;
        protected int m_iLength;

        protected int m_iPos = 0;
        protected int m_iBeginPos = 0;
        
        byte[] m_aPacketBuffer = new byte[1024];

        public int iCommand { get { return m_iCommand; } }

        public virtual void Serialize()
        {
            m_iPos = m_iBeginPos;

            _WriteToBuffer(m_iLength);
            _WriteToBuffer(m_iCommand);
        }
        public virtual void Unserialize()
        {
            m_iPos = m_iBeginPos;

            _ReadFromBuffer(out m_iLength);
            _ReadFromBuffer(out m_iCommand);
        }

        public byte[] SealPacketBuffer(out int iLength)
        {
            m_iLength = m_iPos;

            int iCurPos = m_iPos;
            m_iPos = m_iBeginPos;
            _WriteToBuffer(m_iLength);
            m_iPos = iCurPos;

            iLength = m_iLength;
            return m_aPacketBuffer;
        }
        public void UnSealPacketBuffer(byte[] aPacketData, int iBeginPos)
        {
            m_aPacketBuffer = aPacketData;
            m_iBeginPos = iBeginPos;
        }

        public static void FetchHeader(out int iLength, out int iCommand, byte[] aPacketData, int iBeginPos)
        {
            m_theHeader.UnSealPacketBuffer(aPacketData, iBeginPos);
            m_theHeader.Unserialize();

            iLength = m_theHeader.m_iLength;
            iCommand = m_theHeader.m_iCommand;
        }


        protected bool _WriteToBuffer(int i)
        {
            byte[] aBytes = BitConverter.GetBytes(i);

            _WriteToBuffer(aBytes);
            return true;
        }
        protected bool _ReadFromBuffer(out int i)
        {
            if (BitConverter.IsLittleEndian == false)
            {
                byte[] byteData = new byte[sizeof(int)];
                Buffer.BlockCopy(m_aPacketBuffer, m_iBeginPos + m_iPos, byteData, 0, byteData.Length);
                Array.Reverse(byteData);
                i = BitConverter.ToInt32(byteData, 0);
            }
            else i = BitConverter.ToInt32(m_aPacketBuffer, m_iBeginPos + m_iPos);

            m_iPos += sizeof(int);
            return true;
        }

        protected bool _WriteToBuffer(float f)
        {
            byte[] aBytes = BitConverter.GetBytes(f);

            _WriteToBuffer(aBytes);
            return true;
        }
        protected bool _ReadFromBuffer(out float f)
        {
            if (BitConverter.IsLittleEndian == false)
            {
                byte[] byteData = new byte[sizeof(float)];
                Buffer.BlockCopy(m_aPacketBuffer, m_iBeginPos + m_iPos, byteData, 0, byteData.Length);
                Array.Reverse(byteData);
                f = BitConverter.ToSingle(byteData, 0);
            }
            else f = BitConverter.ToSingle(m_aPacketBuffer, m_iBeginPos + m_iPos);
            
            m_iPos += sizeof(float);
            return true;
        }

        protected bool _WriteToBuffer(string s)
        {
            byte[] aBytes = System.Text.Encoding.Unicode.GetBytes(s);

            if (_WriteToBuffer(aBytes.Length) == false) return false;

            _WriteToBuffer(aBytes);
            return true;
        }
        protected bool _ReadFromBuffer(out string s)
        {
            int iLen;
            _ReadFromBuffer(out iLen);

            s = System.Text.Encoding.Unicode.GetString(m_aPacketBuffer, m_iBeginPos + m_iPos, iLen);
            m_iPos += iLen;
            return true;
        }

        void _WriteToBuffer(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian == false) Array.Reverse(byteData);
            byteData.CopyTo(m_aPacketBuffer, m_iBeginPos + m_iPos);
            m_iPos += byteData.Length;
        }
    }
}

