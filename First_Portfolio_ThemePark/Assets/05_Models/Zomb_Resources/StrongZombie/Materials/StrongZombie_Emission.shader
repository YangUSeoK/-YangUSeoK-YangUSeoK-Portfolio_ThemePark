Shader "Custom/StrongZombie_Emission"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("NormalMap",2D) = "bump" {}
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPower("RimPower", Range(1,100)) = 3
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        float4 _RimColor;
        float _RimPower;

        struct Input
        {
            float2 uv_MainTex;
            float uv_BumpMap;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            //o.Emission = float3(1, 0, 0);

            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            float3 rim = saturate(dot(o.Normal, IN.viewDir));

            o.Metallic = _Metallic;

            o.Smoothness = _Glossiness;

            o.Albedo = c.rgb;
            o.Emission = pow(1 - rim, _RimPower) * _RimColor.rgb;
            o.Alpha = c.a;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
