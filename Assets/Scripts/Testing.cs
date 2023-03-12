using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class Testing : NetworkBehaviour
    {
        int testNum;
        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetKey(KeyCode.T))
            {
                TestServerRpc();
            }
            if (Input.GetKey(KeyCode.Y))
            {
                TestClientRpc();
            }
        }

        [ServerRpc]
        private void TestServerRpc()
        {
            print(OwnerClientId + " " + testNum);
        }


        [ClientRpc]
        private void TestClientRpc()
        {
            print(OwnerClientId + " " + testNum);
        }



    }



}
