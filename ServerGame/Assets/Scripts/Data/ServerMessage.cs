using System;

namespace Data
{
    [Serializable]
    public class ServerMessage
    {
        public string Type;
        public object Data;
    }
}