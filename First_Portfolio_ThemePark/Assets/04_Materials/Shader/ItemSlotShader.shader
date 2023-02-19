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
            o.Emission = float3(0, 1, 0);   // Emission�� �⺻�ʷϻ����� ����

            float rim = saturate(dot(o.Normal, IN.viewDir));

            rim = pow(1 - rim, 3);                // �������� ������ ���İ��� �����Ѵ�.
            o.Alpha = rim * abs(sin(_Time.y));    // �ݺ��ֱ⸦ ������ �ֱ� ���Ͽ� ���밪 ���
        }


        ENDCG
    }
        FallBack "Diffuse"

}
