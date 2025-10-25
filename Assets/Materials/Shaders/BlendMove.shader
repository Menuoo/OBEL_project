Shader "Unlit/BlendMove"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _SecTex ("Secondary Texture", 2D) = "white" {}
        _Speed ("Tex Speed", Float) = 0.0
        _Range ("Colour Range", Float) = 0.0

        _Timer ("Time Offset", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            sampler2D _MainTex, _SecTex;
            float4 _MainTex_ST, _SecTex_ST;
            float _Speed, _Range, _Timer;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _SecTex);
                return o;
            }

            float FindRange(float4 clr, float time)
            {
                float rangeEnd = time + _Range;

                float localAlpha = clr.r;
                if (_Range != 0)
                {
                    localAlpha = (clr.r >= time ? (clr.r - time) : (clr.r + 1.0 - time)) / _Range;
                    //localAlpha = 1.0;
                }

                if (rangeEnd > 1) 
                {
                    rangeEnd -= 1.0;
                    return (clr.r > time || clr.r < rangeEnd) ? localAlpha : 0.0;
                }
                else 
                {
                    return (clr.r > time && clr.r < rangeEnd) ? localAlpha : 0.0;
                }
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float curTime = frac(_Time.y * _Speed);

                fixed4 mainCol = tex2D (_MainTex, i.uv); // + float2(1, 0) * curTime);// * _Color;
                fixed4 secCol = tex2D (_MainTex, i.uv2); // + float2(1, 0) * frac(curTime + 0.5));// * _Color;


                mainCol.r = FindRange(mainCol, curTime);
                secCol.r = FindRange(secCol, frac(curTime + _Timer));


                curTime = abs(curTime - 0.5) * 2.0;

                fixed4 finalCol = mainCol * (curTime) + secCol * (1 - curTime);

                finalCol.a = finalCol.r * _Color.a;
                finalCol.rgb = _Color;

                return finalCol;
            }
            ENDCG
        }
    }
}
