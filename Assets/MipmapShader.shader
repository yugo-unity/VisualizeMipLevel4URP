 Shader "Unlit/MipmapShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SamplerState sampler_BaseMap;

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
			float4 _BaseMap_TexelSize;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

			float calcMipLevel(in float2 texture_coordinate) // in texel units
			{
				float2  dx_vtc = ddx(texture_coordinate);
				float2  dy_vtc = ddy(texture_coordinate);
				float delta_max_sqr = max(dot(dx_vtc, dx_vtc), dot(dy_vtc, dy_vtc));
				return 0.5 * log2(delta_max_sqr);
			}

            half4 frag(Varyings IN) : SV_Target
            {
				float level = calcMipLevel(IN.uv * _BaseMap_TexelSize.zw);

				if (level <= 1)
					return float4(1, 0, 0, 1);
                else if (level <= 2)
                    return float4(0.5, 0.5, 0, 1);
				else if (level <= 3)
					return float4(0, 1, 0, 1);
                else if (level <= 4)
                    return float4(0, 0.5, 0.5, 1);
                else if (level <= 5)
                    return float4(0, 0, 1, 1);

				return float4(1, 1, 1, 1);
				
            }
            ENDHLSL
		}
    }
}
