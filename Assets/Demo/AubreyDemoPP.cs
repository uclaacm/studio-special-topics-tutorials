using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AubreyDemoPPRenderer), PostProcessEvent.AfterStack, "Aubrey/DemoPP")]
public class AubreyDemoPP : PostProcessEffectSettings
{
}

public sealed class AubreyDemoPPRenderer : PostProcessEffectRenderer<AubreyDemoPP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Aubrey/DemoPP"));        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}