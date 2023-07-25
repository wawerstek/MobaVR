using System;
using BNG;
using UnityEngine;

namespace MobaVR
{
    public class Quiver : MonoBehaviour
    {
        public Action<Grabber> OnGrabberTriggerEnter;
        public Action<Grabber> OnGrabberTriggerExit;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Grabber grabber))
            {
                OnGrabberTriggerEnter?.Invoke(grabber);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Grabber grabber))
            {
                OnGrabberTriggerExit?.Invoke(grabber);
            }
        }
    }
}