using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BNG;
using CloudFine.ThrowLab.Oculus;
using UnityEditor;
using UnityEngine;

namespace MobaVR
{
    [CustomEditor(typeof(NetworkGrabbable), true)]
    [CanEditMultipleObjects]
    public class NetworkGrabbableEditor : GrabbableEditor
    {
        private SerializedProperty _isSyncHold;
        private SerializedProperty _maxDeltaPosition;
        private SerializedProperty _isSyncPosition;
        private SerializedProperty _isSyncRotation;
        private SerializedProperty _isSyncScale;

        protected override void OnEnable()
        {
            base.OnEnable();
            _isSyncHold = serializedObject.FindProperty("_isSyncHold");
            _maxDeltaPosition = serializedObject.FindProperty("_maxDeltaPosition");
            _isSyncPosition = serializedObject.FindProperty("_isSyncPosition");
            _isSyncRotation = serializedObject.FindProperty("_isSyncRotation");
            _isSyncScale = serializedObject.FindProperty("_isSyncScale");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            grabbable = (Grabbable)target;

            // Don't use Custom Editor
            if (UseCustomEditor == false || grabbable.UseCustomInspector == false)
            {
                base.OnInspectorGUI();
                return;
            }

            EditorGUILayout.PropertyField(_isSyncHold);
            EditorGUILayout.PropertyField(_maxDeltaPosition);
            EditorGUILayout.PropertyField(_isSyncPosition);
            EditorGUILayout.PropertyField(_isSyncRotation);
            EditorGUILayout.PropertyField(_isSyncScale);

            serializedObject.ApplyModifiedProperties();
        }
    }
}