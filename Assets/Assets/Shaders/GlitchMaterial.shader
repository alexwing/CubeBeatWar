Shader"Alexwing/GlitchMaterial"{
	Properties{
	_MaskTexture("MaskTexture",2D) = "white"{}
	_DistortionX("DistortionX", Range(-1,1)) = 0.5
	_DistortionY("DistortionY", Range(-1,1)) = 0.5
	_Lens("Lens", Range(-10,10)) = 0.5
	_HorizontalLines("Horizontal Lines", Range(0.0,1.0)) = 0.2
	_HorizontalLittleLines("Horizontal Pattern", Range(0.0,60.0)) = 30
	//_Glitch("Glitch", Range(0.0,10.0)) = 2
	//_Shake("Shake", Range(0.0,10.0)) = 2
	_Velocity("Velocity", Range(-10.0,10.0)) = 0.5
	_Vignette("Vignette", Range(-6,6)) = 1
	_VignetteVelocity("Vignette Velocity", Range(-10.0,10.0)) = 0.5
	_Alpha("Alpha", Range(0.0,1.0)) = 1.0
	


	}
		SubShader{
		
			Tags
			{
				"RenderPipeline" = "UniversalPipeline" 
				"LightMode" = "UniversalForward" 				
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Opaque"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			
			Cull Back
			Lighting Off							
			ZWrite Off
			ZTest[unity_GUIZTestMode]		
			Fog { Mode Off }	
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 100
			
/*
			GrabPass
			{
				"_GrabTexture"
			}*/
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
			};

			sampler2D _MaskTexture;
			float4 _MaskTexture_ST;


			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenCoord : TEXCOORD1;
				float4 grabPassUV : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO //Insert
			};
			uniform sampler2D  _GrabTexture;
			float4 _GrabTexture_ST;
			v2f vert(appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v); //Insert
				UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

				o.vertex = UnityObjectToClipPos(v.vertex);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv , _MaskTexture);
				o.grabPassUV = ComputeGrabScreenPos(o.vertex);

				o.screenCoord.xy = ComputeScreenPos(o.vertex);
				return o;
			}

			float _DistortionX;
			float _DistortionY;
			float _Lens;
			float _HorizontalLines;
			//float _Glitch;
		//float _Shake;
			float _Vignette;
			float _HorizontalLittleLines;
			float _Alpha;
			float _Velocity;
			float _VignetteVelocity;
		

			fixed noise(fixed2 p)
			{
				fixed sample = tex2D(_MaskTexture,fixed2(1.,2. * cos(_Time.y)) * _Time.y * _Velocity	 + p * 1.).x;
				sample = mul(sample ,sample);
				return sample;
			}

			fixed onOff(fixed a, fixed b, fixed c)
			{
				return step(c, sin(_Time.y + a * cos(_Time.y * b)));
			}

			fixed ramp(fixed y, fixed start, fixed end)
			{
				fixed inside = step(start,y) - step(end,y);
				fixed fact = (y - start) / (end - start) * inside;
				return (1. - fact) * inside;

			}

			fixed stripes(fixed2 uv)
			{

				return ramp(fmod(uv.y * 4. + _Time.y / 2. + sin(_Time.y + sin(_Time.y * 0.63)),1.),0.5,0.6) * _HorizontalLines;
			}
/*
			fixed4 getVideo(fixed4 uv)
			{
				fixed4 look = uv;
			 	fixed window = 1. / (1. + 20. * (look.y - fmod(_Time.y / 4.,1.)) * (look.y - fmod(_Time.y / 4.,1.)));
			    look.x = look.x + sin(look.y * 10. + _Time.y) / 50. * onOff(4.,4.,.3) * (1. + cos(_Time.y * 80.)) * window * _Shake;
				fixed vShift = 0.4 * onOff(2.,3.,.9) * (sin(_Time.y) * sin(_Time.y * 20.) +
													 (0.5 + 0.1 * sin(_Time.y * 200.) * cos(_Time.y))) * _Glitch;
				look.y = fmod(look.y + vShift, 1.);
				return  look;
			}
*/
			fixed2 screenDistort(fixed2 uv)
			{
				uv -= fixed2(_DistortionX, _DistortionY);
				uv = uv * 1.2 * (1. / 1.2 + 2. * uv.x * uv.x * uv.y * uv.y * _Lens);
				uv += fixed2(_DistortionX, _DistortionY);
				return uv;
			}

			fixed4 frag(v2f i) : SV_Target{
			{
					//fixed4 video  = tex2Dproj(_GrabTexture, i.grabPassUV);
					fixed4 video = tex2Dproj(_GrabTexture, i.grabPassUV);
				fixed2 uv = i.uv;
				uv = screenDistort(uv);
			//	video = getVideo(video);

				
				fixed vigAmt = _Vignette + _Vignette * sin(_Time.y + _VignetteVelocity * cos(_Time.y * _VignetteVelocity));
				fixed vignette = (1. - vigAmt * (uv.y - .5) * (uv.y - .5)) * (1. - vigAmt * (uv.x - .5) * (uv.x - .5));

				video += stripes(uv);
				video += noise(uv * 2.) / 2.;
				video = mul(video ,vignette);
				video = mul(video ,((12. + fmod(uv.y * _HorizontalLittleLines + _Time.y,1.)) / 13.));
				return  video * (1. - _Alpha);
			}
			}ENDCG
		}
	}
}

