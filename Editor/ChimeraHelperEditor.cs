using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRC.Core;

namespace Narazaka.VRChat.Chimera.Editor
{
    [CustomEditor(typeof(ChimeraHelper))]
    public class ChimeraHelperEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Copy from VRC Avatar Descriptor"))
            {
                CopyFromVRCAvatarDescriptor();
            }
            if (GUILayout.Button("Delete VRC Avatar Descriptor"))
            {
                DeleteVRCAvatarDescriptor();
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
