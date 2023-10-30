using UnityEngine;
using Mirror.Discovery;
using UnityEngine.Events;
using System.Collections;

namespace Network
{
    public class DiscoveryNetworkManager : MonoBehaviour
    {
        [Header("Callbacks")]
        public UnityEvent<ServerResponse> onDiscoveryServerFound;
        public UnityEvent OnStartSearching;
        public UnityEvent OnFinishSearching;
        public UnityEvent OnErrorSearching;

        private NetworkDiscovery _networkDiscovery;
        private Coroutine _coroutine;

        private void Start()
        {
            _networkDiscovery = GetComponent<NetworkDiscovery>();
            _networkDiscovery.OnServerFound.AddListener(OnServerFound); 
        }

        public void StartHostDiscovery()
        {
            _networkDiscovery.StartDiscovery();
            OnStartSearching?.Invoke();

            _coroutine = StartCoroutine(WaitToStopDiscovery());
        }

        IEnumerator WaitToStopDiscovery()
        {
            yield return new WaitForSeconds(20f);
            OnErrorSearching?.Invoke();

            StopHostDiscovery();
        }

        public void StopHostDiscovery()
        {
            _networkDiscovery.StopDiscovery();
        }

        // Advertise this Server in local networking
        public void AdvertiseServer()
        {
            _networkDiscovery.AdvertiseServer();
        }

        public void OnServerFound(ServerResponse response)
        {
            StopHostDiscovery();
            OnFinishSearching?.Invoke();

            StopCoroutine(_coroutine);

            onDiscoveryServerFound?.Invoke(response);
        }
    }

}