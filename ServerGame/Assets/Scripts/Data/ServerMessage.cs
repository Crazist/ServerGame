using System;

namespace Data
{
    [Serializable]
    public class ServerMessage<T>
    {
        public string Type;
        public T Data;
    }
}