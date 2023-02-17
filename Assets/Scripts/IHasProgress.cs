using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public interface IHasProgress
    {
        public event EventHandler<OnProgressBarChangedEventArgs> OnProgressBarChanged;
        public class OnProgressBarChangedEventArgs : EventArgs
        {
            public float progressNormalized;
        }
    }
}
