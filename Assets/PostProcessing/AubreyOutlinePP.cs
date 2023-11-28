using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AubreyOutlinePPRenderer), PostProcessEvent.AfterStack, "Aubrey/Outline")]
public class AubreyOutlinePP : PostProcessEffectSettings
{
    public ColorParameter OutlineColor = new() { value = Color.black };

    [Range(0, 1)]
    public FloatParameter DepthBias = new() { value = 0.5f };

    [UnityEngine.Rendering.PostProcessing.Min(1)]
    public FloatParameter NPixels = new() { value = 1 };
}

public sealed class AubreyOutlinePPRenderer : PostProcessEffectRenderer<AubreyOutlinePP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Aubrey/AubreyOutlinePP"));

        sheet.properties.SetFloat("_DepthBias", settings.DepthBias);
        sheet.properties.SetColor("_OutlineColor", settings.OutlineColor);
        sheet.properties.SetFloat("_NPixels", settings.NPixels);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}