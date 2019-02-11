using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class TextMessage : Message
    {
        public TextMessage() : base((int)ECommand.TEXT)
        {

        }

        public string m_sName = "";
        public string m_sText = "";

        public override void Serialize()
        {
            base.Serialize();
            _WriteToBuffer(m_sName);
            _WriteToBuffer(m_sText);
        }
        public override void Unserialize()
        {
            base.Unserialize();
            _ReadFromBuffer(out m_sName);
            _ReadFromBuffer(out m_sText);
        }
    }
}

