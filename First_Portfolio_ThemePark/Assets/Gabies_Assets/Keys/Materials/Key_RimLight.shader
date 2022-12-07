Shader "Custom/Key_RimLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BumpMap("NormalMap", 2D) = "bump" {}
        _RimColor("RimColor", Color) = (1, 1, 1, 1)
        _RimPower("RimPower", Range(1, 10)) = 3

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
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
            float2 uv_BumpMap;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            float rim = saturate(dot(o.Normal, IN.viewDir));        // 카메라 방향과 픽셀을 내적(벡터의 각도를 알아냄)해서 테두리를 계산한다.
            o.Emission = pow(1 - rim, _RimPower) * _RimColor.rgb;   // 조명은 수치가 작아질 수록 빛이 약해지지만, 
            o.Alpha = c.a;   // 수치를 반대로 하기 위해 1-rim          // rimlight는 수치가 작아질 수록(테두리에 가까이 갈 수록) 표현하는 색이 더 커진다.
        }
        ENDCG
    }
    FallBack "Diffuse"
}
