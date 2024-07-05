Shader "Unlit/CrossSection"
{
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_Metallic("Metalness", Range(0, 1)) = 0
		[HDR] _Emission("Emission", color) = (0,0,0)

		[HDR]_CutoffColor("Cutoff Color", Color) = (1,0,0,0)
	}
		SubShader{
			Tags {"RenderType" = "Opaque" "Queue" = "Geometry"}
			Cull Off

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			fixed4 _Color;
			float4 _Plane;
			float4 _CutoffColor;

			half _Smoothness;
			half _Metallic;
			half3 _Emission;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
				float facing : VFACE;
			};

			// the surface shader function which sets parameters the lighting function then uses
			void surf(Input i, inout SurfaceOutputStandard o) {
				// calculate signed distance to plane
				float distance = dot(i.worldPos, _Plane.xyz);
				// add the plane distance to origin so that the plane position matters
				distance = distance + _Plane.w;
				// just render the part below the plane
				clip(-distance);

				float facing = i.facing * 0.5 + 0.5;

				//normal color stuff
				fixed4 col = tex2D(_MainTex, i.uv_MainTex);
				/*col *= _Color;
				o.Albedo = col.rgb * facing;
				o.Metallic = _Metallic * facing;
				o.Smoothness = _Smoothness * facing;
				*/

				o.Albedo = (0, 0, 0);
				o.Metallic = (0, 0, 0);
				o.Smoothness = (0, 0, 0);

				o.Emission = lerp(_CutoffColor, _Emission, facing);
			}
			ENDCG
		}
			FallBack "Standard"
}