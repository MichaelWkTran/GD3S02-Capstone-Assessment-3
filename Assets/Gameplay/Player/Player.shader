Shader "Unlit/Player"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" { }
        _Color1 ("Color1", Color) = (0.309804, 0.643137, 0.647059, 0.0)
        _Color2 ("Color2", Color) = (0.215686, 0.443137, 0.447059, 0.0)
        _Color3 ("Color3", Color) = (0.403922, 0.686275, 0.682353, 0.0)
        _Color4 ("Color4", Color) = (0.239216, 0.498039, 0.501961, 0.0)
        _FlashAlpha ("Flash Alpha", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Cull Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            fixed4 _Color4;
            float _FlashAlpha;

            void Unity_ReplaceColor_float(float3 _in, float3 _from, float3 _to, float _range, float _fuzziness, out float3 _out)
            {
                float colourDistance = distance(_from, _in);
                _out = lerp(_to, _in, saturate((colourDistance - _range) / max(_fuzziness, 1e-5f)));
            }

            v2f vert (appdata _v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(_v.vertex);
                o.uv = TRANSFORM_TEX(_v.uv, _MainTex);
    
                return o;
            }

            fixed4 frag (v2f _i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, _i.uv);
                
                Unity_ReplaceColor_float(col.rgb, float3(0.309804, 0.643137, 0.647059), _Color1, 0.01, 0.0f, col.rgb);
                Unity_ReplaceColor_float(col.rgb, float3(0.215686, 0.443137, 0.447059), _Color2, 0.01, 0.0f, col.rgb);
                Unity_ReplaceColor_float(col.rgb, float3(0.403922, 0.686275, 0.682353), _Color3, 0.01, 0.0f, col.rgb);
                Unity_ReplaceColor_float(col.rgb, float3(0.239216, 0.498039, 0.501961), _Color4, 0.01, 0.0f, col.rgb);
                col.rgb = lerp(col.rgb, float3(1.0f, 1.0f, 1.0f), _FlashAlpha);
                
                col.rgb *= col.a;
    
                return col;
            }
            ENDCG
        }
    }
}
