
Shader "Alexwing/Squares"
{

	Properties{
		//Properties
		_Color("Spec Color", Color) = (0,0,0,0)
		_Color2("Spec Color", Color) = (0,0,0,0)
		_Speed("Speed", Range(-10,10)) = 0.1
		_Size("Size",  Range(-10,10)) = 0.1
		_AmplitudeX("AmplitudeX", Range(-20.00,20.00)) = 5
		_AmplitudeY("AmplitudeY", Range(-20.00,20.00)) = 5
		_Zoom("Zoom", Range(-20.00,20.00)) = 1
		_Wave("Wave", Range(0,0.1)) = 0.5
		_InternalSquareSize("InternalSquareSize", Range(0,10.00)) = 2

	}

		SubShader
	{
			Tags
			{
				"RenderPipeline" = "UniversalPipeline" 
				"LightMode" = "UniversalForward" 
				"IgnoreProjector" = "True"
				"RenderType" = "Opaque"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

	Pass
	{

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct VertexInput {
			fixed4 vertex : POSITION;
			fixed2 uv : TEXCOORD0;
			fixed4 tangent : TANGENT;
			fixed3 normal : NORMAL;
			UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
			//VertexInput
		};


		struct v2f {
			fixed4 pos : SV_POSITION;
			fixed2 uv : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO //Insert
			//v2f
		};

		//Variables


		fixed4 _Color;
		fixed4 _Color2;
		float _Speed;
		float _Size;
		float _AmplitudeX;
		float _AmplitudeY;
		float _Zoom;
		float _Wave;
		float _InternalSquareSize;

		v2f vert(VertexInput v)
		{
			v2f o;
				UNITY_SETUP_INSTANCE_ID(v); //Insert
				UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert			
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			//add emissive color
			

			//VertexFactory
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{

			fixed aspect = 1 / 1;
			fixed value;
			fixed2 uv = i.uv;
			uv -= fixed2(0.5, 0.5 * aspect);
			fixed rot = radians(45.0); // radians(45.0*sin(_Time.y));
			fixed2 m = fixed2(cos(rot), -sin(rot))*_Zoom;
			uv = m * uv;
			uv += fixed2(0.5, 0.5 * aspect);
			uv.y += 0.5 * (1.0 - aspect);
			fixed2 pos = 10.0 * uv;
			fixed2 rep = frac(pos);
			fixed dist = _InternalSquareSize * min(min(rep.x, 1.0 - rep.x), min(rep.y, 1.0 - rep.y));
			fixed squareDist = length((floor(pos) + fixed2(0.5,0.5)) - fixed2(_AmplitudeX, _AmplitudeY));

			fixed edge = sin(_Time.y - squareDist * 0.5) * 0.5 + 0.5;

			edge = (_Time.y - squareDist * _Size) * _Speed;
			edge = 2.0 * frac(edge * 0.5);
			//value = 2.0*abs(dist-0.5);
			//value = pow(dist, 2.0);
			value = frac(dist * 2.0);
			value = lerp(value, 1.0 - value, step(1.0, edge));
			//value *= 1.0-0.5*edge;
			edge = pow(abs(1.0 - edge), 2.0);

			//edge = abs(1.0-edge);
			value = smoothstep(edge - 0.05, edge, 0.95 * value);


			value += squareDist * _Wave;
			//return fixed3(value,value,value,value);
			return lerp(fixed4(_Color2.r, _Color2.g, _Color2.b,1.0),fixed4(_Color.r, _Color.g, _Color.b,1.0), value);
			///fragColor.a = 0.25*clamp(value, 0.0, 1.0);

		}
		ENDCG
	}
	}
}

