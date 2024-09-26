using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Subspace.Rendering
{
    public class PixelizeFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class CustomPassSettings
        {
            public RenderPassEvent RenderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            public int ScreenHeight = 270;
        }

        [SerializeField] private CustomPassSettings m_settings;
        private PixelizePass m_customPass;

        public override void Create()
        {
            m_customPass = new PixelizePass(m_settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            #if UNITY_EDITOR
                if (renderingData.cameraData.isSceneViewCamera)
                    return;
            #endif
                renderer.EnqueuePass(m_customPass);
        }
    }
}
