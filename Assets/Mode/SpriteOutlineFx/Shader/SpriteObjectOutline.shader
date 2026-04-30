Shader "Sprites/Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        
        // --- Outline Properties ---
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineDistance ("Outline Distance", Range(0.0, 0.1)) = 0.005
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            
            fixed4 _OutlineColor;
            float _OutlineDistance;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.texcoord);
                
                // If it's transparent, we check surroundings for outline
                if (col.a < 0.1)
                {
                    float d = _OutlineDistance;
                    
                    // Sample 4 directions (cross)
                    fixed a1 = tex2D(_MainTex, IN.texcoord + float2(d, 0)).a;
                    fixed a2 = tex2D(_MainTex, IN.texcoord + float2(-d, 0)).a;
                    fixed a3 = tex2D(_MainTex, IN.texcoord + float2(0, d)).a;
                    fixed a4 = tex2D(_MainTex, IN.texcoord + float2(0, -d)).a;
                    
                    // Sample diagonals
                    fixed a5 = tex2D(_MainTex, IN.texcoord + float2(d, d)).a;
                    fixed a6 = tex2D(_MainTex, IN.texcoord + float2(-d, -d)).a;
                    fixed a7 = tex2D(_MainTex, IN.texcoord + float2(-d, d)).a;
                    fixed a8 = tex2D(_MainTex, IN.texcoord + float2(d, -d)).a;

                    float alphaSum = a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8;
                    
                    if (alphaSum > 0.0)
                    {
                        // Draw Outline
                        fixed4 outline = _OutlineColor;
                        outline.a = min(alphaSum, 1.0) * _OutlineColor.a * IN.color.a;
                        outline.rgb *= outline.a; // Premultiplied alpha
                        return outline;
                    }
                }

                col.rgb *= col.a; // Premultiplied alpha
                return col * IN.color;
            }
            ENDCG
        }
    }
}
