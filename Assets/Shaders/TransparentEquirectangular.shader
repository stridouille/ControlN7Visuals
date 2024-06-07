Shader "Custom/TransparentEquirectangular"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _decreaseAlpha ("Decrease Alpha", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Cull off
        Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off

        Pass
        {
            CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma glsl
                #pragma target 3.0
                #include "UnityCG.cginc"

                struct appdata {
                   float4 vertex : POSITION;
                   float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4    pos : SV_POSITION;
                    float3    normal : TEXCOORD0;
                    float     decreaseAlpha: TEXCOORD1;
                };

                float _decreaseAlpha;

                v2f vert (appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.normal = v.normal;
                    o.decreaseAlpha = 1.0;
                    if (dot(v.normal, ObjSpaceViewDir(v.vertex)) < 0.0) {
                        o.decreaseAlpha = _decreaseAlpha;
                    }
                    return o;
                }

                sampler2D _MainTex;
                float4 _MainTex_ST;
                #define PI 3.141592653589793

                inline float2 RadialCoords(float3 a_coords)
                {
                    float3 a_coords_n = normalize(a_coords);
                    float lon = atan2(a_coords_n.z, a_coords_n.x);
                    float lat = acos(a_coords_n.y);
                    float2 sphereCoords = float2(lon, lat) * (1.0 / PI);
                    return float2(sphereCoords.x * 0.5 + 0.5, 1 - sphereCoords.y);
                }

                float4 frag(v2f IN) : COLOR
                {
                    float2 equiUV = RadialCoords(IN.normal);

                    //fix mipmap seams
                    float2 dx = ddx(equiUV);
                    float2 dy = ddy(equiUV);
                    float2 du = float2(dx.x, dy.x);
                    du -= (abs(du) > 0.5f) * sign(du);
                    dx.x = du.x;
                    dy.x = du.y;
                    equiUV.x += _MainTex_ST.z;     

                    fixed4 col = tex2Dgrad(_MainTex, equiUV, dx, dy) * float4(1, 1, 1, IN.decreaseAlpha);
                    return col;
                }

            ENDCG
        }
    }
}
