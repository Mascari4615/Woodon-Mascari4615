Shader "Mascari4615/Learn"
{
    Properties
    {
		// _Name ("Display Name", Range(min, max)) = number
		// _Name ("Display Name", Float) = number
		// _Name ("Display Name", Int) = number

		// _Name ("Display Name", Color) = (number, number, number, number)
		// _Name ("Display Name", Vector) = (number, number, number, number)

		// _Name ("Display Name", 2D) = "white" {}
		// _Name ("Display Name", Rect) = "white" {}
		// _Name ("Display Name", Cube) = "white" {}
		// _Name ("Display Name", 3D) = "white" {}

        _Color ("Color", Color) = (1,1,1,1)

		// 보통 텍스쳐를 사용할 때는 _MainTex 라는 이름을 사용
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		// Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		// LOD : Level of Detail
		// 수치가 높을수록 더 높은 퀄리티의 쉐이더를 사용
        LOD 200

		// CGPROGRAM ~ ENDCG
 		// 유니티 자체 스크립트가 아닌 CG 언어를 이용해서 쉐이더를 직접 짜는 부분
        CGPROGRAM

		// 설정 부분, 스니핏

		// sutf : 서피스 함수 (이름 바꾸면 여기서도 바꿔줘야 함)

		// Standard : 표준 라이팅 모델 사용 (물리 기반 라이팅 -> 무겁다)
		// 이펙트 쉐이더는 대부분 서피스 쉐이더 공식에서 생겨나는 자동 생성 코드들도 줄이기 위해 프레그먼트로 작성되어 있다.
		
		// fullforwardshadows : 모든 라이트 타입에 그림자를 적용
		// 있으면 포워드 렌더링 상태일 때, 모든 라이트에게서 그림자를 생성
		// 없으면 가장 가벼운 라이트인 Directional Light 에서만 그림자 생성
		
		// noambient : 주변광 제거

		// 알파 옵션 파라미터
		// 알파 블렌딩 및 알파 테스트는 aplha: 및 aplhatest: 같은 지시어로 제어 된다
		// 알파 블렌딩은 일반적으로 두 가지 종류가 있다
		// 전통적인 알파 블렌딩 (이펙트나 연기처럼 모든 것이 사라져 버리는 알파 블렌딩)
		// 또는 "프리멀티드 블렌딩" Premultied Blender 유리처럼 반투명 표면에서도 반사를 유지할 수 있음 입니다.
		// 반투명을 사용하면 생성된 서피스 쉐이더 코드에 블렌딩 명령이 포함됩니다.
		// 알파 컷아웃 (알파 테스트)을 사용하면 지정된 변수를 기반으로 픽셀 쉐이더에서 잘라버립니다.
		// `alpha`, `alpha:auto` : 일반적인 조명 함수에 대해서는 일반적으로 알파 블렌딩 효과 (`aplha:fade`)로 작동합니다만, 물리 기반 조명 함수에서는 프리멀티드 블렌딩 (`alpha:premul`)으로 작동합니다.
		// alpha:blend : 알파 블렌딩을 가동
		// alpha:fade : 전통적인 반투명 (알파 블렌딩)을 가동
		// alpha:premul : 프리멀티드 블렌딩을 가동
		// alphatest:VariableName : 알파 컷아웃 반투명(알파 테스팅)을 만듭니다. 컷오프되는 값은 VariableName으로 지정될 flaot 변수에 해당하는 수치로 지정됩니다. 적절한 쉐도우 캐스터 패스를 생성하기 위해 addshadow 지시어를 사용할 수도 있습니다.
		// keepalpha : 기본적으로 불투명한 서피스 쉐이더는 출력 구조체의 알파 또는 조명 하수에 의해 반환된 결과와 관계없이 알파 채널에 1.0(흰색)을 사용합니다. 이 옵션을 사용하면 불투명한 표면 쉐이더에서도 조명 함수의 알파 값을 유지할 수 있습니다. 커스텀 블렌딩을 할 때 사용하곤 합니다.

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
		// 쉐이더 모델 3.0 이상에서만 돌아가게 함으로써 더 복잡한 코드를 쓸 수 있게 제한을 풀어주는 기능
        #pragma target 3.0

        sampler2D _MainTex;

		// 엔진으로부터 입력받는 값
        struct Input
        {
            float2 uv_MainTex;

			// 버텍스 컬러
			// 버텍스 컬러 default는 흰색
			// 버텍스가 컬러를 가지고 있기 때문에, 버텍스 사이의 픽셀은 선형 보간을 통해 색을 가짐
			// 버텍스 컬러를 기존 색과 더하거나 곱해서 라이트맵을 베이크 할 수 없는 상황에서 엠비언트 오클루전처럼 사용하거나 저렴한 라이트맵처럼 이용할 수도 있다. (파티클이나 이펙트에서는 많이 쓰인다.)
			// 버텍스 컬러의 각 rgb 값을 이용해 마스킹을 할 수도 있다.
			// 1. lerp(a, b, IN.color.r)
			// 2. d.rgb * IN.color.r + c.rgb * (1 - IN.color.r)
			// float4 color : COLOR;


        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		// SurfaceOutputStandard : unitycg.cginc 에 정의되어 있는 구조체
		// struct SurfaceOutputStandard
		// {
		//     fixed3 Albedo;
		//     fixed3 Normal;
		//     fixed3 Emission;
		//     half Metallic;
		//     half Smoothness;
		//     half Occlusion;
		//     half Alpha;
		// };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			// 텍스쳐가 절반만큼 왼쪽 아래로 이동 (0 ~ 1) -> (0.5 ~ 1.5)
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex + 0.5) * _Color;

            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex + d.r) * _Color;
			// 텍스쳐가 단색이 아니라면?

			// _Time (float4) : 씬이 시작된 이후 경과된 시간
			// (x, y, z, w) = (t / 20, t, 2t, 3t)

			// _SinTime (float4) : 씬이 시작된 이후 경과된 시간의 사인값
			// (x, y, z, w) = (sin(t / 8), sin(t / 4), sin(t / 2), sin(t))

			// _CosTime (float4) : 씬이 시작된 이후 경과된 시간의 코사인값
			// (x, y, z, w) = (cos(t / 8), cos(t / 4), cos(t / 2), cos(t))

			// unity_DeltaTime (float) : 이전 프레임과 현재 프레임 사이의 시간
			// (x, y, z, w) = (dt, 1 / dt, smoothDt, 1 / smoothDt)
            
			o.Albedo = c.rgb;

			// o.Emission = c.rgb * d.rgb; // 두 색을 곱함
			// o.Alpha = c.a * d.a; // 두 알파값을 곱함
			
			// lerp(a, b, t) : a에서 b로 t 비율만큼 이동한 값을 반환
			// 알파 값을 이용해 마스킹을 할 수 있음

			// 그레이 스케일
			// o.Albedo = (c.r + c.g + c.b) / 3.0;
			// o.Albedo = lerp(fixed3(0, 0, 0), c.rgb, 0.5);
			// 그레이 스케일 (RGB to YIQ)
			// o.Albedo = 0.2989 * c.r + 0.5870 * c.g + 0.1140 * c.b;

			// o.Emission = IN.uv_MainTex.x; // 왼쪽에서 오른쪽으로 밝아짐
			// o.Emission = IN.uv_MainTex.y; // 아래에서 위로 밝아짐
			// o.Emission = float3(IN.uv_MainTex.x, IN.uv_MainTex.y, 0); // 대각선으로 밝아짐, 좌측상단은 초록색, 우측하단은 빨간색 (u = red = x, v = green = y)

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"

	// UV = XY = RG
	// 언리얼, DX 에서는 좌측 상단이 0,0
	// 유니티, OpenGL 에서는 좌측 하단이 0,0
	// UV를 피는 것은, 버텍스에 UV 숫자를 입력하는 행위
}
