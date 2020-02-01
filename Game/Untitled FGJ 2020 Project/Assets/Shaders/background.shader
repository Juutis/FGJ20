Shader "Unlit/background"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainColor("Main color", Color) = (0, 0, 0, 1)
		_SecondaryColor("Secondary color", Color) = (1, 1, 1, 1)
		_ScaleX("Scale X", Float) = 1.0
		_ScaleY("Scale Y", Float) = 1.0
		_ScrollSpeedX("ScrollSpeedX", Float) = 1.0
		_ScrollSpeedY("ScrollSpeedY", Float) = 1.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _MainColor;
				float4 _SecondaryColor;
				float _ScaleX;
				float _ScaleY;
				float _ScrollSpeedX;
				float _ScrollSpeedY;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					float2 uv = float2(i.uv.x*_ScaleX + _Time.y*_ScrollSpeedX, i.uv.y*_ScaleY + _Time.y*_ScrollSpeedY);
					//fixed4 col = tex2D(_MainTex, i.uv*_Scale + _Time.y*_ScrollSpeedX);
					fixed4 col = tex2D(_MainTex, uv);
					if (col.a < 0.1) discard;
					fixed4 retcol;
					if (col.r > 0.5) {
						retcol = _MainColor;
						retcol.a = col.a;
					}
					else {
						retcol = _SecondaryColor;
						retcol.a = col.a;
					}
					// apply fog
					//UNITY_APPLY_FOG(i.fogCoord, col);
					return retcol;
				}
				ENDCG
			}
		}
}
