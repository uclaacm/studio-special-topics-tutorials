using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public abstract class UIAnimation : MonoBehaviour
    {
        public abstract void StartAnimation();
        public abstract bool IsCurrentlyAnimating {get;}
    }
}

