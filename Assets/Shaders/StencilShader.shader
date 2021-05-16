Shader "Unlit/StencilShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Back ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 fovuv : TEXCOORD2;
                float4 ogvert : TEXCOORD3;
            };

			uniform sampler2D _StencilBuffer;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
			inline float2 ObjToScreenUV(in float3 obj_pos)
			{
				float4 clip_pos4d = UnityObjectToClipPos(obj_pos);
				float2 clip_pos2d = clip_pos4d.xy / clip_pos4d.w;
				#if UNITY_UV_STARTS_AT_TOP
					return float2(1 + clip_pos2d.x, 1 - clip_pos2d.y) / 2;
				#else
					return (1 + clip_pos2d) / 2;
				#endif
			}
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.ogvert = mul( unity_ObjectToWorld, v.vertex );
                o.fovuv = ObjToScreenUV(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float prop = 0.8;
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_StencilBuffer, i.fovuv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                float z = i.ogvert.z;
                //return z;
                
                if(mask.r == 0 && z < 5) discard;
                
                return col * (mask.r * prop + 1 - prop) * float4(1,1,1,0) + float4(0,0,0,1);
            }
            ENDCG
        }
    }
}
