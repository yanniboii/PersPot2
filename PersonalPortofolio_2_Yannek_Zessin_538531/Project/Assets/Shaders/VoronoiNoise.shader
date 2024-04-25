Shader "Custom/VoronoiShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" { }
        _Scale ("Scale", Range(0.1, 10)) = 1.0
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard
        
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        float _Scale;

        // Function to generate a random offset based on a position
        float3 RandomOffset(float3 position) {
            return frac(sin(dot(position, float3(12.9898, 78.233, 45.543))) * 43758.5453);
        }

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Calculate Voronoi cell color based on world position with random offset
            float3 worldPosScaled = (IN.worldPos + RandomOffset(IN.worldPos)) * _Scale;
            float3 cellCenter = floor(worldPosScaled);
            float3 cellOffset = frac(worldPosScaled);

            // Calculate distance to the nearest cell center
            float minDist = 1.0; // Start with a large distance
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    for (int k = -1; k <= 1; k++) {
                        float3 neighborCell = cellCenter + float3(i, j, k);
                        float3 neighborOffset = frac(neighborCell - cellOffset);
                        float dist = length(neighborOffset);
                        minDist = min(minDist, dist);
                    }
                }
            }

            // Use distance as a factor to generate Voronoi pattern
            float voronoi = smoothstep(0.1, 0.2, minDist);

            // Apply color and texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb * voronoi;
            o.Alpha = c.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}