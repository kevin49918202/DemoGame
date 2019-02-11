using System;
using System.Collections.Generic;

namespace Common
{
    public class ExitMessage : Message
    {
        public ExitMessage() : base((int)ECommand.EXIT)
        {
        }

        public override void Serialize()
        {
            base.Serialize();

            _WriteToBuffer(m_iPlayerIndex);
        }
        public override void Unserialize()
        {
            base.Unserialize();

            _ReadFromBuffer(out m_iPlayerIndex);
        }

        public int m_iPlayerIndex = 0;
    }
}
