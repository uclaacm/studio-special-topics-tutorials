Shader "Aubrey/Demo" // Shader Name
{
    Properties // Editor Properties
    {
        // name   display name         type  default
        _MainTex   ("Aubrey Texture",  2D) = "white" {}
    }
    SubShader
    {
        // Generic options map
        // See Predefined Pass tags in the Built-in Render Pipeline
        Tags {
            "RenderType" = "Opaque"
            "LightMode" = "ForwardBase"
            "Arbitrary Key" = "Arbitrary Value"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDHLSL
        }
    }

    Fallback "Standard"
}
