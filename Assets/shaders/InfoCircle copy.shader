// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CityOases/InfoCircle"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
        _Circle ("Circle", Float) = 0.0
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
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }



        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };


            #define M_PI_2 6.283185307179587
            
            fixed4 _Color;


            float _Circle;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
#endif
                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

                float vecx = IN.texcoord.x - 0.5;
                float vecy = IN.texcoord.y - 0.5;//_Circle;//0.5;

                float rad = M_PI_2 * _Circle;

                if(vecx >= 0 && vecy > 0) {
                    float angle = atan(vecx / vecy);
                    if(angle > rad) {
                        color.a = 0.0;
                    }
                } else if(vecx > 0 && vecy <= 0) {
                    float angle = atan(-vecy / vecx);
                    if((angle + (M_PI_2 * 0.25)) > rad) {
                        color.a = 0.0;
                    }
                } else if(vecx <= 0 && vecy < 0) {
                    float angle = atan(-vecx / -vecy);
                    if((angle + (M_PI_2 * 0.5)) > rad) {
                        color.a = 0.0;
                    }
                } else if(vecx < 0 && vecy >= 0) {
                    float angle = atan(-vecy / vecx);
                    if((angle + (M_PI_2 * 0.75)) > rad) {
                        color.a = 0.0;
                    }
                }

                clip (color.a - 0.01);
                return color;
            }
        ENDCG
        }
    }
}