Shader "Pya/mosaic" {
    Properties {
		[Enum(UnityEngine.Rendering.CullMode)]
        _Cull("Cull", Float) = 0
		[KeywordEnum(mosaic1_normal, mosaic2_average, blur1_normal, blur2_gauss)] _Mode("Type", Float) = 0
		[Header(Mosaic)]
        _Pixelation ("Pixelation Size", Range(1, 1000)) = 70
		[Space (7)]
		[Header(Blur)]
		_BlockSize ("Block Size", Range(1, 1000)) = 10
		_SD ("SD(blur2 only)", Range(1,100)) = 10
    }

    SubShader {
		Tags { 
			"Queue" = "Transparent"
			"RenderType"="Transparent"
		}
		ZWrite Off
		Cull [_Cull]

		//GrabPassでレンダリング結果を取得
		GrabPass {}

		//X方向へのぼかし処理
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _MODE_MOSAIC1_NORMAL _MODE_MOSAIC2_AVERAGE _MODE_BLUR1_NORMAL _MODE_BLUR2_GAUSS
            #include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

			sampler2D _GrabTexture;
            float4 _GrabTexture_ST;
			float4	_GrabTexture_TexelSize;
            float _Pixelation;
			float _BlockSize;
			float _SD;

            v2f vert(appdata v) {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
				fixed4 col;
				float2 uv = i.grabPos.xy / i.grabPos.w; //0～1に変換
				float2 uv_pixel = floor(uv * _Pixelation) /_Pixelation; //ピクセル化


				//-----モザイク1     先頭ピクセルで範囲を塗りつぶす  --------//
				#ifdef _MODE_MOSAIC1_NORMAL
					col = tex2D(_GrabTexture, uv_pixel);

				//-----モザイク2     平均化した色で範囲を塗りつぶす  --------//
				#elif _MODE_MOSAIC2_AVERAGE
					float count=0;
					//X方向にピクセルをずらして色を加算する
					for(int j = 0; j <= floor(1/(_GrabTexture_TexelSize.x * _Pixelation)); j++) {
						col = col + tex2D(_GrabTexture, uv_pixel + float2(j * _GrabTexture_TexelSize.x, 0));
						count++;
					}
					//平均
					col = col / count;

				//-----ぼかし1     周辺ピクセルの色を平均化    -------------//
				#elif _MODE_BLUR1_NORMAL
					float count=0;
					float size=floor(_BlockSize)*0.1;
					for(int j = -size; j <= size; j++) {
						//X方向にピクセルをずらして色を加算する
						col = col + tex2D(_GrabTexture, uv + float2(j * _GrabTexture_TexelSize.x,0));
						count++;
					}
					//平均
					col = col / count;

				//-----ぼかし2      ガウス関数での重み付け    --------------//
				#elif _MODE_BLUR2_GAUSS
					float size=floor(_BlockSize)*0.1;
					float weight_total;
					float var = pow(_SD, 2) * 0.001; //分散

					for(int j = -size; j <= size; j++) {
						float distance = j / size;
						//重み y = -exp(-0.5*x^2/σ^2)
						float weight = exp(-0.5 * pow(distance, 2) / var);
						weight_total = weight_total + weight;
						//X方向にピクセルをずらして重み付けした色を加算する
						col = col + tex2D(_GrabTexture, uv + float2(j * _GrabTexture_TexelSize.x,0)) * weight;
					}
					//正規化
					col = col / weight_total;

				#endif

                return col;
            }

            ENDCG
        }

		//GrabPassで1つ目のPassのレンダリング結果を取得
        GrabPass {}

		//Y方向へのぼかし処理
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _MODE_MOSAIC1_NORMAL _MODE_MOSAIC2_AVERAGE _MODE_BLUR1_NORMAL _MODE_BLUR2_GAUSS
            #include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

			sampler2D _GrabTexture;
            float4 _GrabTexture_ST;
			float4	_GrabTexture_TexelSize;
			float _BlockSize;
			float _SD;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
				fixed4 col;
				float2 uv = i.grabPos.xy / i.grabPos.w; //0～1に変換

				#ifdef _MODE_MOSAIC1_NORMAL
					//何もしない
					col = tex2D(_GrabTexture, uv);
				#elif _MODE_MOSAIC2_AVERAGE
					//何もしない
					col = tex2D(_GrabTexture, uv);
				#elif _MODE_BLUR1_NORMAL
					float count=0;
					float size=floor(_BlockSize)*0.1;
					for(int j = -size; j <= size; j++) {
						//Y方向にピクセルをずらして色を加算する
						col = col + tex2D(_GrabTexture, uv + float2(0,j * abs(_GrabTexture_TexelSize.y)));
						count++;
					}
					col = col / count;
				#elif _MODE_BLUR2_GAUSS
					float size=floor(_BlockSize)*0.1;
					float weight_total;
					float var = pow(_SD, 2) * 0.001;

					for(int j = -size; j <= size; j++) {
						float distance = j / size;
						float weight = exp(-0.5 * pow(distance, 2) / var);
						weight_total = weight_total + weight;
						//Y方向にピクセルをずらして色を加算する
						col = col + tex2D(_GrabTexture, uv + float2(0,j * abs(_GrabTexture_TexelSize.y))) * weight;
					}
					col = col / weight_total;
				#endif
                return col;
            }

            ENDCG
        }
    }

}