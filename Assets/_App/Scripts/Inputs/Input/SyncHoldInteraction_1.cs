using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR.Inputs
{
    public class SyncHoldInteraction_1 : IInputInteraction
    {
        public float HoldTime = 0.5f;
        //public float HoldTimeout = 1.0f;

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
            if (context.timerHasExpired)
            {
                if (m_IsStartSync)
                {
                    m_IsStartSync = false;
                    m_IsStartHold = false;
                    m_IsHold = false;
                    m_IsSync = false;
                    
                    context.Canceled();
                }

                if (m_IsStartHold)
                {
                }

                //return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    float waitingValue = context.ComputeMagnitude();
                    
                    if (waitingValue == 1f && m_IsReleased)
                    {
                        m_IsReleased = false;
                        
                        m_IsStartSync = false;
                        m_IsStartHold = true;
                        m_IsSync = true;
                        m_IsHold = false;

                        m_StartHoldTime = context.time;
                        context.Started();
                        context.SetTimeout(HoldTime);
                        break;
                    }
                    
                    if (waitingValue > 0f && m_IsReleased)
                    {
                        m_IsReleased = false;
                        
                        m_IsStartSync = true;
                        m_IsStartHold = false;
                        m_IsHold = false;
                        m_IsSync = false;

                        m_StartSyncTime = context.time;
                        context.Started();
                        context.SetTimeout(SyncTimeout);
                        break;
                    }

                    if (waitingValue == 0f)
                    {
                        m_IsStartSync = false;
                        m_IsStartHold = false;
                        m_IsHold = false;
                        m_IsSync = false;

                        m_IsReleased = true;
                        break;
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
                            m_IsStartHold = false;
                            m_IsHold = false;
                            m_IsSync = false;
                            
                            m_IsReleased = true;
                            context.Canceled();
                            break;
                        }

                        if (syncDeltaTime > MaxSyncDuration)
                        {
                            m_IsStartSync = false;
                            m_IsStartHold = false;
                            m_IsHold = false;
                            m_IsSync = false;
                            
                            context.Canceled();
                            break;
                        }

                        if (startedValue == 1f 
                            && syncDeltaTime <= MaxSyncDuration)
                        {
                            m_IsStartSync = false;
                            m_IsStartHold = true;
                            m_IsSync = true;
                            m_IsHold = false;

                            m_StartHoldTime = context.time;
                            context.SetTimeout(HoldTime);
                            //m_IsStartPerformed = true;
                            //context.PerformedAndStayPerformed();
                        }
                    }

                    if (m_IsStartHold)
                    {
                        float startedValue = context.ComputeMagnitude();
                        double holdDeltaTime = context.time - m_StartHoldTime;

                        if (startedValue != 1f)
                        {
                            m_IsStartSync = false;
                            m_IsStartHold = false;
                            m_IsSync = false;
                            m_IsHold = false;

                            context.Canceled();
                            break;
                        }
                        
                        if (startedValue == 1f && holdDeltaTime >= HoldTime)
                        {
                            m_IsStartSync = false;
                            m_IsStartHold = false;
                            m_IsSync = false;
                            m_IsHold = true;
                            
                            context.PerformedAndStayPerformed();
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
                        m_IsStartSync = false;
                        m_IsStartHold = false;
                        m_IsSync = false;
                        m_IsHold = false;
                        
                        context.Canceled();
                    }

                    break;
                
                case InputActionPhase.Canceled:
                    m_IsStartSync = false;
                    m_IsStartHold = false;
                    m_IsSync = false;
                    m_IsHold = false;
                    break;
            }
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