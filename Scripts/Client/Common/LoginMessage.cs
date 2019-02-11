using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class LoginMessage : Message
    {
        public LoginMessage() : base((int)ECommand.LOGIN)
        {

        }

        public int m_iPlayerIndex = 0;
        public string m_sName = "";

        public override void Serialize()
        {
            base.Serialize();
            _WriteToBuffer(m_iPlayerIndex);
            _WriteToBuffer(m_sName);
        }
        public override void Unserialize()
        {
            base.Unserialize();
            _ReadFromBuffer(out m_iPlayerIndex);
            _ReadFromBuffer(out m_sName);
        }
    }
}

