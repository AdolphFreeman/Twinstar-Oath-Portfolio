Shader "Custom/Spine/ShadowSkew"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Shadow Color", Color) = (0,0,0,0.5)

        _HorizontalSkew ("Horizontal Skew", Float) = 0.5
        _VerticalSkew ("Vertical Skew", Float) = 0.2

        _Flatten ("Flatten", Float) = 0.3
        _Power ("Skew Power", Float) = 1.5

        _BottomY ("Bottom Y (Ground)", Float) = 0.0
        _HeightOffset ("Height Offset", Float) = 0.0

        _AlphaCutoff ("Alpha Cutoff", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 uv       : TEXCOORD0;
                fixed4 color    : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            float _HorizontalSkew;
            float _VerticalSkew;
            float _Flatten;
            float _Power;
            float _BottomY;
            float _HeightOffset;
            float _AlphaCutoff;

            v2f vert (appdata_t v)
            {
                v2f o;
                float4 pos = v.vertex;

                // ✅ Spine 專用：用 vertex.y - BottomY
                float h = max(0, pos.y - _BottomY);

                // 正規化（避免過大）
                float skewFactor = pow(h, _Power);

                // skew
                pos.x += skewFactor * _HorizontalSkew;
                pos.y += skewFactor * _VerticalSkew;

                // 壓扁
                pos.y *= _Flatten;

                // 微調
                pos.y += _HeightOffset;

                o.vertex = UnityObjectToClipPos(pos);
                o.uv = v.uv;
                o.color = v.color * _Color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                if (col.a < _AlphaCutoff)
                    discard;

                return col;
            }
            ENDCG
        }
    }
}