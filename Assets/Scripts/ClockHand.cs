using System;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class ClockHand : MonoBehaviour
    {
        public void RotateHand(float value)
        {
            transform.eulerAngles = new Vector3(0,0, -value);
        }
    }
}
