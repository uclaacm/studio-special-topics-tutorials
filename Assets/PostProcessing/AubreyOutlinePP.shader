// Shader "Aubrey/AubreyOutlinePP"
// {
//     SubShader
//     {
//         Tags { "RenderType"="Opaque" }
//         LOD 100
//         Cull Off
//         ZWrite Off
//         ZTest Always

//         Pass
//         {
//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag

//             #include "UnityCG.cginc"

//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             struct v2f
//             {
//                 float4 vertex : SV_POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             Texture2D _MainTex;
//             SamplerState sampler_MainTex;

//             Texture2D _CameraDepthTexture;
//             SamplerState sampler_CameraDepthTexture;

//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.vertex = UnityObjectToClipPos(v.vertex);
//                 o.uv = v.uv;
//                 return o;
//             }

//             fixed4 frag (v2f i) : SV_Target
//             {
//                 fixed4 col = 1 - _MainTex.Sample(sampler_MainTex, i.uv);
//                 float depth = Linear01Depth(_CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.uv).r);
//                 col = fixed4(depth, depth, depth, 1);
//                 return col;
//             }
//             ENDHLSL
//         }
//     }
// }


Shader "Aubrey/AubreyOutlinePP"
{
    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        // #include "UnityCG.cginc"

        Texture2D _MainTex;
        SamplerState sampler_MainTex;

        Texture2D _CameraDepthTexture;
        SamplerState sampler_CameraDepthTexture;

        Texture2D _CameraDepthNormalsTexture;
        SamplerState sampler_CameraDepthNormalsTexture;

        float4 _OutlineColor;
        float _DepthBias;
        float _NPixels;

        inline float DecodeFloatRG( float2 enc )
        {
            float2 kDecodeDot = float2(1.0, 1/255.0);
            return dot( enc, kDecodeDot );
        }

        inline void DecodeDepthNormal( float4 enc, out float depth, out float3 normal )
        {
            depth = DecodeFloatRG (enc.zw);
            normal = DecodeViewNormalStereo (enc);
        }

        float4 SamplePixel(Texture2D tex, SamplerState samp, float2 pixel){
            float2 uv = float2(pixel.x / _ScreenParams.x, pixel.y / _ScreenParams.y);
            return tex.Sample(samp, uv);
        }

        float Sobel(float c, float l, float r, float u, float d){
            return abs(l - c)
                + abs(r - c)
                + abs(u - c)
                + abs(d - c);
        }

        float3 Sobel3(float3 c, float3 l, float3 r, float3 u, float3 d){
            return abs(l - c)
                + abs(r - c)
                + abs(u - c)
                + abs(d - c);
        }

        float SobelSample(Texture2D t, SamplerState s, float2 uv)
        {
            float2 right  = float2(_NPixels, 0);
            float2 down   = float2(0, _NPixels);

            float4 c = SamplePixel(t, s, uv);
            float4 l = SamplePixel(t, s, uv - right);
            float4 r = SamplePixel(t, s, uv + right);
            float4 u = SamplePixel(t, s, uv - down);
            float4 d = SamplePixel(t, s, uv + down);

            float cd, ld, rd, ud, dd;
            float3 cn, ln, rn, un, dn;

            DecodeDepthNormal(c, cd, cn);
            DecodeDepthNormal(l, ld, ln);
            DecodeDepthNormal(r, rd, rn);
            DecodeDepthNormal(u, ud, un);
            DecodeDepthNormal(d, dd, dn);

            float3 sobelNormal = Sobel3(cn, ln, rn, un, dn);

            return Sobel(cd, ld, rd, ud, dd) * _DepthBias
                + dot(sobelNormal, float3(1, 1, 1)) * (1 - _DepthBias);
        }

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 pixel = float2(i.texcoord.x * _ScreenParams.x, i.texcoord.y * _ScreenParams.y);
            float4 color = SamplePixel(_MainTex, sampler_MainTex, pixel);

            float4 value = SamplePixel(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, pixel);

            float depth;
            float3 normal;

            DecodeDepthNormal(value, depth, normal);

            float sobel = SobelSample(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, pixel);

            return lerp(color, _OutlineColor, step(0.5, sobel));
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
