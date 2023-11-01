using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class GeneralAnimator : MonoBehaviour {
        public GameObject targetObject;

        private readonly Dictionary<int, AnimationEventChannel> eventChannelDict = new();

        public void Destroy() {
            Destroy(gameObject);
        }

        public void ActiveTargetObject() {
            targetObject.SetActive(true);
        }

        public void DeactiveTargetObject() {
            targetObject.SetActive(false);
        }

        public void DestroyTargetObject() {
            Destroy(targetObject);
        }

        public void AddEventListener(Action action, int channelNum) {
            AnimationEventChannel channel = GetOrCreateChannel(channelNum);
            channel.action += action;
        }

        public void RemoveEventListener(Action action, int channelNum) {
            AnimationEventChannel channel = GetChannel(channelNum);
            if (channel != null)
                channel.action -= action;
        }

        public void InvokeEventListeners(int channelNum) {
            AnimationEventChannel channel = GetChannel(channelNum);
            if (channel != null)
                channel.Invoke();
        }

        private AnimationEventChannel GetOrCreateChannel(int channel) {
            if (!eventChannelDict.TryGetValue(channel, out AnimationEventChannel animationEventChannel)) {
                animationEventChannel = new AnimationEventChannel {
                    channel = channel
                };
                eventChannelDict.Add(channel, animationEventChannel);
            }

            return animationEventChannel;
        }

        private AnimationEventChannel GetChannel(int channel) {
            if (!eventChannelDict.TryGetValue(channel, out AnimationEventChannel animationEventChannel))
                return null;

            return animationEventChannel;
        }

        public class AnimationEventChannel {
            public int channel;
            public event Action action;

            public void Invoke() {
                action?.Invoke();
            }
        }
    }
}