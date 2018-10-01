#ifndef VACUUM_SHADERS_T2M_VARIABLES_CGINC
#define VACUUM_SHADERS_T2M_VARIABLES_CGINC

fixed4 _Color;

sampler2D _V_T2M_Control;
sampler2D _V_T2M_Splat1; half _V_T2M_Splat1_uvScale;
sampler2D _V_T2M_Splat2; half _V_T2M_Splat2_uvScale;

#ifdef V_T2M_BUMP
	sampler2D _V_T2M_Splat1_bumpMap;
	sampler2D _V_T2M_Splat2_bumpMap;
#endif

#ifdef V_T2M_3_TEX
sampler2D _V_T2M_Splat3; half _V_T2M_Splat3_uvScale;

	#ifdef V_T2M_BUMP
		sampler2D _V_T2M_Splat3_bumpMap;
	#endif
#endif

#ifdef V_T2M_4_TEX
sampler2D _V_T2M_Splat4; half _V_T2M_Splat4_uvScale;

	#ifdef V_T2M_BUMP
		sampler2D _V_T2M_Splat4_bumpMap;
	#endif
#endif 

#ifdef V_T2M_2_CONTROL_MAPS
	sampler2D _V_T2M_Control2;

	sampler2D _V_T2M_Splat5; half _V_T2M_Splat5_uvScale;
	#ifdef V_T2M_BUMP
		sampler2D _V_T2M_Splat5_bumpMap;
	#endif

	#ifdef V_T2M_6_TEX
		sampler2D _V_T2M_Splat6; half _V_T2M_Splat6_uvScale;
		#ifdef V_T2M_BUMP
			sampler2D _V_T2M_Splat6_bumpMap;
		#endif
	#endif

	#ifdef V_T2M_7_TEX
		sampler2D _V_T2M_Splat7; half _V_T2M_Splat7_uvScale;
	#endif

	#ifdef V_T2M_8_TEX
		sampler2D _V_T2M_Splat8; half _V_T2M_Splat8_uvScale;
	#endif
#endif

#ifdef V_T2M_SPECULAR
	half _V_T2M_Splat1_Shininess;
	half _V_T2M_Splat2_Shininess;

	#ifdef V_T2M_3_TEX
		half _V_T2M_Splat3_Shininess;
	#endif
	#ifdef V_T2M_4_TEX
		half _V_T2M_Splat4_Shininess;
	#endif
#endif

#ifdef V_T2M_STANDARD
	half _V_T2M_Splat1_Glossiness;
	half _V_T2M_Splat1_Metallic;

	half _V_T2M_Splat2_Glossiness;
	half _V_T2M_Splat2_Metallic;

	#ifdef V_T2M_3_TEX
		half _V_T2M_Splat3_Glossiness;
		half _V_T2M_Splat3_Metallic;
	#endif

	#ifdef V_T2M_4_TEX
		half _V_T2M_Splat4_Glossiness;
		half _V_T2M_Splat4_Metallic;
	#endif

	#ifdef V_T2M_5_TEX
		half _V_T2M_Splat5_Glossiness;
		half _V_T2M_Splat5_Metallic;
	#endif

	#ifdef V_T2M_6_TEX
		half _V_T2M_Splat6_Glossiness;
		half _V_T2M_Splat6_Metallic;
	#endif

	#ifdef V_T2M_7_TEX
		half _V_T2M_Splat7_Glossiness;
		half _V_T2M_Splat7_Metallic;
	#endif

	#ifdef V_T2M_8_TEX
		half _V_T2M_Splat8_Glossiness;
		half _V_T2M_Splat8_Metallic;
	#endif
#endif

#endif
