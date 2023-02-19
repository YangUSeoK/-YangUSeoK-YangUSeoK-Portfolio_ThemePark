Shader "Custom/ItemSlotShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert noambient alpha:fade

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Emission = float3(0, 1, 0);   // Emission을 기본초록색으로 설정

            float rim = saturate(dot(o.Normal, IN.viewDir));

            rim = pow(1 - rim, 3);                // 내적값을 가지고 알파값을 설정한다.
            o.Alpha = rim * abs(sin(_Time.y));    // 반복주기를 빠르게 주기 위하여 절대값 사용
        }


        ENDCG
    }
        FallBack "Diffuse"

}
