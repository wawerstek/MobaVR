using System.Collections;
using UnityEngine.InputSystem;

namespace MobaVR.Input
{
    /// <summary>
    /// Задержка перед применением
    /// </summary>
    public class DelayInteraction : IInputInteraction
    {
        public float Delay = 0.5f;

        private bool m_IsStartPerformed = false;

        public void Process(ref InputInteractionContext context)
        {
            if (context.timerHasExpired)
            {
                if (m_IsStartPerformed)
                {
                    context.PerformedAndStayPerformed();
                }
                else
                {
                    context.Canceled();
                }

                //return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    float waitingValue = context.ReadValue<float>();
                    if (waitingValue == 1f)
                    {
                        context.Started();
                        context.SetTimeout(Delay);
                        
                        m_IsStartPerformed = true;
                    }

                    break;

                case InputActionPhase.Started:
                    break;

                case InputActionPhase.Performed:
                    m_IsStartPerformed = false;
                    float performedValue = context.ReadValue<float>();
                    if (performedValue != 1)
                    {
                        context.Canceled();
                    }

                    break;
                
                case InputActionPhase.Canceled:
                    if (m_IsStartPerformed)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    
                    break;
            }
        }

        private IEnumerator f()
        {
            yield break;
        }

        public void Reset()
        {
            //m_TimePressed = 0f;
        }
    }
}