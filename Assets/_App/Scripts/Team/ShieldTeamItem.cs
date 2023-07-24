using AmazingAssets.AdvancedDissolve;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class ShieldTeamItem : ThemeTeamItem
    {
        [Header("Renderer")]
        [SerializeField] private Renderer m_Renderer;
        [SerializeField] private PhotonView m_PhotonView;

        [SerializeField] private Material m_BlueMaterial;
        [SerializeField] private Material m_TransparentBlueMaterial;
        [SerializeField] private Material m_RedMaterial;
        [SerializeField] private Material m_TransparentRedMaterial;

        [SerializeField] private bool m_UseTransparent = true;
        [SerializeField] private float m_TransparentVale = 0.5f;

        public override void SetTeam(TeamType teamType)
        {
            base.SetTeam(teamType);
            Color color = Color.black;
            Material material = m_RedMaterial;
            switch (teamType)
            {
                case TeamType.RED:
                    color = m_RedTeamThemeSo.MainColor;
                    material = (m_PhotonView.IsMine && m_TransparentRedMaterial != null)
                        ? m_TransparentRedMaterial
                        : m_RedMaterial;
                    break;
                case TeamType.BLUE:
                    color = m_BlueTeamThemeSo.MainColor;
                    material = (m_PhotonView.IsMine && m_TransparentBlueMaterial != null)
                        ? m_TransparentBlueMaterial
                        : m_BlueMaterial;
                    break;
            }

            if (m_Renderer != null)
            {
                Debug.LogError("PHOTON IS MINE 0");

                if (m_PhotonView.IsMine && m_UseTransparent)
                {
                    Debug.LogError("PHOTON IS MINE");

                    material = new Material(material);

                    Color transparentColor = new Color(1f, 1f, 1f, m_TransparentVale);
                    material.color = transparentColor;

                    //material.SetOverrideTag("RenderType", "Transparent");
                    //material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    material.SetFloat("_Mode", 2.0f);
                    Debug.LogError("PHOTON IS MINE 2");
                }

                m_Renderer.material = material;

                AdvancedDissolveProperties.Edge.Base.UpdateLocalProperty(
                    m_Renderer.material,
                    AdvancedDissolveProperties.Edge.Base.Property.Color,
                    color);
            }
        }
    }
}