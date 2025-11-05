Shader "Custom/HeatWaveTwirl2D"
{
    Properties
    {
        _Distortion("Wave Distortion", Range(0, 0.1)) = 0.02
        _Twirl("Twirl Strength", Range(0, 5)) = 1.0
        _Speed("Animation Speed", Range(0, 10)) = 1.5
    }
        SubShader
    {
        // We need this to render after opaque objects and to handle transparency correctly.
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        // GrabPass captures the screen behind the object into _GrabTexture
        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 grabUV : TEXCOORD0; // UV for the GrabPass texture
            };

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize; // Provides pixel size for the texture

            float _Distortion;
            float _Twirl;
            float _Speed;

            v2f vert(appdata v)
            {
                v2f o;
                // Calculate the position on screen
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Calculate the UV for the grabbed screen texture
                o.grabUV = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // We need to perform a perspective divide on the grabUV for it to be correct.
                // This converts it from clip space to normalized screen space (0 to 1).
                float2 uv = i.grabUV.xy / i.grabUV.w;

                // --- Heat Wave (Sine Wave Distortion) ---
                // Add a time-based vertical sine wave offset to the x-coordinate
                float waveOffset = sin(uv.y * 20.0 + _Time.y * _Speed) * _Distortion;
                uv.x += waveOffset;

                // --- Twirl Distortion ---
                // Remap UVs to be centered at (0,0) instead of (0.5, 0.5)
                float2 centeredUV = uv - 0.5;

                // Calculate distance from center
                float dist = length(centeredUV);

                // Calculate the rotation angle. The twirl is stronger near the center.
                float angle = _Twirl * (1.0 - dist) * _Time.y * _Speed;

                // Create a rotation matrix
                float s = sin(angle);
                float c = cos(angle);
                float2x2 rotationMatrix = float2x2(c, -s, s, c);

                // Apply the rotation
                centeredUV = mul(rotationMatrix, centeredUV);

                // Remap UVs back to (0,1) range
                uv = centeredUV + 0.5;

                // Sample the grabbed screen texture with the final distorted UVs
                fixed4 col = tex2D(_GrabTexture, uv);
                return col;
            }
            ENDCG
        }
    }
}