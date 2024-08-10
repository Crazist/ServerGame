using System;
using Newtonsoft.Json.Linq;

namespace Data
{
    [Serializable]
    public class ClientMessage<T>
    {
        public string Type;
        public T Data;
    }
}