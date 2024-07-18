using System.Threading.Tasks;
using BloodField.SO;
using Managers;
using Nakama;
using UnityEngine;

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

        private async Task Authentication()
        {
            PlayerPrefs.SetString(_clientRefName, System.Guid.NewGuid().ToString());

            Session = await Client.AuthenticateDeviceAsync(PlayerPrefs.GetString(_clientRefName));

            PlayerPrefs.SetString(_sessionTokenName, Session.AuthToken);
            PlayerPrefs.SetString(_sessionRefreshTokenName, Session.RefreshToken);

            Socket = Client.NewSocket(true);
            await Socket.ConnectAsync(Session, true, 30);
        }

        public async Task FindMatch(string name)
        {
            await Authentication();

            await Client.UpdateAccountAsync(Session, name, name);

            // assign in match
            var matchmakerTicket = await Socket.AddMatchmakerAsync("*", 2, 2);
            _ticket = matchmakerTicket.Ticket;

            Socket.ReceivedMatchmakerMatched += GameManager.Instance.eventManager.ReceivedMatchmakerMatchedEvent;
            Socket.ReceivedMatchPresence += GameManager.Instance.eventManager.ReceivedMatchPresenceEvent;
            Socket.ReceivedStatusPresence += GameManager.Instance.eventManager.ReceivedStatusPresenceEvent;
            Socket.ReceivedMatchState += GameManager.Instance.eventManager.ReceivedMatchStateEvent;

            GameManager.Instance.setSessionId(Session.UserId);
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