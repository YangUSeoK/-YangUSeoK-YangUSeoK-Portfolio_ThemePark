Shader "Custom/ItemSlotShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        //Tags { "RenderType" = "Opaque" }

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
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            //o.Albedo = c.rgb;             // 알베도 색깔 X
            o.Emission = float3(0, 1, 0);   // Emission을 기본초록색으로 설정

            float rim = saturate(dot(o.Normal, IN.viewDir));

            rim = pow(1 - rim, 3);                            // 내적값을 가지고 알파값을 설정한다.
            //o.Alpha = rim * sin(_Time.y);                    // 초록 마스크맨이 내적값이 1인부분은 알파값이 0이되어 투명해지고, 테두리로 갈 수록 윤곽이 보인다.
            //o.Alpha = rim * (sin(_Time.y) * 0.5f + 0.5f);
            //o.Alpha = rim * (sin(_Time.y) * 0.5f + 0.5f);
            o.Alpha = rim * abs(sin(_Time.y));                // abs : 절대값

            //o.Alpha = 1;
            //o.Emission = IN.worldPos.g;          // 같은 색깔로 대입 = 검회흰
            //o.Emission = frac(IN.worldPos.g);          // 소숫점만 리턴
            //o.Emission = pow(frac(IN.worldPos.g),30);          // 위에거에 강도 조절(제곱)
            //o.Emission = pow(frac(IN.worldPos.g * 3),30);      // 층 개수를 조절
            //o.Emission = pow(frac(IN.worldPos.rgb * 3 - _Time.y),30);      // 층을 시간으로 조절

        }
        ENDCG
    }
        FallBack "Diffuse"

}
