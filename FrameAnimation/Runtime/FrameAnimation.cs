using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAnimation {
    public class FrameAnimation : IDisposable {
        private FrameAnimationAsset asset;

        private List<IFramePlayer> players;
        private int sampleRate = 24;
        private bool loop = false;
        private bool playOnAwake;

        private float frameLength;
        private int beginFrame;
        private int endFrame;
        private int frameCount;
        private float length;

        private bool usable;
        private float frameTimer;
        private int curFrame;

        public FrameAnimationAsset Asset => asset;
        public bool PlayOnAwake => playOnAwake;
        public float startTime => beginFrame;
        public float stopTime => endFrame;
        public int CurFrame => curFrame;
        public float CurTime => curFrame * frameLength;

        public float Delay { get => delay; set => delay = value; }
        private float delay;
        private float delayTimer;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameAnimation(Transform root, FrameAnimationAsset asset, bool isEdit = false) {
            this.asset = asset;

            players = new List<IFramePlayer>();
            if(asset.Tracks != null) {
                foreach(var track in asset.Tracks)
                    players.Add(track.CreatePlayer(root));
            }
            sampleRate = asset.SampleRate;
            loop = asset.Loop;
            playOnAwake = asset.PlayOnAwake;

            frameLength = sampleRate <= 0 ? 0f : 1f / sampleRate;
            beginFrame = isEdit ? players.Select(i => i.BeginFrame).Min() : 0;
            endFrame = players.Select(i => i.EndFrame).Max();
            frameCount = endFrame - beginFrame + 1;
            length = frameCount * frameLength;

            usable = length > 0f;

            foreach(var player in players)
                player.InitCurve(beginFrame, endFrame);
        }
        public void Play() {
            frameTimer = 0f;
            curFrame = beginFrame;
            SetFrame(curFrame);
        }
        public bool Update(float time) {
            if(!usable)
                return false;
            if(Delay > 0f)
                delayTimer += time;
            if(delayTimer < Delay)
                return true;

            frameTimer += time;
            LerpFrame(((curFrame - beginFrame) * frameLength + frameTimer) / length);
            if(frameTimer >= frameLength) {
                int step = (int)(frameTimer / frameLength);
                frameTimer = frameTimer % frameLength;
                curFrame += step;
                if(curFrame > endFrame) {
                    if(loop) {
                        delayTimer = 0f;
                        curFrame = beginFrame;
                    } else {
                        return false;
                    }
                }
                SetFrame(curFrame);
            }
            return true;
        }
        public void NextFrame() {
            curFrame++;
            if(curFrame > endFrame)
                curFrame = loop ? beginFrame : endFrame;
            SetFrame(curFrame);
        }
        public void PrevFrame() {
            curFrame--;
            if(curFrame < beginFrame)
                curFrame = loop ? endFrame : beginFrame;
            SetFrame(curFrame);
        }
        public void Reset() {
            foreach(var player in players)
                player.Reset();
        }
        public void Dispose() {
            if (players != null) {
                foreach (var player in players) {
                    player.Dispose();
                }
                players.Clear();
                players = null;
            }
            GC.SuppressFinalize(this);
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void LerpFrame(float smooth) {
            foreach(var player in players) {
                if(player.Enable)
                    player.LerpFrame(smooth);
            }
        }
        private void SetFrame(int frameIndex) {
            foreach(var player in players) {
                if(player.Enable)
                    player.SetFrame(frameIndex);
            }
        }
    }
}