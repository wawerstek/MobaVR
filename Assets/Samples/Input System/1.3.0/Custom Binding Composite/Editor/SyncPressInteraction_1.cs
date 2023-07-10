using UnityEngine.InputSystem;

namespace MobaVR.Input
{
    /// <summary>
    /// Одновременное нажатие клавиш
    /// </summary>
    public class SyncPressInteraction_1 : IInputInteraction
    {
        public float Duration = 0.5f;

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
                    if (m_LastValue == 0f)
                    {
                        m_TimePressed = context.time;
                    }

                    m_LastValue = context.ReadValue<float>();
                    double deltaTime = context.time - m_TimePressed;
                    if (m_LastValue == 1f && deltaTime <= Duration)
                    {
                        context.Started();
                    }

                    break;

                case InputActionPhase.Started:
                    float startedValue = context.ReadValue<float>();
                    if (startedValue == 1f)
                    {
                        context.PerformedAndStayPerformed();
                    }

                    break;
            }
        }

        // Unlike processors, Interactions can be stateful, meaning that you can keep a
        // local state that mutates over time as input is received. The system might
        // invoke the Reset() method to ask Interactions to reset to the local state
        // at certain points.
        public void Reset()
        {
        }
    }
}