using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class LookatCamera : MonoBehaviour
    {
        private enum Mode
        {
            CameraForward,
            CameraForwardInterval
        }

        [SerializeField] private Mode mode;
        private void LateUpdate()
        {
            switch (mode)
            {
                case Mode.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;
                case Mode.CameraForwardInterval:
                    transform.forward = -Camera.main.transform.forward;
                    break;
            }


        }
    }
}
