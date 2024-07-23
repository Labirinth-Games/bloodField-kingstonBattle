using System;

namespace BloodField.Network.Entities
{
    [Serializable]
    public class MiniatureNetworkEntity : AuthorityNetworkEntity
    {
        public int y;
        public int x;
        public string miniturePath;
        public string cardPath;
        public string id;
    }
}