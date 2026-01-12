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
                if (helper.DestroyObjectPaths != null)
                {
                    foreach (var path in helper.DestroyObjectPaths)
                    {
                        var obj = ctx.AvatarRootTransform.Find(path);
                        if (obj != null)
                        {
                            Object.DestroyImmediate(obj.gameObject);
                        }
                    }
                }
                
                var motchiriType = MotchiriShaderMaType();
                if (motchiriType != null && helper.WillBeDestroyed_MotchiriMask != null)
                {
                    var motchiriObj = ctx.AvatarRootTransform.Find("motchiri_shader_Setup");
                    if (motchiriObj != null)
                    {
                        var motchiri = motchiriObj.GetComponent(motchiriType);
                        if (motchiri != null)
                        {
                            var meshMaskField = MeshMaskField();
                            var value = meshMaskField.GetValue(motchiri) as Texture2D[];
                            if (value != null && value.Length >= 2)
                            {
                                value[1] = helper.WillBeDestroyed_MotchiriMask;
                            }
                        }
                    }
                }
            }
            Object.DestroyImmediate(helper);
        }

        static System.Type motchiriShaderMaType = null;
        static bool motchiriShaderMaTypeResolved = false;
        internal static System.Type MotchiriShaderMaType()
        {
            if (motchiriShaderMaTypeResolved)
            {
                return motchiriShaderMaType;
            }
            motchiriShaderMaTypeResolved = true;
            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms)
            {
                var type = asm.GetType("wataameya.motchiri_shader.ndmf.motchiri_shader_MA");
                if (type != null)
                {
                    motchiriShaderMaType = type;
                    break;
                }
            }
            return motchiriShaderMaType;
        }

        static System.Reflection.FieldInfo MeshMaskField()
        {
            var type = MotchiriShaderMaType();
            if (type == null) return null;
            return type.GetField("_meshMask", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        }
    }
}
