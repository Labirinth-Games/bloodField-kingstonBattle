using Nakama;
using UnityEngine;

namespace BloodField.SO
{
    [CreateAssetMenu(fileName = "NakamaConnectionSO", menuName = "ScriptableObjects/Connection", order = 0)]
    public class NetworkConnectionSO : ScriptableObject
    {
        public string scheme = "http";
        public string host = "127.0.0.1";
        public int port = 7350;
        public string serverKey = "defaultkey";
    }
}