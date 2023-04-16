using UnityEngine;

namespace MobaVR.Utils
{
    public static class ParticleSystemExtension
    {
        public static void SetColorOverLifeTime(this ParticleSystem particleSystem, ParticleSystem.MinMaxGradient color)
        {
            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = color;
        }
        
        public static void SetTrailColor(this ParticleSystem particleSystem, ParticleSystem.MinMaxGradient color)
        {
            ParticleSystem.TrailModule trailModule = particleSystem.trails;
            trailModule.enabled = true;
            trailModule.colorOverTrail = color;
        }
    }
}