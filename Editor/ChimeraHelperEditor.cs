using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRC.Core;
using VRC.SDKBase;

namespace Narazaka.VRChat.Chimera.Editor
{
    [CustomEditor(typeof(ChimeraHelper))]
    public class ChimeraHelperEditor : UnityEditor.Editor
    {
        SerializedProperty replaceFaceMeshFirst;
        SerializedProperty destroyObjectPaths;

        SerializedProperty viewPosition;
        SerializedProperty lipSync;
        SerializedProperty lipSyncJawBone;
        SerializedProperty lipSyncJawClosed;
        SerializedProperty lipSyncJawOpen;
        SerializedProperty visemeSkinnedMesh;
        SerializedProperty mouthOpenBlendShapeName;
        SerializedProperty visemeBlendShapes;
        SerializedProperty enableEyeLook;
        SerializedProperty customEyeLookSettings;

        void OnEnable()
        {
            replaceFaceMeshFirst = serializedObject.FindProperty(nameof(ChimeraHelper.ReplaceFaceMeshFirst));
            destroyObjectPaths = serializedObject.FindProperty(nameof(ChimeraHelper.DestroyObjectPaths));

            viewPosition = serializedObject.FindProperty(nameof(ChimeraHelper.ViewPosition));
            lipSync = serializedObject.FindProperty(nameof(ChimeraHelper.lipSync));
            lipSyncJawBone = serializedObject.FindProperty(nameof(ChimeraHelper.lipSyncJawBone));
            lipSyncJawClosed = serializedObject.FindProperty(nameof(ChimeraHelper.lipSyncJawClosed));
            lipSyncJawOpen = serializedObject.FindProperty(nameof(ChimeraHelper.lipSyncJawOpen));
            visemeSkinnedMesh = serializedObject.FindProperty(nameof(ChimeraHelper.VisemeSkinnedMesh));
            mouthOpenBlendShapeName = serializedObject.FindProperty(nameof(ChimeraHelper.MouthOpenBlendShapeName));
            visemeBlendShapes = serializedObject.FindProperty(nameof(ChimeraHelper.VisemeBlendShapes));
            enableEyeLook = serializedObject.FindProperty(nameof(ChimeraHelper.enableEyeLook));
            customEyeLookSettings = serializedObject.FindProperty(nameof(ChimeraHelper.customEyeLookSettings));
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Copy from VRC Avatar Descriptor"))
            {
                CopyFromVRCAvatarDescriptor();
            }
            if (GUILayout.Button("Delete VRC Avatar Descriptor"))
            {
                DeleteVRCAvatarDescriptor();
            }
            serializedObject.UpdateIfRequiredOrScript();
            EditorGUILayout.PropertyField(replaceFaceMeshFirst);
            EditorGUILayout.PropertyField(destroyObjectPaths, true);
            EditorGUILayout.Space();
            DrawVRCAvatarDescriptorLikeInspector();
            serializedObject.ApplyModifiedProperties();
        }

        bool showView = true;
        bool showLipSync = true;
        bool showEyeLook = true;

        void DrawVRCAvatarDescriptorLikeInspector()
        {
            if (showView = EditorGUILayout.Foldout(showView, "View"))
            {
                EditorGUILayout.PropertyField(viewPosition);
            }
            if (showLipSync = EditorGUILayout.Foldout(showLipSync, "LipSync"))
            {
                EditorGUILayout.PropertyField(lipSync, new GUIContent("Mode"));
                switch (lipSync.enumValueIndex)
                {
                    case 0: // Default
                        {
                            break;
                        }
                    case 1: // JawFlapBone
                        {
                            EditorGUILayout.PropertyField(lipSyncJawBone, new GUIContent("Jaw Bone"));
                            EditorGUILayout.LabelField("Rotation States");
                            using (new EditorGUI.DisabledScope(lipSyncJawBone.objectReferenceValue == null))
                            {
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    EditorGUILayout.PropertyField(lipSyncJawClosed, new GUIContent("Closed"));
                                    EditorGUILayout.PropertyField(lipSyncJawOpen, new GUIContent("Open"));
                                }
                            }
                            break;
                        }
                    case 2: // JawFlapBlendShape
                        {
                            EditorGUILayout.PropertyField(visemeSkinnedMesh, new GUIContent("Face Mesh"));
                            EditorGUILayout.PropertyField(mouthOpenBlendShapeName, new GUIContent("Jaw Flap Blend Shape"));
                            break;
                        }
                    case 3: // VisemeBlendShape
                        {
                            EditorGUILayout.PropertyField(visemeSkinnedMesh, new GUIContent("Face Mesh"));
                            for (var i = 0; i < (int)VRC_AvatarDescriptor.Viseme.Count; i++)
                            {
                                var visemeName = ((VRC_AvatarDescriptor.Viseme)i).ToString();
                                if (visemeBlendShapes.arraySize <= i)
                                {
                                    visemeBlendShapes.arraySize = i + 1;
                                }
                                var element = visemeBlendShapes.GetArrayElementAtIndex(i);
                                element.stringValue = EditorGUILayout.TextField("Viseme: " + visemeName, element.stringValue);
                            }
                            break;
                        }
                    case 4: // VisemeParameterOnly
                        {
                            break;
                        }
                }
            }
            if (showEyeLook = EditorGUILayout.Foldout(showEyeLook, "Eye Look"))
            {
                EditorGUILayout.PropertyField(enableEyeLook, new GUIContent("Enable Eye Look"));
                using (new EditorGUI.DisabledScope(!enableEyeLook.boolValue))
                {
                    EditorGUILayout.PropertyField(customEyeLookSettings, new GUIContent("Custom Eye Look Settings"));
                }
            }
        }

        void CopyFromVRCAvatarDescriptor()
        {
            var helper = target as ChimeraHelper;
            var descriptor = helper.GetComponent<VRC.SDK3.Avatars.Components.VRCAvatarDescriptor>();
            if (descriptor == null)
            {
                Debug.LogError("No VRC_AvatarDescriptor found on this GameObject.");
                return;
            }
            Undo.RecordObject(helper, "Copy from VRC Avatar Descriptor");
            helper.CopyFromVRCAvatarDescriptor(descriptor);
        }

        void DeleteVRCAvatarDescriptor()
        {
            var helper = target as ChimeraHelper;
            var descriptor = helper.GetComponent<VRC.SDK3.Avatars.Components.VRCAvatarDescriptor>();
            if (descriptor == null)
            {
                Debug.LogError("No VRC_AvatarDescriptor found on this GameObject.");
                return;
            }
            Undo.DestroyObjectImmediate(descriptor);

            var pipeline = helper.GetComponent<PipelineManager>();
            if (pipeline != null)
            {
                Undo.DestroyObjectImmediate(pipeline);
            }
        }
    }
}
