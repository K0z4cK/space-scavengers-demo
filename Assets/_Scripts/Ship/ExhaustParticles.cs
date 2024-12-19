using UnityEngine;
using System.Collections.Generic;

namespace SpaceScavengers
{
    public class ExhaustParticles : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> exhaustParticlesList; // List of exhaust particle systems
        [SerializeField] private Rigidbody _rb; // Reference to the ship's Rigidbody
        private ParticleSystem.MainModule _mainModule;
        private float _sizeLerpValue = 0f; // Variable to control the gradual increase in particle size
        private float _colorLerpValue = 0f; // Variable to control the gradual change in color

        void Update()
        {
            float speed = _rb.velocity.magnitude;
            print(speed);

            // Check if the ship is moving
            if (speed > 20)
            {
                // Activate each exhaust particle system in the list
                foreach (var exhaustParticles in exhaustParticlesList)
                {
                    var mainModule = exhaustParticles.main;
                    exhaustParticles.Play();

                    // Gradually increase particle size when accelerating
                    _sizeLerpValue = Mathf.Clamp(_sizeLerpValue + Time.deltaTime, 0f, 1f);
                    mainModule.startSize = Mathf.Lerp(0.05f, 0.3f, _sizeLerpValue);

                    // Smooth transition from blue to white color
                    _colorLerpValue = Mathf.Clamp(_colorLerpValue + Time.deltaTime, 0f, 1f);
                    mainModule.startColor = Color.Lerp(Color.cyan, Color.white, speed / _rb.maxAngularVelocity * _colorLerpValue);
                }
            }
            else
            {
                // Ship is slowing down or stopped
                foreach (var exhaustParticles in exhaustParticlesList)
                {
                    var mainModule = exhaustParticles.main;

                    // Gradually decrease particle size and color lerp value
                    _sizeLerpValue = Mathf.Clamp(_sizeLerpValue - Time.deltaTime, 0f, 1f);
                    _colorLerpValue = Mathf.Clamp(_colorLerpValue - Time.deltaTime, 0f, 1f);

                    mainModule.startSize = Mathf.Lerp(0.05f, 0.3f, _sizeLerpValue);
                    mainModule.startColor = Color.Lerp(Color.cyan, Color.white, speed / _rb.maxAngularVelocity * _colorLerpValue);

                    // Stop the particle system if fully faded
                    if (_sizeLerpValue <= 0f && _colorLerpValue <= 0f)
                    {
                        exhaustParticles.Stop();
                    }
                }
            }
        }
    }
}