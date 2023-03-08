using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DisplayMipLevelPassFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader mipLevelShader = null;

    class CustomRenderPass : ScriptableRenderPass
    {
        public Shader overrideShader { get; set; }

        ShaderTagId SHADER_TAG_ID = new ShaderTagId("UniversalForward");

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var drawSettings = CreateDrawingSettings(SHADER_TAG_ID, ref renderingData, SortingCriteria.CommonOpaque);
            drawSettings.overrideShader = this.overrideShader;
            var filterSettings = new FilteringSettings(RenderQueueRange.opaque, -1);
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filterSettings); 
        }
    }

    CustomRenderPass m_ScriptablePass;
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();

        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        // overrideShader is not supported on 2021.3 or lower.
        m_ScriptablePass.overrideShader = this.mipLevelShader;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


