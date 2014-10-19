Shader "Example/ScreenPosAdditionBlend" {
    Properties {
      _Fact("fact",Range(0,3)) = 2
      _ColorMap ("Texture", 2D) = "white" { }
      _Screen ("Detail", 2D) = "gray" {}
      _Glow ("glow", Range(0,5)) = 0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_ColorMap;
          float4 screenPos;
      };
      sampler2D _ColorMap;
      sampler2D _Screen;
      float _Fact;
      float _Glow;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_ColorMap, IN.uv_ColorMap).rgb;
          float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          float fact = _Fact;
          float glow = _Glow;
          screenUV *= float2(8,6);
          o.Albedo += tex2D (_Screen, screenUV).rgb * _Fact;
          o.Albedo += glow;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }