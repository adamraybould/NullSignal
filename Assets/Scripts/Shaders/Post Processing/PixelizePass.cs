using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Subspace.Rendering
{
    public class PixelizePass : ScriptableRenderPass
    {
        private PixelizeFeature.CustomPassSettings m_settings;

        private RenderTargetIdentifier m_colorBuffer;
        private RenderTargetIdentifier m_pixelBuffer;
        private int m_pixelBufferID = Shader.PropertyToID("_PixelBuffer");

        private Material m_material;
        private int m_pixelScreenWidth;
        private int m_pixelScreenHeight;

        public PixelizePass(PixelizeFeature.CustomPassSettings settings)
        {
            this.m_settings = settings;
            this.renderPassEvent = settings.RenderPassEvent;

            if (m_material == null)
                m_material = CoreUtils.CreateEngineMaterial("Hidden/Pixelize");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            m_colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            m_pixelScreenHeight = m_settings.ScreenHeight;
            m_pixelScreenWidth = (int)(m_pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);
            
            m_material.SetVector("_BlockCount", new Vector2(m_pixelScreenWidth, m_pixelScreenHeight));
            m_material.SetVector("_BlockSize", new Vector2(1.0f / m_pixelScreenWidth, 1.0f / m_pixelScreenHeight));
            m_material.SetVector("_HalfBlockSize", new Vector2(0.5f / m_pixelScreenWidth, 0.5f / m_pixelScreenHeight));

            descriptor.width = m_pixelScreenWidth;
            descriptor.height = m_pixelScreenHeight;

            cmd.GetTemporaryRT(m_pixelBufferID, descriptor, FilterMode.Point);
            m_pixelBuffer = new RenderTargetIdentifier(m_pixelBufferID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();

            Blit(cmd, m_colorBuffer, m_pixelBuffer, m_material);
            Blit(cmd, m_pixelBuffer, m_colorBuffer);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null)
                throw new System.ArgumentNullException("cmd");
            
            cmd.ReleaseTemporaryRT(m_pixelBufferID);
        }
    }
}
