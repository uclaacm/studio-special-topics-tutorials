Shader "Aubrey/AubreyPixelPP"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _PixelPercent;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 pixels = float2(i.texcoord.x * _ScreenParams.x, i.texcoord.y * _ScreenParams.y);
            float cutoffPixels = min(_ScreenParams.x, _ScreenParams.y) * _PixelPercent / 100;
            // float2 targetPixel = float2(floor(percent.x * 100) / 100, floor(percent.y * 100) / 100);
            float2 targetPixel = floor(pixels / cutoffPixels) * cutoffPixels;
            float2 uv = float2(targetPixel.x / _ScreenParams.x, targetPixel.y / _ScreenParams.y);
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            return color;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
