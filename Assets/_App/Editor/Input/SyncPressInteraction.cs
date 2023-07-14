using UnityEngine.InputSystem;

namespace MobaVR.Input
{
    /// <summary>
    /// Одновременное нажатие клавиш
    /// </summary>
    public class SyncPressInteraction : IInputInteraction
    {
        public float Duration = 0.5f;
        public float Timeout = 1.0f;

        private bool m_IsFirstPress = false;
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
                    float waitingValue = context.ReadValue<float>();
                    if (waitingValue > 0f)
                    {
                        m_TimePressed = context.time;
                        context.Started();
                        context.SetTimeout(Timeout);
                    }

                    break;

                case InputActionPhase.Started:
                    float startedValue = context.ReadValue<float>();
                    double deltaTime = context.time - m_TimePressed;

                    if (deltaTime <= Duration)
                    {
                        context.Canceled();
                    }

                    if (startedValue == 1f && deltaTime <= Duration)
                    {
                        context.PerformedAndStayPerformed();
                    }

                    //if (startedValue == 0f)
                    if (startedValue != 1f)
                    {
                        context.Canceled();
                    }

                    break;

                case InputActionPhase.Performed:
                    /*
                    if (!context.ControlIsActuated())
                    {
                        context.Canceled();
                    }
                    */
                    float performedValue = context.ReadValue<float>();
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