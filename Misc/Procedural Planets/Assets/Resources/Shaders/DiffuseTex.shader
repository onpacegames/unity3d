// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "DiffusePlanet" {
   Properties {
      _Color ("Diffuse Material Color", Color) = (1,1,1,1)
      
      	_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColorShift ("Color Shift", Vector) = (1,1,1)
		_PlanetPos ("Planet Position", Vector) = (0,0,0)
		
		_DetailTex ("Detail Texture", 2D) = "black" {}
		
		_BlendThreshold ("Blending Threshold", float) = 0
		
		_ChangePoint ("Change at this distance", Float) = 0.002
		
		_MultTest ("Test Multiplier", float ) =  1
   }
   SubShader {
      Pass {    
       
         Tags { "LightMode" = "ForwardBase" } 
           // pass for first light source
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
         #pragma fragmentoption ARB_fog_exp2
 
         uniform float4 _Color; // define shader property for shaders
         sampler2D _MainTex;
		 sampler2D _DetailTex;
		 uniform sampler2D _DetailTex2;
		 float _ChangePoint;
		 float _BlendThreshold;
		 uniform float _MultTest;
		 uniform float fInnerRadius;
		 
		 float CoastalMax = -0.005;
		 float MidMax = 0.000;
		 float HighMax = 0.005;
			
		uniform float3 _PlanetPos;
 
         // The following built-in uniforms (apart from _LightColor0) 
         // are defined in "UnityCG.cginc", which could be #included 
          // w = 1/scale; see _World2Object
         
          // inverse model matrix 
            // (all but the bottom-right element have to be scaled 
            // with unity_Scale.w if scaling is important) 
          
            // position or direction of light source
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
            float2 uv : TEXCOORD0;
            float3 vertpos : TEXCOORD1;
            
            
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
            float3 v3Pos = (mul(_Object2World, input.vertex).xyz) - _PlanetPos;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            float3 normalDirection = normalize(float3(
               mul(float4(input.normal, 0.0), modelMatrixInverse)));
            //normalDirection = normalize(input.normal) 
            float3 lightDirection = normalize(
               float3(_WorldSpaceLightPos0));
 
            float3 diffuseReflection = 
               float3(_LightColor0) * float3(_Color)
               * max(0.0, dot(normalDirection, lightDirection));
 
            output.col = float4(diffuseReflection, 1.0);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            output.vertpos = v3Pos;
            output.uv = input.texcoord;
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         		input.uv = input.uv * _MultTest;
				_BlendThreshold = 0.1;
				float3 vertp = input.vertpos.xyz;
				float _length = length(vertp);
			    float disp = (_length - fInnerRadius)/fInnerRadius;
			    half3 texel = tex2D(_MainTex, input.uv).rgb;
			    half3 lowercol = float3(0.84,0.757,0.42);
			    half3 uppercol = float3(0.6,0.7,0.1882352941176471);
			    half3 highercol = float3(0.9,0.9,1);
			    
			      _ChangePoint = fInnerRadius * (1 + (CoastalMax));
			      //lowercol = float3(0.2,0.2,0.6);
			      //uppercol = float3(0.0,0.2,0.0);
			    
			  
			   
			    	
			    
			    
			    float startBlending = _ChangePoint - _BlendThreshold;
         		float endBlending = _ChangePoint + _BlendThreshold;
         		
         		float startBlending2 = _ChangePoint + 0.7 - _BlendThreshold;
         		float endBlending2 = _ChangePoint + 0.7 - _BlendThreshold;
 
         		float curDistance = _length;
         		float changeFactor = saturate((curDistance - startBlending) / (_BlendThreshold * 2));
         		float changeFactor2 = saturate((curDistance - startBlending2) / (_BlendThreshold * 2));
         		
			    
			    if (disp < 0)
			    {
					texel = float3(1,0.2,0.2);
				}
				if (disp > 0.007)
				{
					texel = float3(1,1,1);
				}
				half3 ttexel = lerp(lowercol, uppercol, changeFactor);
				texel = lerp(ttexel, highercol, changeFactor2);
				half3 texel2 = tex2D(_DetailTex, input.uv).rgb;
				
				half3 tex1hold = tex2D(_DetailTex, input.uv).rgb;
				half3 tex2hold = tex2D(_DetailTex2, input.uv).rgb;
				texel2 = lerp(tex1hold, tex2hold, changeFactor);
				
				texel = texel + (texel2);
         		float3 col = input.col;
         		return half4(texel*col,1.0);
         
         
            return input.col;
         }
 
         ENDCG
      }
 
      
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Diffuse"
}