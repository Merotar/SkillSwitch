// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SkillSwitch/Sky"
{
	Properties
	{
		skyColor("Sky Color", Color) = (0.75, 0.75, 0.75, 0)
	}

		CGINCLUDE

#include "UnityCG.cginc"

	struct appdata
	{
		float4 position : POSITION;
		float3 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 position : SV_POSITION;
		float3 texcoord : TEXCOORD0;
	};

	half4 skyColor;

	v2f vert(appdata v)
	{
		v2f o;
		o.position = UnityObjectToClipPos(v.position);
		o.texcoord = v.texcoord;
		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		return skyColor;
	}

		ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Skybox" "Queue" = "Background" }
			Pass
		{
			ZWrite Off
			Cull Off
			Fog { Mode Off }
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}