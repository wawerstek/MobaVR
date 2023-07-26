using UnityEngine.InputSystem;

namespace MobaVR.Inputs
{
    /// <summary>
    /// Одновременное нажатие клавиш
    /// </summary>
    public class SyncDelayAnalogInteraction : IInputInteraction
    {
        public float HoldTime = 0.5f;
        public float HoltTimeout = 1.0f;

        public float MaxSyncDuration = 0.5f;
        public float SyncTimeout = 1.0f;

        private double m_StartSyncTime = 0f;
        private float m_StartHoldTime = 0f;

        private bool m_IsStartSync = false;
        private bool m_IsSync = false;
        private bool m_IsStartHold = false;
        private bool m_IsHold = false;

        private bool m_IsReleased = false;
        private float m_LastValue = 0f;
        //private bool m_IsStartPerformed = false;
        //private double m_TimePressed = 0f;

        public void Process(ref InputInteractionContext context)
        {
            if (context.timerHasExpired)
            {
                if (m_IsStartSync)
                {
                    if (m_IsSync)
                    {
                        context.Canceled();
                    }
                }

                if (m_IsStartHold)
                {
                    m_IsStartHold = false;
                    float holdValue = context.ComputeMagnitude();
                    if (holdValue == 1f)
                    {
                        m_IsHold = true;
                        context.PerformedAndStayPerformed();
                    }
                    else
                    {
                        m_IsHold = false;
                        context.Canceled();
                    }
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
                        m_IsStartSync = true;
                        m_StartSyncTime = context.time;
                        //m_TimePressed = context.time;
                        context.Started();
                        context.SetTimeout(SyncTimeout);
                    }

                    if (waitingValue == 0f)
                    {
                        m_IsReleased = true;
                    }

                    break;

                case InputActionPhase.Started:
                    if (m_IsStartSync)
                    {
                        float startedValue = context.ComputeMagnitude();
                        double syncDeltaTime = context.time - m_StartSyncTime;

                        if (syncDeltaTime > MaxSyncDuration)
                        {
                            context.Canceled();
                        }

                        if (startedValue == 1f && syncDeltaTime <= MaxSyncDuration)
                        {
                            m_IsStartSync = false;
                            m_IsSync = true;
                            m_IsStartHold = true;

                            context.SetTimeout(HoldTime);
                            //m_IsStartPerformed = true;
                            //context.PerformedAndStayPerformed();
                        }
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