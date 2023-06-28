using UnityEngine;

namespace BNG
{
    public class SkinMirrorSettings : MonoBehaviour
    {
        public ControllerBinding RunObuch = ControllerBinding.BButtonDown;

        public GameObject ChageSkinUI;
        private bool ChageSkinBool = false;

        private void Start()
        {
            ChageSkinUI.SetActive(false);
        }

        private void LateUpdate()
        {
            if (InputBridge.Instance.GetControllerBindingValue(RunObuch))
            {
                RunChangeSkin();
            }
        }

        public void RunChangeSkin()
        {
            if (!ChageSkinBool)
            {
                ChageSkinUI.SetActive(true);
                ChageSkinBool = true;
            }
            else
            {
                ChageSkinUI.SetActive(false);
                ChageSkinBool = false;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ChageSkin"))
            {
                RunChangeSkin();

                MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false; // ��������� ��������� MeshRenderer
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ChageSkin"))
            {
                RunChangeSkin();

                MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true; // ��������� ��������� MeshRenderer
                }
            }
        }
    }
}