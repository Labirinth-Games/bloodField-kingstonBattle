using System.Threading.Tasks;
using BloodField.SO;
using BloodField.Managers;
using Nakama;
using UnityEngine;
using System.Net.NetworkInformation;
using System.Linq;
using System;

namespace BloodField.Network
{
    public class NetworkManager : MonoBehaviour
    {
        private const string _clientRefName = "nakama.clientId";
        private const string _sessionTokenName = "nakama.SessionToken";
        private const string _sessionRefreshTokenName = "nakama.SessionRefreshToken";

        [SerializeField] private NetworkConnectionSO connectionSO;

        public IClient Client;
        public ISession Session;
        public ISocket Socket;

        private string _ticket;

        public void Connect()
        {
            Client = new Client(connectionSO.scheme, connectionSO.host, connectionSO.port, connectionSO.serverKey, UnityWebRequestAdapter.Instance);
        }

        public async Task<IApiAccount> Authenticator()
        {
            try
            {
                Connect();

                var firstMacAddress = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();

                Session = await Client.AuthenticateDeviceAsync(firstMacAddress);

                PlayerPrefs.SetString(_sessionTokenName, Session.AuthToken);
                PlayerPrefs.SetString(_sessionRefreshTokenName, Session.RefreshToken);

                return await Client.GetAccountAsync(Session);
            }
            catch (ApiResponseException err)
            {
                throw err;
            }
        }

        public async Task UpdateUserRegister(string name)
        {
            try
            {
                await Client.UpdateAccountAsync(Session, name, name);
            }
            catch (ApplicationException err)
            {
                throw err;
            }
        }

        public async Task FindMatch()
        {
            Socket = Client.NewSocket(true);
            await Socket.ConnectAsync(Session, true, 30);

            // assign in match
            var matchmakerTicket = await Socket.AddMatchmakerAsync("*", 2, 2);
            _ticket = matchmakerTicket.Ticket;

            Socket.ReceivedMatchmakerMatched += GameManager.Instance.eventManager.ReceivedMatchmakerMatchedEvent;
            Socket.ReceivedMatchPresence += GameManager.Instance.eventManager.ReceivedMatchPresenceEvent;
            Socket.ReceivedStatusPresence += GameManager.Instance.eventManager.ReceivedStatusPresenceEvent;
            Socket.ReceivedMatchState += GameManager.Instance.eventManager.ReceivedMatchStateEvent;

            GameManager.Instance.SetUserId(Session.UserId);
        }
        
        public async Task ExitMatch()
        {
            await Socket.RemoveMatchmakerAsync(_ticket);
        }

        public async Task Logoff()
        {
            await Client.DeleteAccountAsync(Session);
        }
    }
}