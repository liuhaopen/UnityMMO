Shader "VacuumShaders/Terrain To Mesh/Standard/Diffuse/8 Textures" 
{
	Properties 
	{
		_Color("Tint Color", color) = (1, 1, 1, 1)
		_V_T2M_Control ("Control Map (RGBA)", 2D) = "black" {}

		//TTM	
		[V_T2M_Layer] _V_T2M_Splat1 ("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat1_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat1_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat1_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat2 ("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat2_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat2_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat2_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat3 ("Layer 3 (B)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat3_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat3_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat3_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat4 ("Layer 4 (A)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat4_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat4_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat4_Metallic("Metallic", Range(0,1)) = 0.0


		 
		_V_T2M_Control2 ("Control Map 2 (RGBA)", 2D) = "black" {}

		//TTM				
		[V_T2M_Layer] _V_T2M_Splat5 ("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat5_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat5_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat5_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat6 ("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat6_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat6_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat6_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat7 ("Layer 3 (B)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat7_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat7_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat7_Metallic("Metallic", Range(0,1)) = 0.0

		[V_T2M_Layer] _V_T2M_Splat8 ("Layer 4 (A)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat8_uvScale("", float) = 1	
		[HideInInspector] _V_T2M_Splat8_Glossiness("Smoothness", Range(0,1)) = 0.5
		[HideInInspector] _V_T2M_Splat8_Metallic("Metallic", Range(0,1)) = 0.0
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma exclude_renderers d3d9

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		#define V_T2M_STANDARD
		#define V_T2M_2_CONTROL_MAPS
		#define V_T2M_3_TEX
		#define V_T2M_4_TEX
		#define V_T2M_5_TEX 
		#define V_T2M_6_TEX
		#define V_T2M_7_TEX
		#define V_T2M_8_TEX 

		#include "../cginc/T2M_Deferred.cginc"		

		ENDCG
	}  
	
	FallBack "VacuumShaders/Terrain To Mesh/Legacy Shaders/Diffuse/8 Textures" 
}
