Shader "VacuumShaders/Terrain To Mesh/Standard/Bumped/2 Textures" 
{
	Properties 
	{
		_Color("Tint Color", color) = (1, 1, 1, 1)
		_V_T2M_Control ("Control Map (RGBA)", 2D) = "black" {}

		//TTM				
		[V_T2M_Layer] _V_T2M_Splat1 ("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat1_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat1_bumpMap("", 2D) = ""{}	
		[HideInInspector] _V_T2M_Splat1_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat1_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat2 ("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat2_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat2_bumpMap("", 2D) = ""{}
		[HideInInspector] _V_T2M_Splat2_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat2_Metallic("Metallic", Range(0,1)) = 0.0
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		#define V_T2M_STANDARD
		#define V_T2M_BUMP 
		 
		#include "../cginc/T2M_Deferred.cginc"		

		ENDCG
	} 

	FallBack "VacuumShaders/Terrain To Mesh/Legacy Shaders/Diffuse/2 Textures" 
}
