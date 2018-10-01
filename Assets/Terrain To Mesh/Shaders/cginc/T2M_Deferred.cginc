#ifndef VACUUM_SHADERS_T2M_DEFERRED_CGINC
#define VACUUM_SHADERS_T2M_DEFERRED_CGINC

#include "../cginc/T2M_Variables.cginc"

struct Input 
{
	float2 uv_V_T2M_Control;

	#ifdef V_T2M_2_CONTROL_MAPS
		float2 uv_V_T2M_Control2;
	#endif
};

#ifdef V_T2M_STANDARD
void surf (Input IN, inout SurfaceOutputStandard o)
#else
void surf (Input IN, inout SurfaceOutput o) 
#endif
{
	half4 splat_control = tex2D (_V_T2M_Control, IN.uv_V_T2M_Control);

	fixed4 mainTex  = splat_control.r * tex2D (_V_T2M_Splat1, IN.uv_V_T2M_Control * _V_T2M_Splat1_uvScale);
	       mainTex += splat_control.g * tex2D (_V_T2M_Splat2, IN.uv_V_T2M_Control * _V_T2M_Splat2_uvScale);
	
	#ifdef V_T2M_3_TEX
		mainTex += splat_control.b * tex2D (_V_T2M_Splat3, IN.uv_V_T2M_Control * _V_T2M_Splat3_uvScale);
	#endif
	#ifdef V_T2M_4_TEX
		mainTex += splat_control.a * tex2D (_V_T2M_Splat4, IN.uv_V_T2M_Control * _V_T2M_Splat4_uvScale);
	#endif


	#ifdef V_T2M_2_CONTROL_MAPS
		 half4 splat_control2 = tex2D (_V_T2M_Control2, IN.uv_V_T2M_Control2);

		 mainTex.rgb += tex2D (_V_T2M_Splat5, IN.uv_V_T2M_Control2 * _V_T2M_Splat5_uvScale) * splat_control2.r;

		 #ifdef V_T2M_6_TEX
			mainTex.rgb += tex2D (_V_T2M_Splat6, IN.uv_V_T2M_Control2 * _V_T2M_Splat6_uvScale) * splat_control2.g;
		 #endif

		 #ifdef V_T2M_7_TEX
			mainTex.rgb += tex2D (_V_T2M_Splat7, IN.uv_V_T2M_Control2 * _V_T2M_Splat7_uvScale) * splat_control2.b;
		 #endif

		 #ifdef V_T2M_8_TEX
			mainTex.rgb += tex2D (_V_T2M_Splat8, IN.uv_V_T2M_Control2 * _V_T2M_Splat8_uvScale) * splat_control2.a;
		 #endif
	#endif



	mainTex.rgb *= _Color.rgb;

	 
	#ifdef V_T2M_BUMP
		fixed4 nrm = 0.0f;
		nrm += splat_control.r * tex2D(_V_T2M_Splat1_bumpMap, IN.uv_V_T2M_Control * _V_T2M_Splat1_uvScale);
		nrm += splat_control.g * tex2D(_V_T2M_Splat2_bumpMap, IN.uv_V_T2M_Control * _V_T2M_Splat2_uvScale);

		#ifdef V_T2M_3_TEX
			nrm += splat_control.b * tex2D (_V_T2M_Splat3_bumpMap, IN.uv_V_T2M_Control * _V_T2M_Splat3_uvScale);
		#endif

		#ifdef V_T2M_4_TEX
			nrm += splat_control.a * tex2D (_V_T2M_Splat4_bumpMap, IN.uv_V_T2M_Control * _V_T2M_Splat4_uvScale);
		#endif
		 
		 
		o.Normal = UnpackNormal(nrm);
	#endif


	

	#ifdef V_T2M_STANDARD
		half metallic = 0;
		metallic += splat_control.r * _V_T2M_Splat1_Metallic;
		metallic += splat_control.g * _V_T2M_Splat2_Metallic;
		#ifdef V_T2M_3_TEX
			metallic += splat_control.b * _V_T2M_Splat3_Metallic;
		#endif
		#ifdef V_T2M_4_TEX
			metallic += splat_control.a * _V_T2M_Splat4_Metallic;
		#endif
		#ifdef V_T2M_2_CONTROL_MAPS
			#ifdef V_T2M_5_TEX
				metallic += splat_control2.r * _V_T2M_Splat5_Metallic;
			#endif
			#ifdef V_T2M_6_TEX
				metallic += splat_control2.g * _V_T2M_Splat6_Metallic;
			#endif
			#ifdef V_T2M_7_TEX
				metallic += splat_control2.b * _V_T2M_Splat7_Metallic;
			#endif
			#ifdef V_T2M_8_TEX
				metallic += splat_control2.a * _V_T2M_Splat8_Metallic;
			#endif
		#endif


		half glossiness = 0;
		glossiness += splat_control.r * _V_T2M_Splat1_Glossiness;
		glossiness += splat_control.g * _V_T2M_Splat2_Glossiness;
		#ifdef V_T2M_3_TEX
			glossiness += splat_control.b * _V_T2M_Splat3_Glossiness;
		#endif
		#ifdef V_T2M_4_TEX
			glossiness += splat_control.a * _V_T2M_Splat4_Glossiness;
		#endif
		#ifdef V_T2M_2_CONTROL_MAPS
			#ifdef V_T2M_5_TEX
				glossiness += splat_control2.r * _V_T2M_Splat5_Glossiness;
			#endif
			#ifdef V_T2M_6_TEX
				glossiness += splat_control2.g * _V_T2M_Splat6_Glossiness;
			#endif
			#ifdef V_T2M_7_TEX
				glossiness += splat_control2.b * _V_T2M_Splat7_Glossiness;
			#endif
			#ifdef V_T2M_8_TEX
				glossiness += splat_control2.a * _V_T2M_Splat8_Glossiness;
			#endif
		#endif

		o.Metallic = metallic;
		o.Smoothness = glossiness;
	#else
		#ifdef V_T2M_SPECULAR
			o.Gloss = mainTex.a;

			half shininess = 0;
			shininess += splat_control.r * _V_T2M_Splat1_Shininess;
			shininess += splat_control.g * _V_T2M_Splat2_Shininess;
			#ifdef V_T2M_3_TEX
				shininess += splat_control.b * _V_T2M_Splat3_Shininess;
			#endif
			#ifdef V_T2M_4_TEX
				shininess += splat_control.a * _V_T2M_Splat4_Shininess;
			#endif

			o.Specular = shininess;
		#endif
	#endif
	
	o.Albedo = mainTex.rgb;
	o.Alpha = 1.0;
}

#endif