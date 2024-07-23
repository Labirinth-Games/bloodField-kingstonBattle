using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BloodField.Enums;
using BloodField.Managers;
using BloodField.Network.Entities;
using Nakama;
using Nakama.TinyJson;

namespace BloodField.Helpers
{
    static class NetworkHelper
    {
        public async static Task Send<T>(long opCode, T data)
        {
            if (GameManager.Instance.networkManager.Socket is not null)
            {
                // if (data.GetType().GetProperty("userId") is not null)
                (data as AuthorityNetworkEntity).userId = GameManager.Instance.UserId;

                await GameManager.Instance.networkManager.Socket.SendMatchStateAsync(GameManager.Instance.matchManager.MatchId, opCode, JsonWriter.ToJson(data));
            }
        }

        public static void Listen<T>(IMatchState matchState, long state, Action<T, bool, bool> callback)
        {
            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);
            var content = JsonParser.FromJson<Dictionary<string, string>>(jsonUtf8);
            var isOwner = content.ContainsKey("userId") && content["userId"] == GameManager.Instance.UserId;
            var isHost = GameManager.Instance.IsHost;

            if (matchState.OpCode == state) callback(JsonParser.FromJson<T>(jsonUtf8), isHost, isOwner);
        }
    }
}