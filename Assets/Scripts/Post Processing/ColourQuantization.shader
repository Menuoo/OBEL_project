Shader "Custom/ColourQuantization"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColourAmount ("Colour Amount", Int) = 64
        _Spread ("Dither Spread", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler3D _DitherMaskLOD;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            int _ColourAmount;
            float _Spread;

            static const int _BayerMatrix[16] = {
                0, 8, 2, 10, 
                12, 4, 14, 6,
                3, 11, 1, 9,
                15, 7, 13, 5
            };




            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 pixelCoord = (i.uv * _MainTex_TexelSize.zw) % 4;

                float mapValue = _BayerMatrix[pixelCoord.y * 4 + pixelCoord.x] / 16.0 - 0.5;

                col.rgb = floor(col.rgb * _ColourAmount + mapValue * _Spread) / _ColourAmount;

                return col;
            }
            ENDCG
        }
    }
}
