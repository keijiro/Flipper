using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Flipper
{
    [System.Serializable]
    [PostProcess(typeof(RecolorRenderer), PostProcessEvent.BeforeStack, "Flipper/Recolor")]
    public sealed class Recolor : PostProcessEffectSettings
    {
        [Range(0, 1)] public FloatParameter blackLevel = new FloatParameter { value = 0.0f };
        [Range(0, 1)] public FloatParameter whiteLevel = new FloatParameter { value = 1.0f };
    }

    sealed class RecolorRenderer : PostProcessEffectRenderer<Recolor>
    {
        static class ShaderIDs
        {
            internal static readonly int Color = Shader.PropertyToID("_Color");
            internal static readonly int Levels = Shader.PropertyToID("_Levels");
        }

        public override void Render(PostProcessRenderContext context)
        {
            var cmd = context.command;
            cmd.BeginSample("Recolor");

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Flipper/Recolor"));
            sheet.properties.SetColor(ShaderIDs.Color, GlobalConfig.Instance.BaseColor.gamma);
            sheet.properties.SetVector(ShaderIDs.Levels, new Vector2(settings.blackLevel, settings.whiteLevel));
            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

            cmd.EndSample("Recolor");
        }
    }
}
