Shader "ReflectionTextured" {
   Properties {
      _MainTex ("Texture For Diffuse Material Color", 2D) = "white" {} 
      _Color ("Overall Diffuse Color Filter", Color) = (1,1,1,1)
      _SpecColor ("Specular Material Color", Color) = (0,0,0,0) 
      _Shininess ("Shininess", Float) = 0.1
      
      
      
      _radius("Radius", Float) = 50
		_LowlandCutoff("Lowland Cutoff", Float) = -0.000
		_CoastalMax("Coastal Max", Float) = 0.002
		_MidMax("Midland Max", Float) = 0.009
		
		_BaseColor("BaseColor", 2D) = "white" {} 
		_LowlandColor("Lowland Color", 2D) = "red" {} 
		_CoastColor("Coastal Color", 2D) = "blue" {} 
		_HighlandColor("Highland Color", 2D)= "black" {} 
		
		_Color("Light Color", Color) = (1, 1, 1, 1)
		_planetPos("Planet Postion", Vector) = (0,0,0,0)
   }
   SubShader {
      Pass {      
         Tags { "LightMode" = "ForwardBase" } 
            // pass for ambient light and first light source
 
         GLSLPROGRAM
 
         // User-specified properties
         uniform sampler2D _MainTex; 
         uniform vec4 _Color;
         uniform vec4 _SpecColor; 
         uniform float _Shininess;
 
         // The following built-in uniforms (except _LightColor0) 
         // are also defined in "UnityCG.glslinc", 
         // i.e. one could #include "UnityCG.glslinc" 
         uniform vec3 _WorldSpaceCameraPos; 
            // camera position in world space
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
         uniform vec4 _WorldSpaceLightPos0; 
            // direction to or position of light source
         uniform vec4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         varying vec3 diffuseColor; 
            // diffuse Phong lighting computed in the vertex shader
         varying vec3 specularColor; 
            // specular Phong lighting computed in the vertex shader
         varying vec4 textureCoordinates; 
         
         
         
         
         varying vec3 vertpos;
         
         varying float displacement;
         uniform vec4 _planetPos;
         
         uniform sampler2D _BaseColor;
         uniform sampler2D _LowlandColor;
         uniform sampler2D _HighlandColor;
         uniform sampler2D _CoastColor;
 
         #ifdef VERTEX
 
         void main()
         {  
         
           vertpos = (_Object2World * gl_Vertex) - _planetPos;
            
                                                                  
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object; // unity_Scale.w 
               // is unnecessary because we normalize vectors
 
            vec3 normalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            vec3 viewDirection = normalize(vec3(
               vec4(_WorldSpaceCameraPos, 1.0) 
               - modelMatrix * gl_Vertex));
            vec3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = vec3(_WorldSpaceLightPos0 
                  - modelMatrix * gl_Vertex);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            vec3 ambientLighting = 
               vec3(gl_LightModel.ambient) * vec3(_Color);
 
            vec3 diffuseReflection = 
               attenuation * vec3(_LightColor0) * vec3(_Color) 
               * max(0.0, dot(normalDirection, lightDirection));
 
            vec3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = vec3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * vec3(_LightColor0) 
                  * vec3(_SpecColor) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            diffuseColor = ambientLighting + diffuseReflection;
            specularColor = specularReflection;
            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
         
         
         
         uniform float _radius;
		uniform float _LowlandCutoff;
		uniform float _CoastalMax;
		uniform float _MidMax;
		
		
 
         void main()
         {
         
         
         
         	float radius = _radius;
			float _length = length(vertpos);
			float displacement = ((_length - radius)/radius);
			
         
            gl_FragColor = vec4(diffuseColor 
               * vec3(texture2D(_MainTex, vec2(textureCoordinates)))
               + specularColor, 1.0);
               
               
               
               if(displacement < _LowlandCutoff)
			{
				gl_FragColor = vec4(diffuseColor 
               * vec3(texture2D(_LowlandColor, vec2(textureCoordinates)))
               + specularColor, 1.0);
			}
			if(displacement > _LowlandCutoff && displacement < _CoastalMax)
			{
				gl_FragColor = vec4(diffuseColor 
               * vec3(texture2D(_CoastColor, vec2(textureCoordinates)))
               + specularColor, 1.0);
			}
			if(displacement > _CoastalMax && displacement < _MidMax)
			{
				gl_FragColor = vec4(diffuseColor 
               * vec3(texture2D(_BaseColor, vec2(textureCoordinates)))
               + specularColor, 1.0);
			}
			if(displacement > _MidMax)
			{
				gl_FragColor = vec4(diffuseColor 
               * vec3(texture2D(_HighlandColor, vec2(textureCoordinates)))
               + specularColor, 1.0);
			}
         }
 
         #endif
 
         ENDGLSL
      }
 
      Pass {      
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
         GLSLPROGRAM
 
         // User-specified properties
         uniform sampler2D _MainTex; 
         uniform vec4 _Color;
         uniform vec4 _SpecColor; 
         uniform float _Shininess;
 
         // The following built-in uniforms (except _LightColor0) 
         // are also defined in "UnityCG.glslinc", 
         // i.e. one could #include "UnityCG.glslinc" 
         uniform vec3 _WorldSpaceCameraPos; 
            // camera position in world space
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
         uniform vec4 _WorldSpaceLightPos0; 
            // direction to or position of light source
         uniform vec4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         varying vec3 diffuseColor; 
            // diffuse Phong lighting computed in the vertex shader
         varying vec3 specularColor; 
            // specular Phong lighting computed in the vertex shader
         varying vec4 textureCoordinates; 
 
         #ifdef VERTEX
 
         void main()
         {                                
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object; // unity_Scale.w 
               // is unnecessary because we normalize vectors
 
            vec3 normalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            vec3 viewDirection = normalize(vec3(
               vec4(_WorldSpaceCameraPos, 1.0) 
               - modelMatrix * gl_Vertex));
            vec3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = vec3(_WorldSpaceLightPos0 
                  - modelMatrix * gl_Vertex);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            vec3 diffuseReflection = 
               attenuation * vec3(_LightColor0) * vec3(_Color) 
               * max(0.0, dot(normalDirection, lightDirection));
 
            vec3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = vec3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               specularReflection = attenuation * vec3(_LightColor0) 
                  * vec3(_SpecColor) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
 
            diffuseColor = diffuseReflection;
            specularColor = specularReflection;
            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            gl_FragColor = vec4(diffuseColor 
               * vec3(texture2D(_MainTex, vec2(textureCoordinates)))
               + specularColor, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
   } 
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Specular"
}