using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class CharacterReady : NetworkBehaviour
    {
        public static CharacterReady Instance { get; private set; }

        public event EventHandler OnReadyChanged;

        private Dictionary<ulong, bool> playerReadyDictionary;

        private void Awake()
        {
            Instance = this;
            playerReadyDictionary = new Dictionary<ulong, bool>();
        }

        public void SetPlayerReady()
        {
            SetPlayerReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
            playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
            bool isAllReady = true;

            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientID) || playerReadyDictionary[clientID] == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            if (isAllReady)
            {
                KitchenGameLobby.Instance.DeleteLobby();
                Loader.LoadSceneNetwork(Loader.SceneName.GameScene);
            }
        }

        [ClientRpc]
        private void SetPlayerReadyClientRpc(ulong clientID)
        {
            playerReadyDictionary[clientID] = true;
            OnReadyChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool GetPlayerIsReady(ulong clientID)
        {
            return playerReadyDictionary.ContainsKey(clientID) && playerReadyDictionary[clientID];
        }
    }
}
