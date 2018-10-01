// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Terrain To Mesh/Unlit/3 Textures" 
{
	Properties 
	{
		_Color("Tint Color", color) = (1, 1, 1, 1)
		_V_T2M_Control ("Control Map (RGBA)", 2D) = "black" {}

		//TTM				
		[V_T2M_Layer] _V_T2M_Splat1 ("Layer 1 (R)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat1_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat2 ("Layer 2 (G)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat2_uvScale("", float) = 1	

		[V_T2M_Layer] _V_T2M_Splat3 ("Layer 3 (B)", 2D) = "white" {}
		[HideInInspector] _V_T2M_Splat3_uvScale("", float) = 1	
	}
	  
	
	SubShader   
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		   
		Pass
	    {
			CGPROGRAM
			#pragma vertex vert 
	    	#pragma fragment frag
			#pragma multi_compile_fog	
			#pragma target 2.0
			#include "UnityCG.cginc"
				 

			#define V_T2M_3_TEX

			#include "../cginc/T2M_Unlit.cginc" 

			ENDCG

		} //Pass

	} //SubShader
	 
} //Shader
