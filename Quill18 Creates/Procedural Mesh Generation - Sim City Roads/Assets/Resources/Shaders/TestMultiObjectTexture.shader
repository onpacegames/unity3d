Shader "Custom/TestMultiObjectTexture" {
 Properties {
      _MainTex ("Texture Image", 2D) = "white" {} 
      _ObjectPos ("Object Position", Vector) = (0,0,0,0)
      _TextureMidPoint ("Texture Mid Point", Vector) = (0,0,0,0)
      _ObjectSize ("XY size of plane", Vector) = (1,1,1,0)
         // a 2D texture property that we call "_MainTex", which should
         // be labeled "Texture Image" in Unity's user interface.
         // By default we use the built-in texture "white"  
         // (alternatives: "black", "gray" and "bump").
   }
   SubShader {
      Pass {    
         GLSLPROGRAM
 
 		uniform vec4 _Object2World;
 		uniform vec4 _World2Object;
         uniform sampler2D _MainTex;    
         uniform vec4 _TextureMidPoint;
         uniform vec4 _ObjectSize;
            // a uniform variable refering to the property above
            // (in fact, this is just a small integer specifying a 
            // "texture unit", which has the texture image "bound" 
            // to it; Unity takes care of this).
 
         varying vec4 textureCoordinates; 
         
         varying vec4 vertexCoordinates;
         
         uniform vec4 _ObjectPos;
            // the texture coordinates at the vertices,
            // which are interpolated for each fragment
 
         #ifdef VERTEX
 
         void main()
         {
         	vec4 holdercoords;
            //textureCoordinates = gl_MultiTexCoord0;
            holdercoords = (gl_MultiTexCoord0);
            textureCoordinates = holdercoords;
            vertexCoordinates = gl_Vertex * _Object2World;
               // Unity provides default longitude-latitude-like 
               // texture coordinates at all vertices of a 
               // sphere mesh as the attribute "gl_MultiTexCoord0".
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            gl_FragColor = 
               texture2D(_MainTex, (vec2(textureCoordinates)));   
               // look up the color of the texture image specified by 
               // the uniform "_MainTex" at the position specified by 
               // "textureCoordinates.x" and "textureCoordinates.y" 
               // and return it in "gl_FragColor"             
         }
 
         #endif
 
         ENDGLSL
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Unlit/Texture"
}