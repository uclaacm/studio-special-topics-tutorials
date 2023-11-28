Shader "Aubrey/AubreySlice"
{
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Amount", Float) = 0.1
      _Distance ("Distance", Float) = 0.1
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Off
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float3 worldPos;
      };
      sampler2D _MainTex;
      float _Amount;
      float _Distance;

      void surf (Input IN, inout SurfaceOutput o) {
          clip (frac((IN.worldPos.y+IN.worldPos.z*_Amount * 0.1) * 5 * _Distance) - 0.5 * _Amount);
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }