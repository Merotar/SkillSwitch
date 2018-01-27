Shader "SkillSwitch/Skill"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Steepness("Steepness", Range(0, 0.15)) = 0.1

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        ZWrite Off
        Cull Off
 
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
                o.uv = v.texcoord - 0.5;
                return o;
            }
 
            fixed4 _Color;
            fixed _Steepness;
 
            fixed4 frag (v2f i) : SV_Target
            {
                float dist = length(i.uv);
                fixed alpha = exp(-1/2.*(dist/_Steepness)*(dist/_Steepness));
 
                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}