Shader "CityOases/PinCircle" {
Properties {
	_Color ("Main Color", Color) = (1,0,0,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "black" {}
    _Circle ("Circle", Float) = 0.0
}

SubShader {
	Tags {"Queue"="Transparent-50" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha

#define M_PI_2 6.283185307179587

sampler2D _MainTex;
fixed4 _Color;
//fixed1 _Circle;
float _Circle;

struct Input {
	float2 uv_MainTex;
	float2 uv_TrafficTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

    float vecx = IN.uv_MainTex.x - 0.5;
    float vecy = IN.uv_MainTex.y - 0.5;//_Circle;//0.5;

   // float rad = M_PI_2 * _Circle;

    if(vecx >= 0 && vecy > 0) {
        float angle = atan(vecx / vecy);
        if(angle > _Circle) {
            c.a = 0.0;
        }
    } else if(vecx > 0 && vecy <= 0) {
        float angle = atan(-vecy / vecx);
        if((angle + (M_PI_2 * 0.25)) > _Circle) {
            c.a = 0.0;
        }
    } else if(vecx <= 0 && vecy < 0) {
        float angle = atan(-vecx / -vecy);
        if((angle + (M_PI_2 * 0.5)) > _Circle) {
            c.a = 0.0;
        }
    } else if(vecx < 0 && vecy >= 0) {
        float angle = atan(-vecy / vecx);
        if((angle + (M_PI_2 * 0.75)) > _Circle) {
            c.a = 0.0;
        }
    }/**/

	fixed3 ct = c.rgb;
	ct = ct * _Color;
	o.Albedo = ct;
	o.Alpha = c.a;// * _Color.a;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}
