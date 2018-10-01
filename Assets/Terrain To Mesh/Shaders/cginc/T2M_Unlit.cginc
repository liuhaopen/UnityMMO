#ifndef VACUUM_SHADERS_T2M_UNLIT_CGINC
#define VACUUM_SHADERS_T2M_UNLIT_CGINC


#include "../cginc/T2M_Variables.cginc"


float4 _V_T2M_Control_ST;
#ifdef V_T2M_2_CONTROL_MAPS
	float4 _V_T2M_Control2_ST;
#endif

struct vInput
{
	float4 vertex : POSITION;    
	float4 texcoord : TEXCOORD0;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct vOutput
{
	float4 pos : SV_POSITION;
	float2 uv_V_T2M_Control : TEXCOORD0;

	#ifdef V_T2M_2_CONTROL_MAPS
		float2 uv_V_T2M_Control2 : TEXCOORD1;
	#endif

	UNITY_FOG_COORDS(2)

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

	mainTex.a = 1.0;

	UNITY_APPLY_FOG(IN.fogCoord, mainTex);
	UNITY_OPAQUE_ALPHA(mainTex.a);

	return mainTex;
}

#endif