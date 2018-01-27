Shader "SkillSwitch/Ground"
{
    Properties
    {
    	_BaseColor ("_BaseColor", Color) = (1,1,1,1)
        _EdgeColor ("_EdgeColor", Color) = (1,1,1,1)
        _GlowWidth("_GlowWidth", Range(0, 1)) = 0.1
        _LineWidth("_LineWidth", Range(0, 0.1)) = 0.1
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125


    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ColorMask RGB
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            fixed4 _BaseColor;
            fixed4 _EdgeColor;
            fixed _GlowWidth;
            fixed _LineWidth;
            float _Shininess;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord * 2 - 1;

	            float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
	            float dotProduct =( 1 - dot(v.normal, viewDir))*_Shininess;
	            o.color = smoothstep(0.3, 1.0, dotProduct);
	            o.color *= _EdgeColor;

                return o;
            }


			half glowIntensity(half dist) {
				return _GlowWidth-dist;
			}

            fixed4 frag (v2f i) : SV_Target
            {
            	half2 dist = 1-abs(i.uv);
		        half2 pwidth = fwidth(dist);
				float alpha = max (saturate((_LineWidth - dist.x) / pwidth.x) , saturate((_LineWidth - dist.y) / pwidth.y) );

               	alpha += max(glowIntensity(dist.x), glowIntensity(dist.y));

                return lerp(i.color, _EdgeColor, alpha);
            }
            ENDCG
        }
    }
}