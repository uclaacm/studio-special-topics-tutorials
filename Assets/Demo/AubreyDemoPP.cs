using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AubreyDemoPPRenderer), PostProcessEvent.AfterStack, "Aubrey/DemoPP")]
public class AubreyDemoPP : PostProcessEffectSettings
{
    [Range(1, 100)]
    public IntParameter PixelSize = new IntParameter { value = 10 };
}

public sealed class AubreyDemoPPRenderer : PostProcessEffectRenderer<AubreyDemoPP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Aubrey/DemoPP"));

        sheet.properties.SetFloat("_PixelSize", settings.PixelSize);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}