using System;

namespace Data
{
    [Serializable]
    public class ClientMessage
    {
        public string Type;
        public object Data;
    }
}