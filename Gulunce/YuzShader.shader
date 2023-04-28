Shader "Custom/YuzShader"
{
    Properties
    {
        _MainTex ("Temel Texture", 2D) = "white" {}
        _MainTex2 ("Alternatif Texture", 2D) = "white" {}
        _RoughTex ("Rougness Texture", 2D) = "white" {}
        _NormalTex ("Normal Texture", 2D) = "white" {}
        _Mixture ("Karisim", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;
        sampler2D _NormalTex;
        sampler2D _RoughTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        float _Mixture;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c1 = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 c2 = tex2D (_MainTex2, IN.uv_MainTex);

            o.Albedo = lerp (c1.rgb, c2.rgb, _Mixture);
            
            // Metallic and smoothness come from slider variables
            o.Smoothness = tex2D (_RoughTex, IN.uv_MainTex);
            o.Alpha = c1.a;

            o.Normal = UnpackNormal( tex2D(_NormalTex, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
