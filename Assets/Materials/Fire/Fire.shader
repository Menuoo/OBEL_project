Shader "Fire"
{
    Properties
    {
       [Enum(UnityEngine.Rendering.BlendMode)]
       _SrcFactor("Src Factor", Float) = 5
       [Enum(UnityEngine.Rendering.BlendMode)]
       _DstFactor("Dst Factor", Float) = 10
       [Enum(UnityEngine.Rendering.BlendOp)]
       _BlendOp("Operation", Float) = 0

       _MainTex("Texture", 2D) = "white" {}
       _FadeTex("Texture", 2D) = "white" {}
       _MaskActive("Edge Mask?", Range(0, 1)) = 0

       _Offset("Tex Offset", float) = 0
       _Speed("Speed", float) = 1

       _Tint("Color1", color) = (1, 1, 1, 1)
       _Tint2("Color2", color) = (1, 1, 1, 1)
       _Tint3("Color3", color) = (1, 1, 1, 1)

       _MyTime ("My Time", float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_BlendOp]

        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float2 adds : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _FadeTex;
            float4 _FadeTex_ST;
            float _MaskActive;

            float _Speed;
            float _Offset;
            float4 _Tint;
            float4 _Tint2;
            float4 _Tint3;

            float _MyTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.z = sqrt(_Offset);
                o.uv.w = sqrt(o.uv.z);

                float timeDiff = _Time.y - _MyTime;
                o.adds.x = 1;//sin((timeDiff) * 1.2 + 0.5);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv1 = i.uv.xy;
                //uv1.x = uv1.x * 2 - 0.5 - _Time.y * 0.5;
                uv1.y = uv1.y - _Time.y * _Speed;

                fixed4 col = tex2D(_MainTex, uv1);

                float fade = 1 - tex2D(_FadeTex, i.uv.xy).r;

                fixed4 col2 = col;
                fixed4 col3 = col2;
                fixed4 mask = col3;
                fixed4 mask2 = mask;
                float edge = pow(abs(i.uv.x - 0.5), 1.5);

                float selection = 1 - i.uv.y;

                col.rgb = max(col.rgb + (selection - _Offset - edge), 0);
                col2.rgb = max(col2.rgb + (selection - i.uv.z - edge), 0);
                col3.rgb = max(col3.rgb + (selection - i.uv.w - edge), 0);

                col.rgba = (1 - step(col.r, 0.2));
                col2.rgba = (1 - step(col2.r, 0.2));
                col3.rgba = (1 - step(col3.r, 0.2));
                
                col.rgb = col.rgb * col.a * _Tint;
                col.rgb = lerp(col.rgb, col2.rgb * _Tint2, col2.a);
                col.rgb = lerp(col.rgb, col3.rgb * _Tint3, col3.a);

                if (_MaskActive > 0)
                {
                mask.rgb = max(mask.rgb + (selection * length(i.uv.xy * 2 - 1) - i.uv.w + edge), 0);
                mask2.rgb = max(mask2.rgb + ((1 - selection) * length(i.uv.xy * 2 - 1) - i.uv.w + edge), 0);

                mask.rgba = (1 - step(mask.r, _MaskActive));
                mask2.rgba = (1 - step(mask2.r, _MaskActive));

                col.a = col.a * (1 - mask.a) * (1 - mask2.a);
                }

                col.a = col.a * step(fade, i.adds.x);

                return fixed4(col);
            }
            ENDCG
        }
    }
}
