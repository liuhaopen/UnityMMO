#ifndef VACUUM_SHADERS_T2M_ODL_CGINC
#define VACUUM_SHADERS_T2M_ODL_CGINC

#include "../cginc/T2M_Variables.cginc"


float4 _V_T2M_Control_ST;
#ifdef V_T2M_2_CONTROL_MAPS
	float4 _V_T2M_Control2_ST; 
#endif

float _V_T2M_Lightmap_Multiplier;

struct vInput
{
	float4 vertex : POSITION;    
	float4 texcoord : TEXCOORD0;	
	float3 normal : NORMAL;

	#ifdef V_T2M_BUMP
		float4 tangent : TANGENT;
	#endif

	#ifndef LIGHTMAP_OFF
		float4 texcoord1: TEXCOORD1;
	#endif

	UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct vOutput
{
	float4 pos : SV_POSITION;
	float2 uv_V_T2M_Control : TEXCOORD0;

	#ifdef V_T2M_2_CONTROL_MAPS
		float2 uv_V_T2M_Control2 : TEXCOORD1;
	#endif

	#ifdef V_T2M_BUMP
		fixed3 lightDir : TEXCOORD2;
	#else
		fixed3 normal : TEXCOORD2;
	#endif

	UNITY_FOG_COORDS(3)

	#ifdef LIGHTMAP_OFF
		fixed3 vlight : TEXCOORD4;   
		SHADOW_COORDS(5)
	#else
		float2 lmap : TEXCOORD4;
	#endif

	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};


vOutput vert(vInput v)
{ 
	UNITY_SETUP_INSTANCE_ID(v);
	vOutput o;
	UNITY_INITIALIZE_OUTPUT(vOutput,o); 
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	o.pos = UnityObjectToClipPos(v.vertex);	
	o.uv_V_T2M_Control = v.texcoord.xy * _V_T2M_Control_ST.xy + _V_T2M_Control_ST.zw;

	#ifdef V_T2M_2_CONTROL_MAPS
		o.uv_V_T2M_Control2 = v.texcoord.xy * _V_T2M_Control2_ST.xy + _V_T2M_Control2_ST.zw;
	#endif

	#ifndef LIGHTMAP_OFF
	    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	#endif


	float3 worldN = UnityObjectToWorldNormal(v.normal);
    #ifdef LIGHTMAP_OFF
		#ifdef V_T2M_BUMP
			TANGENT_SPACE_ROTATION;
		    o.lightDir = mul (rotation, ObjSpaceLightDir(v.vertex)); 
		#else
			o.normal = worldN;
		#endif
    #endif

    // SH/ambient and vertex lights
    #ifdef LIGHTMAP_OFF
		#ifdef UNITY_SHOULD_SAMPLE_SH
			float3 shlight = ShadeSH9 (float4(worldN,1.0));
			o.vlight = shlight;

			#ifdef VERTEXLIGHT_ON
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		
				o.vlight += Shade4PointLights (unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
											   unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
											   unity_4LightAtten0, worldPos, worldN );
			#endif // VERTEXLIGHT_ON
		#endif
    #endif // LIGHTMAP_OFF

    
	#ifdef LIGHTMAP_OFF
		TRANSFER_SHADOW(o);
	#endif
	
	UNITY_TRANSFER_FOG(o,o.pos); 


    return o;
}

fixed4 frag (vOutput IN) : SV_Target
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

		 
		half3 normal = UnpackNormal(nrm);
	#else
		#ifdef LIGHTMAP_OFF
			half3 normal = IN.normal;
		#else
			half3 normal = 0;
		#endif
	#endif


	// compute lighting & shadowing factor    
    fixed4 c = 0;

    // realtime lighting: call lighting function
    #ifdef LIGHTMAP_OFF
		#ifdef V_T2M_BUMP
			fixed diff = max (0, dot (normal, IN.lightDir));
		#else
			fixed diff = max (0, dot (normal, _WorldSpaceLightPos0.xyz));
		#endif

		fixed atten = LIGHT_ATTENUATION(IN);

		c.rgb = mainTex.rgb * _LightColor0.rgb * (diff * atten);
		c.a = mainTex.a;
  
  		c.rgb += mainTex.rgb * IN.vlight;
	#else

		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap));

        c.rgb += mainTex.rgb * lm * _V_T2M_Lightmap_Multiplier;
    #endif 
	
	c.a = 1;


	UNITY_APPLY_FOG(IN.fogCoord, c); 
	UNITY_OPAQUE_ALPHA(c.a);

	return c;
}

#endif