// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "Hidden/VacuumShaders/Terrain To Mesh/Basemap" 
{
	Properties 
	{
		_MainTex("", 2D) = "white" {}
		
		//TTM				
		_V_T2M_Control ("", 2D) = "black" {}
		_V_T2M_ChannelIndex ("", float) = 4

		_V_T2M_Splat_D ("", 2D) = "white" {}
		_V_T2M_Splat_N ("", 2D) = "bump" {}
		_V_T2M_Splat_uvScale("", vector) = (1, 1, 0, 0)
	}
	 


	CGINCLUDE

	#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_TexelSize;

	sampler2D _V_T2M_Control;
	float _V_T2M_ChannelIndex;

	sampler2D _V_T2M_Splat_D;
	sampler2D _V_T2M_Splat_N;
	float4 _V_T2M_Splat_uvScale;

	float4 _V_T2M_Splat_uvOffset;

	struct v2f 
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert( appdata_img v ) 
	{ 
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);		
		
		o.uv =  v.texcoord.xy;	
		#if UNITY_UV_STARTS_AT_TOP
		if(_MainTex_TexelSize.y < 0.0)
			o.uv.y = 1.0 - o.uv.y;
		#endif

		return o;
	}

	half4 fragBasemapD(v2f i) : SV_Target 
	{
		int index = 0;
		if(_V_T2M_ChannelIndex >= 1 && _V_T2M_ChannelIndex < 2) index = 1;
		else if(_V_T2M_ChannelIndex >= 2 && _V_T2M_ChannelIndex < 3) index = 2;
		else if(_V_T2M_ChannelIndex >= 3 && _V_T2M_ChannelIndex < 4) index = 3;
				
		_V_T2M_Splat_uvScale.zw += _V_T2M_Splat_uvScale.xy * _V_T2M_Splat_uvOffset.zw;
		_V_T2M_Splat_uvScale.xy *= _V_T2M_Splat_uvOffset.xy;
		float2 mapUV = float2(i.uv * _V_T2M_Splat_uvScale.xy + _V_T2M_Splat_uvScale.zw);
			

		float2 controlUV = float2(i.uv * _V_T2M_Splat_uvOffset.xy + _V_T2M_Splat_uvOffset.zw);
		
		half4 c = tex2D(_MainTex, i.uv) + tex2D (_V_T2M_Splat_D, mapUV) * tex2D (_V_T2M_Control, controlUV)[index];

		return c;
	}

	half4 fragBasemapN1(v2f i) : SV_Target
	{
		int index = 0;
		if(_V_T2M_ChannelIndex >= 1 && _V_T2M_ChannelIndex < 2) index = 1;
		else if(_V_T2M_ChannelIndex >= 2 && _V_T2M_ChannelIndex < 3) index = 2;
		else if(_V_T2M_ChannelIndex >= 3 && _V_T2M_ChannelIndex < 4) index = 3;

		_V_T2M_Splat_uvScale.zw += _V_T2M_Splat_uvScale.xy * _V_T2M_Splat_uvOffset.zw;
		_V_T2M_Splat_uvScale.xy *= _V_T2M_Splat_uvOffset.xy;
		float2 mapUV = float2(i.uv * _V_T2M_Splat_uvScale.xy + _V_T2M_Splat_uvScale.zw);


		float2 controlUV = float2(i.uv * _V_T2M_Splat_uvOffset.xy + _V_T2M_Splat_uvOffset.zw);

		half4 c = tex2D(_MainTex, i.uv) + tex2D(_V_T2M_Splat_N, mapUV) * tex2D(_V_T2M_Control, controlUV)[index];

		return c;
	}

	half4 fragBasemapN2(v2f i) : SV_Target
	{
		half4 c = tex2D(_MainTex, i.uv);
		c.x = c.a;
		c.a = 1;

		return c;
	}

	ENDCG
	

	SubShader    
	{				    
		Pass
	    {
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
	    	#pragma fragment fragBasemapD
			ENDCG

		} //Pass
		
		Pass
	    {
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert 
	    	#pragma fragment fragBasemapN1
			ENDCG

		} //Pass

		Pass
	    {
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
	    	#pragma fragment fragBasemapN2
			ENDCG

		} //Pass

	} //SubShader
	 
} //Shader
