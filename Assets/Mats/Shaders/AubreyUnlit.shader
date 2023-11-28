Shader "Aubrey/AubreyUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Ambient ("Ambient", Float) = 0.2
        _NCels ("Ambient", Integer) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Ambient;
            int _NCels;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = float4(UnityObjectToWorldDir(v.normal.xyz), v.normal.w);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float intensity = dot(_WorldSpaceLightPos0, i.normal) * (1.0f - _Ambient) + _Ambient;
                intensity = ceil(intensity * _NCels) / _NCels;
                col *= intensity;
                return col;
            }
            ENDHLSL
        }
    }

    Fallback "Standard"
}
