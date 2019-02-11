using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class MovementMessage : Message
    {
        public MovementMessage() : base((int)ECommand.MOVEMENT)
        {

        }

        public int m_iPlayerIndex = 0;

        public float m_fVelocityX, m_fVelocityY, m_fVelocityZ;
        public float m_fPositionX, m_fPositionY, m_fPositionZ;
        public float m_fEularAngle;
        public int m_iAnimInfo;

        public override void Serialize()
        {
            base.Serialize();
            _WriteToBuffer(m_iPlayerIndex);
            _WriteToBuffer(m_fVelocityX);
            _WriteToBuffer(m_fVelocityY);
            _WriteToBuffer(m_fVelocityZ);
            _WriteToBuffer(m_fPositionX);
            _WriteToBuffer(m_fPositionY);
            _WriteToBuffer(m_fPositionZ);
            _WriteToBuffer(m_fEularAngle);
            _WriteToBuffer(m_iAnimInfo);
        }
        public override void Unserialize()
        {
            base.Unserialize();
            _ReadFromBuffer(out m_iPlayerIndex);
            _ReadFromBuffer(out m_fVelocityX);
            _ReadFromBuffer(out m_fVelocityY);
            _ReadFromBuffer(out m_fVelocityZ);
            _ReadFromBuffer(out m_fPositionX);
            _ReadFromBuffer(out m_fPositionY);
            _ReadFromBuffer(out m_fPositionZ);
            _ReadFromBuffer(out m_fEularAngle);
            _ReadFromBuffer(out m_iAnimInfo);
        }
    }
}

