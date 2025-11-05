Shader "Custom/HeatPulse2D"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}     // ✅ added
        _DistortionTex("Distortion Noise", 2D) = "gray" {}
        _DistortionStrength("Distortion Strength", Range(0, 0.1)) = 0.03
        _PulseSpeed("Pulse Speed", Range(0, 10)) = 2.0
        _Tint("Tint", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            GrabPass { "_GrabTex" }

            Pass
            {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha
                Cull Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _GrabTex;
                sampler2D _MainTex;           // ✅ added
                sampler2D _DistortionTex;
                float4 _DistortionTex_ST;
                float4 _MainTex_ST;           // ✅ added
                float _DistortionStrength;
                float _PulseSpeed;
                float4 _Tint;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uvMain : TEXCOORD0;    // ✅ new UV for main texture
                    float2 uvDistort : TEXCOORD1;
                    float4 grabPos : TEXCOORD2;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uvMain = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uvDistort = TRANSFORM_TEX(v.uv, _DistortionTex);
                    o.grabPos = ComputeGrabScreenPos(o.pos);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                    float2 noise = tex2D(_DistortionTex, i.uvDistort * 2.0).rg - 0.5;
                    float2 offset = noise * _DistortionStrength * pulse;
                    float2 distortedUV = (i.grabPos.xy / i.grabPos.w) + offset;

                    // Sample the grabbed background
                    fixed4 col = tex2D(_GrabTex, distortedUV);

                    // Multiply by main texture color if needed
                    fixed4 main = tex2D(_MainTex, i.uvMain);
                    return col * main * _Tint;
                }
                ENDCG
            }
        }
}
