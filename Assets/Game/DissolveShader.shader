Shader "Custom/DissolveShader"
{
    Properties{
        _Diffuse("Diffuse", Color) = (1,1,1,1)
        _DissolveColorA("Dissolve Color A", Color) = (0,1,1,0)
        _DissolveColorB("Dissolve Color B", Color) = (0.3,0.3,0.3,1)
        _MainTex("Base 2D", 2D) = "white"{}
        _DissolveMap("DissolveMap", 2D) = "white"{}
        _DissolveThreshold("DissolveThreshold", Range(0,2)) = 2
        _ColorFactorA("ColorFactorA", Range(0,1)) = 0.7
        _ColorFactorB("ColorFactorB", Range(0,1)) = 0.8
        _DissolveDistance("DissolveDistance", Range(0, 20)) = 14
        _DissolveDistanceFactor("DissolveDistanceFactor", Range(0,3)) = 3
    }
    
    CGINCLUDE
    #include "Lighting.cginc"
    uniform fixed4 _Diffuse;
    uniform fixed4 _DissolveColorA;
    uniform fixed4 _DissolveColorB;
    uniform sampler2D _MainTex;
    uniform float4 _MainTex_ST;
    uniform sampler2D _DissolveMap;
    uniform float _DissolveThreshold;
    uniform float _ColorFactorA;
    uniform float _ColorFactorB;
    uniform float _DissolveDistance;
    uniform float _DissolveDistanceFactor;
    
    struct v2f
    {
        float4 pos : SV_POSITION;
        float3 worldNormal : TEXCOORD0;
        float2 uv : TEXCOORD1;
        float4 screenPos : TEXCOORD2;
        float3 viewDir : TEXCOORD3;
    };
    
    v2f vert(appdata_base v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
        o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
        o.pos = UnityObjectToClipPos(v.vertex);
        o.viewDir = ObjSpaceViewDir(v.vertex);
        //������Ļ����
        o.screenPos = ComputeGrabScreenPos(o.pos);
        return o;
    }
    
    fixed4 frag(v2f i) : SV_Target
    {
        float2 screenPos = i.screenPos.xy / i.screenPos.w;
        //����������ĵ������Ϊһ������ϵ��
        float2 dir = float2(0.5, 0.5) - screenPos;
        float screenSpaceDistance = 0.5 - sqrt(dir.x * dir.x + dir.y * dir.y);
        //����һ�����ص㵽���������Ϊ��һ������ϵ��
        float viewDistance =  max(0,(_DissolveDistance - length(i.viewDir)) / _DissolveDistance) * _DissolveDistanceFactor;
        //����������ϵ����Ϊ�����ܽ��ϵ��
        float disolveFactor = viewDistance * screenSpaceDistance * _DissolveThreshold;
        //����Dissolve Map
        fixed4 dissolveValue = tex2D(_DissolveMap, i.uv);
        //С����ֵ�Ĳ���ֱ��discard
        if (dissolveValue.r < disolveFactor)
        {
            discard;
        }
        //Diffuse + Ambient���ռ���
        fixed3 worldNormal = normalize(i.worldNormal);
        fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
        fixed3 lambert = saturate(dot(worldNormal, worldLightDir));
        fixed3 albedo = lambert * _Diffuse.xyz * _LightColor0.xyz + UNITY_LIGHTMODEL_AMBIENT.xyz;
        fixed3 color = tex2D(_MainTex, i.uv).rgb * albedo;
        //����Ϊ�˱ȽϷ��㣬ֱ����color�����յı�Եlerp��
        float lerpValue = disolveFactor / dissolveValue.r;
        if (lerpValue > _ColorFactorA)
        {
            if (lerpValue > _ColorFactorB)
                return _DissolveColorB;
            return _DissolveColorA;
        }
        return fixed4(color, 1);
    }
    ENDCG
    
    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag    
            ENDCG
        }
    }
    FallBack "Diffuse"
}
