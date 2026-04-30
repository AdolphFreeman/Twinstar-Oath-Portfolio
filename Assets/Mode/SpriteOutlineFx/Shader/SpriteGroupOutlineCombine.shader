Shader "Hidden/SpriteGroupOutlineCombine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Outline Color", Color) = (1,0,0,1)
        _Distance ("Distance", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float _Distance;

            float4 frag(v2f_img i) : SV_Target
            {
                float d = _Distance;
                float2 uv = i.uv;

                // Sample surroundings
                half a1 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2(-1, -1)).a;
                half a2 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2( 0, -1)).a;
                half a3 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2(+1, -1)).a;
                half a4 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2(-1,  0)).a;
                
                half center = tex2D(_MainTex, uv).a;
                
                half a6 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2(+1,  0)).a;
                half a7 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2(-1, +1)).a;
                half a8 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2( 0, +1)).a;
                half a9 = tex2D(_MainTex, uv + d * _MainTex_TexelSize.xy * float2(+1, +1)).a;

                // Sobel edge detection
                float gx = - a1 - a2*2 - a3 + a7 + a8*2 + a9;
                float gy = - a1 - a4*2 - a7 + a3 + a6*2 + a9;

                float w = sqrt(gx * gx + gy * gy) / 4.0;
                
                // Boost weight for solid edge
                float outlineAlpha = saturate(w * 5.0); 
                
                // Do not draw outline where the sprite mask already exists
                outlineAlpha *= saturate(1.0 - center);
                
                // Return outline color with edge alpha
                return float4(_Color.rgb, _Color.a * outlineAlpha);
            }
            ENDCG
        }
    }
}
