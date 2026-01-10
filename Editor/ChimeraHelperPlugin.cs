using nadena.dev.ndmf;
using nadena.dev.ndmf.vrchat;
using UnityEngine;

[assembly: ExportsPlugin(typeof(Narazaka.VRChat.Chimera.Editor.ChimeraHelperPlugin))]

namespace Narazaka.VRChat.Chimera.Editor
{
    public class ChimeraHelperPlugin : Plugin<ChimeraHelperPlugin>
    {
        public override string DisplayName => nameof(ChimeraHelperPlugin);
        public override string QualifiedName => "net.narazaka.vrchat.chimera";

        protected override void Configure()
        {
            InPhase(BuildPhase.Resolving).BeforePlugin("nadena.dev.modular-avatar").BeforePlugin("aoyon.facetune").Run(DisplayName, SetupChimeraHelper);
        }

        void SetupChimeraHelper(BuildContext ctx)
        {
            var helpers = ctx.AvatarRootObject.GetComponentsInChildren<ChimeraHelper>();
            if (helpers == null || helpers.Length == 0)
            {
                return;
            }
            if (helpers.Length > 1)
            {
                throw new System.InvalidOperationException("Multiple ChimeraHelper components found. Only the first one will be set up.");
            }
            var helper = helpers[0];

            var descriptor = ctx.VRChatAvatarDescriptor();
            if (descriptor != null)
            {
                helper.ApplyToVRCAvatarDescriptor(descriptor);
                if (helper.ReplaceFaceMeshFirst && helper.VisemeSkinnedMesh != null && helper.VisemeSkinnedMesh.sharedMesh != null)
                {
                    var faceName = helper.VisemeSkinnedMesh.name;
                    var parentFace = ctx.AvatarRootTransform.Find(faceName);
                    if (parentFace != null)
                    {
                        var parentFaceRenderer = parentFace.GetComponent<SkinnedMeshRenderer>();
                        if (parentFaceRenderer != null)
                        {
                            parentFaceRenderer.sharedMesh = helper.VisemeSkinnedMesh.sharedMesh;
                        }
                    }
                }
            }
            Object.DestroyImmediate(helper);
        }
    }
}
