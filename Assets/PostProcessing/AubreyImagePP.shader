Shader "Aubrey/AubeyPixelImageEffect"
{
    Properties
    {
        _PixelPercent ("Pixel Percent", Float) = 0.05
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _PixelPercent;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pixels = float2(i.uv.x * _ScreenParams.x, i.uv.y * _ScreenParams.y);
                float cutoffPixels = min(_ScreenParams.x, _ScreenParams.y) * _PixelPercent / 100;
                // float2 targetPixel = float2(floor(percent.x * 100) / 100, floor(percent.y * 100) / 100);
                float2 targetPixel = floor(pixels / cutoffPixels) * cutoffPixels;
                float2 uv = float2(targetPixel.x / _ScreenParams.x, targetPixel.y / _ScreenParams.y);

                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDHLSL
        }
    }
}
