
Shader "Atmosphere/GroundFromAtmosphere" 
{

	Properties {
		_ColorShift ("Color Shift", Vector) = (1,1,1)
		_PlanetPos ("Planet Position", Vector) = (0,0,0)
		
		_DetailTex ("Detail Texture", 2D) = "black" {}
		
		_BlendThreshold ("Blending Threshold", float) = 0
		
		_ChangePoint ("Change at this distance", Float) = 0.002
		
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
    	Pass 
    	{
    		
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			sampler2D _DetailTex;
			float _ChangePoint;
			float _BlendThreshold;
			
			uniform float3 _PlanetPos;
			uniform float3 _ColorShift;
			uniform float3 v3LightPos;		// The direction vector to the light source
			uniform float3 v3InvWavelength; // 1 / pow(wavelength, 4) for the red, green, and blue channels
			uniform float fOuterRadius;		// The outer (atmosphere) radius
			uniform float fOuterRadius2;	// fOuterRadius^2
			uniform float fInnerRadius;		// The inner (planetary) radius
			uniform float fInnerRadius2;	// fInnerRadius^2
			uniform float fKrESun;			// Kr * ESun
			uniform float fKmESun;			// Km * ESun
			uniform float fKr4PI;			// Kr * 4 * PI
			uniform float fKm4PI;			// Km * 4 * PI
			uniform float fScale;			// 1 / (fOuterRadius - fInnerRadius)
			uniform float fScaleDepth;		// The scale depth (i.e. the altitude at which the atmosphere's average density is found)
			uniform float fScaleOverScaleDepth;	// fScale / fScaleDepth
			uniform float fHdrExposure;		// HDR exposure
			uniform float g;				// The Mie phase asymmetry factor
			uniform float g2;				// The Mie phase asymmetry factor squared
			float CoastalMax = -0.002;
			float MidMax = 0.003;
			float HighMax = 0.005;
		
			struct v2f 
			{
    			float4 pos : SV_POSITION;
    			float2 uv : TEXCOORD0;
    			float3 c0 : COLOR0;
    			float3 c1 : COLOR1;
    			float3 vpos : TEXCOORD1;
			};
			
			float scale(float fCos)
			{
				float x = 1.0 - fCos;
				return 0.25 * exp(-0.00287 + x*(0.459 + x*(3.83 + x*(-6.80 + x*5.25))));
			}
			
			v2f vert(appdata_base v)
			{
			
				float3 v3CameraPos = _WorldSpaceCameraPos - _PlanetPos;		// The camera's current position
				float fCameraHeight = length(v3CameraPos);			// The camera's current height
				//float fCameraHeight2 = fCameraHeight*fCameraHeight;	// fCameraHeight^2
			
				// Get the ray from the camera to the vertex and its length (which is the far point of the ray passing through the atmosphere)
				float3 v3Pos = (mul(_Object2World, v.vertex).xyz) - _PlanetPos;
				float3 v3Ray = (normalize(v3Pos) * fInnerRadius) - v3CameraPos;
				v3Pos = normalize(v3Pos);
				float fFar = length(v3Ray);
				v3Ray /= fFar;
				
				// Calculate the ray's starting position, then calculate its scattering offset
				float3 v3Start = v3CameraPos;
				float fDepth = exp((fInnerRadius - fCameraHeight) * (1.0/fScaleDepth));
				float fCameraAngle = dot(-v3Ray, v3Pos);
				float fLightAngle = dot(v3LightPos, v3Pos);
				float fCameraScale = scale(fCameraAngle);
				float fLightScale = scale(fLightAngle);
				float fCameraOffset = fDepth*fCameraScale;
				float fTemp = (fLightScale + fCameraScale);
				
				const float fSamples = 2.0;
			
				// Initialize the scattering loop variables
				float fSampleLength = fFar / fSamples;
				float fScaledLength = fSampleLength * fScale;
				float3 v3SampleRay = v3Ray * fSampleLength;
				float3 v3SamplePoint = v3Start + v3SampleRay * 0.5;
				
				// Now loop through the sample rays
				float3 v3FrontColor = float3(0.0, 0.0, 0.0);
				float3 v3Attenuate;
				for(int i=0; i<int(fSamples); i++)
				{
					float fHeight = length(v3SamplePoint);
					float fDepth = exp(fScaleOverScaleDepth * (fInnerRadius - fHeight));
					float fScatter = fDepth*fTemp - fCameraOffset;
					v3Attenuate = exp(-fScatter * (v3InvWavelength * fKr4PI + fKm4PI));
					v3FrontColor += v3Attenuate * (fDepth * fScaledLength);
					v3SamplePoint += v3SampleRay;
				}
			
    			v2f OUT;
    			OUT.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    			OUT.uv = v.texcoord.xy;
    			
				OUT.c0.rgb = v3FrontColor * (v3InvWavelength * fKrESun + fKmESun);
				OUT.c1.rgb = v3Attenuate;
				OUT.vpos = v3Pos;
							
    			return OUT;
			}
			
			half4 frag(v2f IN) : COLOR
			{
				_BlendThreshold = 0.3;
				float3 vertp = IN.vpos.xyz;
				float _length = length(vertp);
			    float disp = (_length - fInnerRadius)/fInnerRadius;
			    half3 texel = tex2D(_DetailTex, IN.uv).rgb;
			    half3 lowercol = float3(0.2,0.2,0.6);
			    half3 uppercol = float3(0.6,0.6,0.6);
			    half3 highercol = float3(1,1,1);
			    
			      _ChangePoint = fInnerRadius * (1 + (CoastalMax));
			      lowercol = float3(0.2,0.2,0.6);
			      uppercol = float3(0.0,0.2,0.0);
			    
			  
			   
			    	
			    
			    
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
				half3 texel2 = tex2D(_DetailTex, IN.uv).rgb;
				texel = texel + texel2;
				
				float3 v4DiffuseColor = float3(0.1,0.4,0.1); //green color
					
				float3 col = IN.c0 + 0.25 * IN.c1;
					
				return half4(texel+col,1.0);
			}
			
			ENDCG

    	}
	}
}
