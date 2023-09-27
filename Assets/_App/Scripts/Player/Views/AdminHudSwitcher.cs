using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class AdminHudSwitcher : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var users = FindObjectsOfType<HudSwitcher>();
                foreach (HudSwitcher hudSwitcher in users)
                {
                    hudSwitcher.ShowLocalHud();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                var users = FindObjectsOfType<HudSwitcher>();
                foreach (HudSwitcher hudSwitcher in users)
                {
                    hudSwitcher.HideLocalHud();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.C))
            {
                var users = FindObjectsOfType<HudSwitcher>();
                foreach (HudSwitcher hudSwitcher in users)
                {
                    hudSwitcher.ShowRemoteHud();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                var users = FindObjectsOfType<HudSwitcher>();
                foreach (HudSwitcher hudSwitcher in users)
                {
                    hudSwitcher.HideRemoteHud();
                }
            }
        }
    }
}