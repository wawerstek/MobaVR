// Upgrade NOTE: replaced 'defined _USEZUP_ON' with 'defined (_USEZUP_ON)'

// Upgrade NOTE: replaced 'defined _USEZUP_ON' with 'defined (_USEZUP_ON)'



Shader "Tazo/lava_optimized"
{
	Properties
	{
		[Header(Noise)]
		_BaseTex3("Noise(RG)", 2D) = "black" {}
		_nn("Noise Strengh", Range(0, 5)) = 0
	
		
		[Header(Mat)]
		_ALLColor("OverAll Color", Color) = (1,1,1,1)
		_MColor("Tint Color", Color) = (0.5,0.5,0.5,1)
		[HDR]_GColor("Glow Color", Color) = (0.8,0.8,0.5,1)
		_G_width("Glow Width", Range(0, 20)) = 1
		_D("Dissolve", Range(-0.05,1.01)) = 1

		[NoScaleOffset]_MatCap ("MatCap (RG)", 2D) = "white" {}
		_Deflect_flow("Deflect Flow", Range(0, 1)) = 0

	
		


		[Header(Project)]
		[NoScaleOffset]_Project("Project(RGB)", 2D) = "white" {}
		//_controll_x("Controll X", Range(0, 20)) = 0
		_controll_y("Control y", Range(0, 20)) = 0
		_controll_z("Control z", Range(0, 20)) = 0

		_tile("Tile", Range(0.0,10.0)) = 1
		_tileOffsetX("OffsetX", Range(0, 1)) = 1
		_tileOffsetY("OffseeY", Range(0, 1)) = 1
		
		_Noise_flow("Lava Flow(for Y-UP)", Range(-1, 1)) = 0
		_z("Push to 1 for Z-UP", Range(0, 1)) = 0
		[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 0
	}
	
	Subshader
	{
		Tags { "Queue" = "Transparent+1" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
//shell
		Pass
		{
			Cull Back
			ZWrite [_ZWrite]
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 pos	: SV_POSITION;
				float2 cap	: TEXCOORD0;
				float2 uv	: TEXCOORD2;
				float4 uv_object : TEXCOORD1;
				float4 xyz	: TEXCOORD3;
				fixed4 color : COLOR;
			};
		
			
		
			uniform sampler2D _BaseTex3;
			fixed _z;
			float4 _BaseTex3_ST;
			
			float _tile;
	
		
			float _tileOffsetX;
			float _tileOffsetY;
			//fixed _controll_x;
			fixed _controll_y;
			fixed _controll_z;
			fixed  _D;
			fixed _Noise_flow;
			fixed _nn;
			fixed _Deflect_flow;
			v2f vert(appdata v)
			{
				v2f o;
				float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
				worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
				//float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				//float dotProduct = 1 - dot(v.normal, viewDir);
			
				o.cap.xy = worldNorm.xy * 0.5 + 0.5;
				//o.cap.z = saturate(1 - abs(normalize(v.normal.y)));
				//o.cap.w = v.color.a;
				//float3 my_normal = normalize(v.normal);
			
				o.xyz.xyz = abs(v.normal);
 				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.uv = TRANSFORM_TEX(v.texcoord, _BaseTex3);
				o.uv_object = v.vertex*0.5+0.5;
				return o;
			}

			uniform fixed4 _ALLColor;
			uniform fixed4 _MColor;
			uniform fixed4 _GColor;
			uniform fixed _G_width;
			uniform sampler2D _MatCap;
			uniform sampler2D _Project;
	

			float4 frag(v2f i) : SV_Target
			{
				fixed4 mc = tex2D(_MatCap, i.cap.xy);

				float2 offset   = float2(_tileOffsetX, _tileOffsetY);
				

				

			 
				
		

					fixed4 px = tex2D(_Project, _Deflect_flow * mc.rg + offset + i.uv_object.gb * _tile + lerp(  frac(float2(_Noise_flow * _Time.y, 0))  , frac(float2(0, _Noise_flow * _Time.y)),floor(_z)));
					fixed4 py = tex2D(_Project, _Deflect_flow * mc.rg + offset + i.uv_object.rb * _tile + lerp(0, frac(float2(0, _Noise_flow * _Time.y)), floor(_z)));
					fixed4 pz = tex2D(_Project, _Deflect_flow * mc.rg + offset + i.uv_object.rg * _tile + lerp(frac(float2(0, _Noise_flow * _Time.y)),0,floor(_z)));


				
				//fixed4 mt = tex2D(_Project, i.uv );
		
				fixed4 cc =1;
				
				fixed4 qq =saturate( lerp(lerp( px ,py, saturate(_controll_y*(i.xyz.y-0.5)+0.5)),pz, saturate(_controll_z*(i.xyz.z-0.5)+0.5)));
				//fixed2 noise = tex2D(_BaseTex3, i.uv + frac( _Time.y));
				fixed nnoise = tex2D(_BaseTex3, i.uv  /*+ noise*_nn*/).r;
				cc.rgb = qq * _MColor + ((_G_width - _D*3)*(qq.g-0.5)+0.5) * _GColor + nnoise* qq.g;

				cc.a = smoothstep(qq.a, qq.a+0.05, 1-_D)* _MColor.a;
				return cc * _ALLColor;
			}
			ENDCG
		}

		
	}
	
	Fallback "VertexLit"
}
