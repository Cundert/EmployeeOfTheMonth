Shader "GUI/StencilShaderGUI" { 
    Properties {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
    }

    SubShader {

        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Lighting Off Cull Off ZTest Always ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 fovuv : TEXCOORD2;
                float4 ogvert : TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed4 _Color;
            uniform sampler2D _StencilBuffer;
            
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

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Color;
                o.ogvert = mul( unity_ObjectToWorld, v.vertex );
                o.fovuv = ObjToScreenUV(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float prop = 0.8;
                fixed4 col = i.color;
                fixed4 mask = tex2D(_StencilBuffer, i.fovuv);
                float z = i.ogvert.z;
                if(mask.r == 0 && z < 5) discard;
                col.a *= tex2D(_MainTex, i.texcoord).a;
                return col;
            }
            ENDCG
        }
    }
}
