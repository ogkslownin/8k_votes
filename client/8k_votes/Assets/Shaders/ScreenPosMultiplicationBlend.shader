Shader "Example/ScreenPosMultiplicationBlend" {
    Properties {
      _Fact("float",Range(0,3)) = 2
      _ColorMap ("Texture", 2D) = "white" { }
      _Screen ("Detail", 2D) = "gray" {}
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
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_ColorMap, IN.uv_ColorMap).rgb;
          float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          float fact = _Fact;
          screenUV *= float2(8,6);
          o.Albedo *= tex2D (_Screen, screenUV).rgb * _Fact;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }