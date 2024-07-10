using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnprogressChangedEventArgs> OnProgressChanged;
    public class OnprogressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
