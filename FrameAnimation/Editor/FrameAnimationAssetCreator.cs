using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;

namespace VAnimation {
    public static class FrameAnimationAssetCreator {
        private static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        /******************************************************************
         *
         *      method
         *
         ******************************************************************/
        [MenuItem(CM, false, -1000)]
        private static void Create() {
            string path = GetPath();
            path = AssetDatabase.GenerateUniqueAssetPath(path + '/' + ASSET_NAME);
            var asset = ScriptableObject.CreateInstance<FrameAnimationAsset>();
            try {
                SetAsset(asset, GetSprites());
            } finally {
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        public static bool Create(string filePath, List<Sprite> frames, int sampleRate, bool loop, string dirTag) {
            if(frames == null || frames.Count <= 0)
                return false;
            AssetDatabase.DeleteAsset(filePath);
            var asset = ScriptableObject.CreateInstance<FrameAnimationAsset>();
            SetAsset(asset, frames.ToArray(), sampleRate, loop, dirTag);
            AssetDatabase.CreateAsset(asset, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
        }
        private static string GetPath() {
            var obj = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(obj);
            if(obj.GetType() == typeof(DefaultAsset))
                return path;
            int pathPos = path.LastIndexOf('/');
            return path.Substring(0, pathPos);
        }
        public static Sprite[] GetSprites() {
            var objs = Selection.objects;
            if(objs == null)
                return null;
            List<Sprite> col = new List<Sprite>();
            foreach(var obj in objs) {
                if(obj.GetType() == typeof(Sprite))
                    col.Add(obj as Sprite);
                else if(obj.GetType() == typeof(Texture2D)) {
                    var path = AssetDatabase.GetAssetPath(obj);
                    path = GetDirPath(path);
                    string[] guids = AssetDatabase.FindAssets($"{obj.name} t:Sprite", new string[] { path });
                    if(guids == null || guids.Length <= 0)
                        continue;
                    string sPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(sPath);
                    col.Add(sprite);
                }
            }
            return col.Count > 0 ? col.ToArray() : null;
        }
        private static void SetAsset(FrameAnimationAsset asset, Sprite[] sprites) {
            if(asset == null)
                return;
            if(sprites == null)
                return;
            var track = new FrameSpriteTrack();
            SetValue(track, FIELD_COMPONENT_PATH, "Body");
            SetValue(track, FIELD_FRAMES, new List<FrameSpriteData>());
            for(int i = 0; i < sprites.Length; ++i) {
                var data = new FrameSpriteData();
                SetValue(data, FIELD_FRAME_INDEX, i);
                SetValue(data, FIELD_FRAME, sprites[i]);
                track.Datas.Add(data);
            }
            SetValue(asset, FIELD_TRACKS, new List<FrameTrackBase>());
            asset.Tracks.Add(track);
        }
        private static void SetAsset(FrameAnimationAsset asset, Sprite[] sprites, int sampleRate, bool loop, string dirTag) {
            if(asset == null || sprites == null)
                return;
            SetAsset(asset, sprites);
            SetValue(asset, FIELD_SAMPLE_RATE, sampleRate);
            SetValue(asset, FIELD_LOOP, loop);
            switch(dirTag) {
                case "left": AddTrack(asset, EVENT_KEY_LEFT); break;
                case "right": AddTrack(asset, EVENT_KEY_RIGHT); break;
                default: break;
            }
        }
        private static void AddTrack(FrameAnimationAsset asset, string eventKey) {
            var track = new FrameEventTrack();
            SetValue(track, FIELD_FRAMES, new List<FrameEventData>());
            var data = new FrameEventData();
            SetValue(data, FIELD_EVENT_KEY, eventKey);
            track.Datas.Add(data);
            asset.Tracks.Add(track);
        }
        private static void SetValue<T>(T obj, string field, object value) {
            Type type = typeof(T);
            FieldInfo info = type.GetField(field, flags);
            info.SetValue(obj, value);
        }
        private static string GetDirPath(string path) {
            int index = path.LastIndexOf('/');
            return path.Substring(0, index);
        }
        /******************************************************************
         *
         *      const define
         *
         ******************************************************************/
        private const string CM = "Assets/Create/Frame Animation Asset";
        private const string ASSET_NAME = "FrameAnimation.asset";
        private const string EVENT_KEY_LEFT = "BackToTarget";
        private const string EVENT_KEY_RIGHT = "FaceToTarget";

        private const string FIELD_COMPONENT_PATH = "componentPath";
        private const string FIELD_FRAMES = "frames";
        private const string FIELD_FRAME_INDEX = "frameIndex";
        private const string FIELD_FRAME = "frame";
        private const string FIELD_TRACKS = "tracks";

        private const string FIELD_SAMPLE_RATE = "sampleRate";
        private const string FIELD_LOOP = "loop";
        private const string FIELD_EVENT_KEY = "eventKey";
    }
}