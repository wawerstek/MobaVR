using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR.Inputs
{
    public class SyncHoldInteraction : IInputInteraction
    {
        public float HoldTime = 0.5f;
        public float HoldTimeout = 1.0f;

        public float MaxSyncDuration = 0.5f;
        public float SyncTimeout = 1.0f;

        private double m_StartSyncTime = 0f;
        private double m_StartHoldTime = 0f;

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
            Debug.Log("Sync BUTTONS 1: TimesHasExpired " + context.timerHasExpired + ", state = " + context.phase);

            if (context.timerHasExpired)
            {
                if (m_IsStartSync)
                {
                    context.Canceled();
                }

                if (m_IsStartHold)
                {
                }

                //return;
            }
            
            Debug.Log("Sync BUTTONS 2: TimesHasExpired " + context.timerHasExpired + ", state = " + context.phase);

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    float waitingValue = context.ComputeMagnitude();
                    if (waitingValue > 0f && m_IsReleased)
                    {
                        m_IsReleased = false;
                        m_IsStartSync = true;
                        m_StartSyncTime = context.time;
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

                        if (startedValue == 0f)
                        {
                            m_IsStartSync = false;
                            m_IsSync = false;
                            m_IsReleased = true;
                            context.Canceled();
                        }

                        if (syncDeltaTime > MaxSyncDuration)
                        {
                            context.Canceled();
                        }

                        if (startedValue == 1f && syncDeltaTime <= MaxSyncDuration)
                        {
                            m_IsStartSync = false;
                            m_IsSync = true;
                            m_IsStartHold = true;
                            m_StartHoldTime = context.time;
                            context.SetTimeout(HoldTime);
                            //m_IsStartPerformed = true;
                            //context.PerformedAndStayPerformed();
                        }

                        break;
                    }

                    if (m_IsStartHold)
                    {
                        float startedValue = context.ComputeMagnitude();
                        double holdDeltaTime = context.time - m_StartHoldTime;

                        if (startedValue != 0f || holdDeltaTime < HoldTime)
                        {
                            m_IsStartSync = false;
                            m_IsSync = false;
                            m_IsStartHold = false;
                            m_IsHold = false;

                            context.Canceled();
                        }

                        if (startedValue == 1f && holdDeltaTime >= HoldTime)
                        {
                            m_IsStartSync = false;
                            m_IsStartHold = false;
                            m_IsSync = false;
                            m_IsStartHold = false;

                            m_IsHold = true;
                            context.PerformedAndStayPerformed();
                        }

                        break;
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
            
            Debug.Log("Sync BUTTONS 3: TimesHasExpired " + context.timerHasExpired + ", state = " + context.phase);
        }

        private void Clear()
        {
            m_IsStartSync = false;
            m_IsSync = true;
            m_IsStartHold = true;
        }


        public void Reset()
        {
            //m_TimePressed = 0f;
        }
    }
}