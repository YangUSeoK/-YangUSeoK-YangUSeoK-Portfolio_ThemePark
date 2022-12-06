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
            //o.Albedo = c.rgb;             // �˺��� ���� X
            o.Emission = float3(0, 1, 0);   // Emission�� �⺻�ʷϻ����� ����

            float rim = saturate(dot(o.Normal, IN.viewDir));

            rim = pow(1 - rim, 3);                            // �������� ������ ���İ��� �����Ѵ�.
            //o.Alpha = rim * sin(_Time.y);                    // �ʷ� ����ũ���� �������� 1�κκ��� ���İ��� 0�̵Ǿ� ����������, �׵θ��� �� ���� ������ ���δ�.
            //o.Alpha = rim * (sin(_Time.y) * 0.5f + 0.5f);
            //o.Alpha = rim * (sin(_Time.y) * 0.5f + 0.5f);
            o.Alpha = rim * abs(sin(_Time.y));                // abs : ���밪

            //o.Alpha = 1;
            //o.Emission = IN.worldPos.g;          // ���� ����� ���� = ��ȸ��
            //o.Emission = frac(IN.worldPos.g);          // �Ҽ����� ����
            //o.Emission = pow(frac(IN.worldPos.g),30);          // �����ſ� ���� ����(����)
            //o.Emission = pow(frac(IN.worldPos.g * 3),30);      // �� ������ ����
            //o.Emission = pow(frac(IN.worldPos.rgb * 3 - _Time.y),30);      // ���� �ð����� ����

        }
        ENDCG
    }
        FallBack "Diffuse"

}
