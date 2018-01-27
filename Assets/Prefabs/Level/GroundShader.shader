Shader "SkillSwitch/Ground"
{
    Properties
    {
    	_BaseColor ("_BaseColor", Color) = (1,1,1,1)
        _EdgeColor ("_EdgeColor", Color) = (1,1,1,1)
        _GlowWidth("_GlowWidth", Range(0, 1)) = 0.1
        _LineWidth("_LineWidth", Range(0, 0.1)) = 0.1

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
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
         
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord * 2 - 1;
                return o;
            }

            fixed4 _BaseColor;
            fixed4 _EdgeColor;
            fixed _GlowWidth;
            fixed _LineWidth;


			half glowIntensity(half dist) {
				return _GlowWidth-dist;
			}

            fixed4 frag (v2f i) : SV_Target
            {
            	half2 dist = 1-abs(i.uv);
		        half2 pwidth = fwidth(dist);
				float alpha = max (saturate((_LineWidth - dist.x) / pwidth.x) , saturate((_LineWidth - dist.y) / pwidth.y) );

               	alpha += max(glowIntensity(dist.x), glowIntensity(dist.y));

                return lerp(_BaseColor, _EdgeColor, alpha);
            }
            ENDCG
        }
    }
}