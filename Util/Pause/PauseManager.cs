using System;
using System.Collections.Generic;
using System.Linq;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient {
    public delegate void PauseStateChangedDelegate(bool paused);

    public delegate void PauseRejectedDelegate(float leftCooltime);

    public class PauseManager : MonoBehaviour {
        private struct ParticleSystemState {
            public ParticleSystem particle;
            public bool state;
        }

        private struct PauseableState {
            public MonoBehaviour pauseable;
            public bool state;
        }

        private struct AnimatorState {
            public Animator animator;
            public bool state;
        }

        private const float PauseCooltime = 5f;
        public static PauseManager Instance { get; private set; }

        public bool IsPaused { get; private set; }

        private ParticleSystemState[] particleStates;
        private PauseableState[] pauseableStates;
        private AnimatorState[] animatorStates;


        private float pauseLeftCooltime;

        public event PauseStateChangedDelegate PauseStateChanged;
        public event PauseRejectedDelegate PauseRejected;

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (!IsPaused)
                pauseLeftCooltime = Mathf.Max(0f, pauseLeftCooltime - Time.deltaTime);
        }

        public void Pause() {
            if (IsPaused) return;

            if (GameManager.Instance.IsPlaying && GameManager.Instance.PlayingGame.IsBattlePhase &&
                pauseLeftCooltime > 0f) {
                PauseRejected?.Invoke(pauseLeftCooltime);
                return;
            }

            IsPaused = true;

            pauseLeftCooltime = PauseCooltime;

            // Pause Particle Systems
            ParticleSystem[] particles = FindObjectsOfType<ParticleSystem>();
            particleStates = new ParticleSystemState[particles.Length];

            for (int i = 0; i < particles.Length; ++i) {
                particleStates[i] = new ParticleSystemState {
                    particle = particles[i],
                    state = particles[i].isPlaying
                };
                particles[i].Pause();
            }

            // Pause Pauseables
            MonoBehaviour[] pauseables = FindObjectsOfType<MonoBehaviour>(true).OfType<IPauseable>()
                .Select(x => x as MonoBehaviour).ToArray();
            pauseableStates = new PauseableState[pauseables.Length];

            for (int i = 0; i < pauseables.Length; ++i) {
                pauseableStates[i] = new PauseableState {
                    pauseable = pauseables[i],
                    state = pauseables[i].enabled
                };
                pauseables[i].enabled = false;
            }

            // Pause Animators
            Animator[] animators = FindObjectsOfType<Animator>();
            animatorStates = new AnimatorState[animators.Length];

            for (int i = 0; i < animators.Length; ++i) {
                animatorStates[i] = new AnimatorState {
                    animator = animators[i],
                    state = animators[i].enabled
                };
                animators[i].enabled = false;
            }

            PauseStateChanged?.Invoke(true);
        }

        public void Resume() {
            if (!IsPaused) return;
            IsPaused = false;

            // Resume Particle Systems
            for (int i = 0; i < particleStates.Length; ++i) {
                if (particleStates[i].particle == null) continue;
                if (particleStates[i].state)
                    particleStates[i].particle.Play();
            }

            // Resume Pauseables
            for (int i = 0; i < pauseableStates.Length; ++i) {
                if (pauseableStates[i].pauseable == null) continue;
                pauseableStates[i].pauseable.enabled = pauseableStates[i].state;
            }

            // Resume Animators
            for (int i = 0; i < animatorStates.Length; ++i) {
                if (animatorStates[i].animator == null) continue;
                animatorStates[i].animator.enabled = animatorStates[i].state;
            }

            particleStates = null;
            pauseableStates = null;
            animatorStates = null;

            PauseStateChanged?.Invoke(false);
        }
    }
}