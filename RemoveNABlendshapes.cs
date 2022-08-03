using NeosModLoader;
using HarmonyLib;
using FrooxEngine;
using FrooxEngine.UIX;

namespace RemoveNABlendshapes
{
    public class RemoveNABlendshapes : NeosMod
    {
        public override string Name => "RemoveNABlendshapes";
        public override string Author => "hantabaru1014";
        public override string Version => "1.0.0";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony($"net.{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(SkinnedMeshRenderer), nameof(SkinnedMeshRenderer.BuildInspectorUI))]
        class SkinnedMeshRenderer_BuildInspectorUI_Patch
        {
            static void Postfix(SkinnedMeshRenderer __instance, UIBuilder ui)
            {
                var removeBtn = ui.Button("Remove N/A Blendshapes");
                removeBtn.LocalPressed += (IButton btn, ButtonEventData data) => RemoveNAshapes(__instance);
            }

            private static void RemoveNAshapes(SkinnedMeshRenderer instance)
            {
                Msg($"Blendshapes Count: {instance.BlendShapeCount}, List Count: {instance.BlendShapeWeights.Count}");
                for (int i=instance.BlendShapeWeights.Count-1; i>=0; i--)
                {
                    if (instance.BlendShapeName(i) is null) instance.BlendShapeWeights.RemoveAt(i);
                    else break;
                }
            }
        }
    }
}
