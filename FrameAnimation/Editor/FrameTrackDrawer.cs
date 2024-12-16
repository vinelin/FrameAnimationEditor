using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Cinemachine.Editor;

namespace VAnimation {
    [CustomPropertyDrawer(typeof(FrameTrackBase))]
    public class FrameTrackDrawer : PropertyDrawer {
        private static Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsSubclassOf(typeof(FrameTrackBase)) && !x.IsAbstract)
            .ToArray();
        private static string[] typenames = types.Select(x => x.Name).ToArray();

        private static GUIContent cempty = new GUIContent(string.Empty);
        private const float BTN_WIDTH = 40f;
        private const float PAD = 2f;

        private bool dragging = false;
        private bool resetFlag = false;
        private int trackIdx;
        private Sprite[] spriteCaches;
        private const int DELAY = 8;
        private int delay;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            var type = property.managedReferenceValue?.GetType();
            bool isSpriteTrack = type == typeof(FrameSpriteTrack);
            // 获取当前类型的索引
            var index = Array.IndexOf(types, type);
            // 获取下拉框的位置
            var n = isSpriteTrack ? 2 : 1;
            var rect = new Rect(position.x, position.y, position.width - BTN_WIDTH * n - PAD * n, EditorGUIUtility.singleLineHeight);
            // 绘制下拉框
            index = EditorGUI.Popup(rect, property.displayName, index, typenames);
            // 排序按钮
            var frames = property.FindPropertyRelative("frames");
            var rectBtn = new Rect(rect.xMax + PAD, position.y, BTN_WIDTH, EditorGUIUtility.singleLineHeight);
            if(GUI.Button(rectBtn, "Sort")) {
                if(index >= 0 && index < types.Length) {
                    bool dirty = false;
                    if(frames.isArray) {
                        int maxIndex = -1;
                        for(int i = 0; i < frames.arraySize; ++i) {
                            var element = frames.GetArrayElementAtIndex(i);
                            var frameIndexProperty = element.FindPropertyRelative("frameIndex");
                            var frameIndex = frameIndexProperty == null ? -1 : frameIndexProperty.intValue;
                            if(frameIndex > maxIndex) {
                                maxIndex = frameIndex;
                            } else {
                                frameIndex = maxIndex + 1;
                                maxIndex = frameIndex;
                                frameIndexProperty.intValue = frameIndex;
                                dirty = true;
                            }
                        }
                    }
                    if(dirty)
                        property.serializedObject.ApplyModifiedProperties();
                }
            }
            // 重置图片按钮
            if(isSpriteTrack) {
                ResetSprites(property);
                var rectReset = new Rect(rectBtn.xMax + PAD, position.y, BTN_WIDTH, EditorGUIUtility.singleLineHeight);
                HandleDrag(rectReset, property);
            }
            // 设置当前类型
            if(index >= 0) {
                var serType = types[index];
                if(property.managedReferenceValue?.GetType() != serType) {
                    property.managedReferenceValue = Activator.CreateInstance(serType);
                }
            }
            EditorGUI.PropertyField(position, property, cempty, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private void HandleDrag(Rect position, SerializedProperty property) {
            GUI.Box(position, "Reset");
            Event e = Event.current;
            if(!position.Contains(e.mousePosition))
                return;

            switch(e.type) {
                case EventType.DragUpdated:
                    dragging = true;
                    break;
                case EventType.Used:
                    if(dragging) {
                        var p = property.serializedObject.FindProperty("tracks");
                        for(int i = 0; i < p.arraySize; ++i) {
                            if(property.managedReferenceValue == p.GetArrayElementAtIndex(i).managedReferenceValue) {
                                trackIdx = i;
                                break;
                            }
                        }
                        spriteCaches = FrameAnimationAssetCreator.GetSprites();
                        resetFlag = true;
                        dragging = false;
                    }
                    break;
                default:
                    break;
            }
        }
        private void ResetSprites(SerializedProperty property) {
            if(!resetFlag || spriteCaches == null)
                return;

            if(delay < DELAY) {
                delay++;
                return;
            }
            var p = property.serializedObject.FindProperty("tracks");
            var frames = p.GetArrayElementAtIndex(trackIdx).FindPropertyRelative("frames");
            frames.ClearArray();
            frames.arraySize = spriteCaches.Length;
            for(int i = 0; i < spriteCaches.Length; ++i) {
                var element = frames.GetArrayElementAtIndex(i);
                var frameIndexProperty = element.FindPropertyRelative("frameIndex");
                frameIndexProperty.intValue = i;
                var frameSprite = element.FindPropertyRelative("frame");
                frameSprite.objectReferenceValue = spriteCaches[i];
            }
            property.serializedObject.ApplyModifiedProperties();

            resetFlag = false;
            spriteCaches = null;
            delay = 0;
        }
    }
}