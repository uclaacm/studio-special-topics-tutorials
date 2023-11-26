Shader "Aubrey/CoolShader" // Shader Name
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
            "Arbitrary Key" = "Arbitrary Value"
        }

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                // Vertex shader logic
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                // Fragment shader logic
                return col;
            }
            ENDHLSL
        }
    }
}
