Shader "Custom/SwarmIndirect"
{
    Properties {}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct CubeData
            {
                float4 position;
                float4 color;
            };

            StructuredBuffer<CubeData> _Buffer;
            float _Scale;

            struct appdata
            {
                float3 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 col : COLOR;
            };

            v2f vert(appdata v, uint id : SV_InstanceID)
            {
                v2f o;
                CubeData d = _Buffer[id];

                float3 scaled = v.vertex * _Scale;
                float3 worldPos = d.position.xyz + scaled;

                o.pos = UnityWorldToClipPos(float4(worldPos, 1.0));
                o.col = d.color;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return i.col;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
