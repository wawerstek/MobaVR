using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MobaVR
{
    public class ClassicMode : GameMode<ClassicMode>
    {
        [SerializeField] private BaseModeView m_ModeView;
        [SerializeField] private BaseEnvironmentMode m_Environment;

        public BaseModeView ModeView => m_ModeView;
        public BaseEnvironmentMode Environment => m_Environment;
        public ZoneManager ZoneManager => m_GameSession != null ? m_GameSession.ZoneManager : null;


        private void Awake()
        {
            InitStateMachine();
        }

        public override void InitStateMachine()
        {
            m_StateMachine.Init(this);
        }
    }
}