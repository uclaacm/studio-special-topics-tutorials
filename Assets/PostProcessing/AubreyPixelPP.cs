using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AubreyPixelPPRenderer), PostProcessEvent.AfterStack, "Aubrey/Pixel")]
public class AubreyPixelPP : PostProcessEffectSettings
{
    [Range(0, 8)]
    public FloatParameter PixelPercent = new FloatParameter { value = 5f };

}

public sealed class AubreyPixelPPRenderer : PostProcessEffectRenderer<AubreyPixelPP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Aubrey/AubreyPixelPP"));
        sheet.properties.SetFloat("_PixelPercent", settings.PixelPercent);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}