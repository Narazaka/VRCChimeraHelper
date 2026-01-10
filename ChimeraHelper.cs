using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Narazaka.VRChat.Chimera
{
    public class ChimeraHelper : MonoBehaviour, IEditorOnly
    {
        public Vector3 ViewPosition;

        public VRC_AvatarDescriptor.LipSyncStyle lipSync;

        public Transform lipSyncJawBone;

        public Quaternion lipSyncJawClosed = Quaternion.identity;

        public Quaternion lipSyncJawOpen = Quaternion.identity;

        public SkinnedMeshRenderer VisemeSkinnedMesh;

        public string MouthOpenBlendShapeName = "Facial_Blends.Jaw_Down";

        public string[] VisemeBlendShapes;

        public bool enableEyeLook;

        public VRCAvatarDescriptor.CustomEyeLookSettings customEyeLookSettings;

        public bool ReplaceFaceMeshFirst = true;

        public void CopyFromVRCAvatarDescriptor(VRCAvatarDescriptor descriptor)
        {
            ViewPosition = descriptor.ViewPosition;
            lipSync = descriptor.lipSync;
            lipSyncJawBone = descriptor.lipSyncJawBone;
            lipSyncJawClosed = descriptor.lipSyncJawClosed;
            lipSyncJawOpen = descriptor.lipSyncJawOpen;
            VisemeSkinnedMesh = descriptor.VisemeSkinnedMesh;
            MouthOpenBlendShapeName = descriptor.MouthOpenBlendShapeName;
            VisemeBlendShapes = descriptor.VisemeBlendShapes;
            enableEyeLook = descriptor.enableEyeLook;
            customEyeLookSettings = descriptor.customEyeLookSettings;
        }

        public void ApplyToVRCAvatarDescriptor(VRCAvatarDescriptor descriptor, bool fixViewPositionOffset = true)
        {
            descriptor.ViewPosition = ViewPosition + (fixViewPositionOffset ? transform.localPosition : Vector3.zero);
            descriptor.lipSync = lipSync;
            descriptor.lipSyncJawBone = lipSyncJawBone;
            descriptor.lipSyncJawClosed = lipSyncJawClosed;
            descriptor.lipSyncJawOpen = lipSyncJawOpen;
            descriptor.VisemeSkinnedMesh = VisemeSkinnedMesh;
            descriptor.MouthOpenBlendShapeName = MouthOpenBlendShapeName;
            descriptor.VisemeBlendShapes = VisemeBlendShapes;
            descriptor.enableEyeLook = enableEyeLook;
            descriptor.customEyeLookSettings = customEyeLookSettings;
        }
    }
}
