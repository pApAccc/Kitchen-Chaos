using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
