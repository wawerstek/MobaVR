using MobaVR;
using UnityEngine;

namespace MobaVR
{
    public class BookMenu : MonoBehaviour
    {
        private ClassicGameSession m_ClassicGameSession;

        private void Start()
        {
            m_ClassicGameSession = FindObjectOfType<ClassicGameSession>(true);
        }

        public void LoadRole(string id)
        {
            if (m_ClassicGameSession != null)
            {
                m_ClassicGameSession.SwitchRole(id);
            }
        }
        
        public void SetRedTeam()
        {
            if (m_ClassicGameSession != null)
            {
                m_ClassicGameSession.SetRedTeam();
            }
        }
        
        public void SetBlueTeam()
        {
            if (m_ClassicGameSession != null)
            {
                m_ClassicGameSession.SetBlueTeam();
            }
        }
    }
}