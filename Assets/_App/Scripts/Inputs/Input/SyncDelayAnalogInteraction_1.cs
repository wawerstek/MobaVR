using UnityEngine.InputSystem;

namespace MobaVR.Inputs
{
    /// <summary>
    /// Одновременное нажатие клавиш
    /// </summary>
    public class SyncDelayAnalogInteraction_1 : IInputInteraction
    {
        public float HoldTime = 0.5f;
        public float HoltTimeout = 1.0f;
        
        public float MaxSyncDuration = 0.5f;
        public float SyncTimeout = 1.0f;

        private float m_StartSyncTime = 0f;
        private bool m_IsStartSync = false;
        private bool m_IsSync = false;
        private bool m_IsStartHold = false;
        private bool m_IsHold = false;
        
        
        private bool m_IsStartPerformed = false;
        private bool m_IsReleased = false;
        private float m_LastValue = 0f;
        private double m_TimePressed = 0f;

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
                    float waitingValue = context.ComputeMagnitude();
                    if (waitingValue > 0f && m_IsReleased)
                    {
                        m_IsReleased = false;
                        m_TimePressed = context.time;
                        context.Started();
                        context.SetTimeout(SyncTimeout);
                    }

                    if (waitingValue == 0f)
                    {
                        m_IsReleased = true;
                    }

                    break;

                case InputActionPhase.Started:
                    float startedValue = context.ComputeMagnitude();
                    double deltaTime = context.time - m_TimePressed;

                    if (deltaTime > MaxSyncDuration)
                    {
                        context.Canceled();
                    }

                    if (startedValue == 1f && deltaTime <= MaxSyncDuration)
                    {
                        m_IsStartPerformed = true;
                        context.SetTimeout(SyncTimeout);
                        //context.PerformedAndStayPerformed();
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