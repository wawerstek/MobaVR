using UnityEngine.InputSystem;

namespace MobaVR.Inputs
{
    /// <summary>
    /// Одновременное нажатие клавиш
    /// </summary>
    public class SyncAnalogInteraction : IInputInteraction
    {
        public float Duration = 0.5f;
        public float Timeout = 1.0f;

        private bool m_IsReleased = false;
        private float m_LastValue = 0f;
        private double m_TimePressed = 0f;

        public void Process(ref InputInteractionContext context)
        {
            if (context.timerHasExpired)
            {
                context.Canceled();
                return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    float waitingValue = context.ComputeMagnitude();
                    if (waitingValue > 0f && m_IsReleased)
                    {
                        m_IsReleased = false;
                        m_TimePressed = context.time;
                        context.Started();
                        context.SetTimeout(Timeout);
                    }

                    if (waitingValue == 0f)
                    {
                        m_IsReleased = true;
                    }

                    break;

                case InputActionPhase.Started:
                    float startedValue = context.ComputeMagnitude();
                    double deltaTime = context.time - m_TimePressed;

                    if (deltaTime > Duration)
                    {
                        context.Canceled();
                    }

                    if (startedValue == 1f && deltaTime <= Duration)
                    {
                        context.PerformedAndStayPerformed();
                    }

                    break;

                case InputActionPhase.Performed:
                    /*
                    if (!context.ControlIsActuated())
                    {
                        context.Canceled();
                    }
                    */
                    float performedValue = context.ComputeMagnitude();
                    if (performedValue != 1)
                    {
                        context.Canceled();
                    }

                    break;
            }
        }

        public void Reset()
        {
            //m_TimePressed = 0f;
        }
    }
}