using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(TemplatePPRenderer), PostProcessEvent.AfterStack, "Template/Template")]
public class TemplatePP : PostProcessEffectSettings
{
    // add parameters here
    // parameter types include: FloatParameter, IntegerParameter, ColorParameter, TextureParameter
    // eg.
    //public FloatParameter PixelPercent = new FloatParameter { value = 5f };
}

public sealed class TemplatePPRenderer : PostProcessEffectRenderer<TemplatePP>
{
    public override void Render(PostProcessRenderContext context)
    {
        const string shaderName = "Custom/PPTemplate"; // REPLACE WITH YOUR SHADER NAME
        var sheet = context.propertySheets.Get(Shader.Find(shaderName));

        // set the properties for your shader from the parameters
        // eg.
        //sheet.properties.SetFloat("_PixelPercent", settings.PixelPercent);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}