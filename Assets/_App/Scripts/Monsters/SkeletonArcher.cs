using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class SkeletonArcher : Monster
    {
        [SerializeField] private Arrow m_ArrowPrefab;
        [SerializeField] private Transform m_ArrowPoint;

        [Header("Chance to Miss")]
        [SerializeField] private bool m_UseMissing = true;
        [SerializeField] private float m_StepX = 0.5f;
        [SerializeField] private float m_StepY = 0.5f;
        [SerializeField] private float m_StepZ = 0.5f;

        #region Atack

        public override void StartAttack()
        {
            base.StartAttack();
        }

        public override void CompleteAttack()
        {
            base.CompleteAttack();
        }

        public void SpawnArrow()
        {
            if (m_Wizard != null && m_Wizard.IsLife)
            {
                Vector3 target = m_Wizard.transform.position;
                target.y += 0.2f;
                if (m_UseMissing)
                {
                    target.x += Random.Range(-m_StepX, m_StepX);
                    target.y += Random.Range(-m_StepY, m_StepY);
                    target.z += Random.Range(-m_StepZ, m_StepZ);
                }

                Vector3 direction = (target - m_ArrowPoint.transform.position).normalized;

                //Arrow arrow = Instantiate(m_ArrowPrefab, m_ArrowPoint.position, Quaternion.identity);
                GameObject aGameObject = PhotonNetwork.Instantiate("Monsters/" + m_ArrowPrefab.name, m_ArrowPoint.position,
                                                        Quaternion.identity);

                if (aGameObject.TryGetComponent(out Arrow arrow))
                {
                    arrow.transform.rotation = Quaternion.LookRotation(direction);
                    arrow.Init(direction, Damage);
                }

                //DrawLines(direction, arrow);
            }
        }

        private void DrawLines(Vector3 direction, Arrow arrow)
        {
            Debug.DrawLine(m_ArrowPoint.transform.position, m_ArrowPoint.transform.position + direction * 10, Color.red,
                           Mathf.Infinity);
            Debug.DrawLine(arrow.transform.position, arrow.transform.position + arrow.transform.forward * 10f,
                           Color.blue,
                           Mathf.Infinity);
        }

        #endregion
    }
}