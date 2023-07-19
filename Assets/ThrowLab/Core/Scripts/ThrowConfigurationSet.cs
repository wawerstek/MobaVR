using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudFine.ThrowLab
{
    [Serializable]
    public class ThrowConfigurationSet
    {
        [SerializeField] private ThrowConfiguration[] _deviceConfigurations;
        
        public ThrowConfiguration[] DeviceConfigurations => _deviceConfigurations;

        public ThrowConfiguration GetConfigForDevice(Device device)
        {
            if (DeviceConfigurations[(int)device] == null)
            {
                Debug.LogWarning("No ThrowConfiguration set for " + device.ToString());
                DeviceConfigurations[(int)device] = new ThrowConfiguration();
            }
            return DeviceConfigurations[(int)device];
        }

        public void SetConfigForDevice(Device device, ThrowConfiguration config)
        {
            DeviceConfigurations[(int)device] = config;
        }

        public void SetConfigs(ThrowConfiguration[] set)
        {
            _deviceConfigurations = set;
        }
    }
}
