using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaVR
{
    public class SingleWizardPlayer : MonoBehaviour
    {
        protected const string TAG = nameof(WizardPlayer);

        [SerializeField] private BigFireBall m_BigFireballPrefab;
        [SerializeField] private SmallFireBall m_SmallFireballPrefab;

        [Header("Right Hand")]
        [SerializeField] private Grabber m_RightGrabber;
        [SerializeField] private Transform m_RightBigFireballPoint;
        [SerializeField] private Transform m_RightSmallFireballPoint;
        [SerializeField] private InputActionReference m_RightGrabInput;
        [SerializeField] private InputActionReference m_RightActivateInput;

        [Header("Left Hand")]
        [SerializeField] private Grabber m_LeftGrabber;
        [SerializeField] private Transform m_LeftBigFireballPoint;
        [SerializeField] private Transform m_LeftSmallFireballPoint;
        [SerializeField] private InputActionReference m_LeftGrabInput;
        [SerializeField] private InputActionReference m_LeftActivateInput;

        private BigFireBall m_RightBigFireBall;
        private BigFireBall m_LeftBigFireBall;

        private SmallFireBall m_RightSmallFireBall;
        private SmallFireBall m_LeftSmallFireBall;

        private void Awake()
        {
            m_RightGrabInput.action.started += context => { Debug.Log($"{TAG}: RightGrab: started"); };
            m_LeftGrabInput.action.started += context => { Debug.Log($"{TAG}: LeftGrab: started"); };

            m_RightGrabInput.action.performed += context =>
            {
                Debug.Log($"{TAG}: RightGrab: performed");
                CreateBigFireBall(out m_RightBigFireBall, m_RightGrabber);
            };
            m_LeftGrabInput.action.performed += context =>
            {
                Debug.Log($"{TAG}: LeftGrab: performed");
                CreateBigFireBall(out m_LeftBigFireBall, m_LeftGrabber);
            };

            m_RightGrabInput.action.canceled += context =>
            {
                Debug.Log($"{TAG}: RightGrab: canceled");
                ThrowBigFireBall(m_RightBigFireBall);
            };
            m_LeftGrabInput.action.canceled += context =>
            {
                Debug.Log($"{TAG}: LeftGrab: canceled");
                ThrowBigFireBall(m_LeftBigFireBall);
            };

            m_RightActivateInput.action.started += context => { Debug.Log($"{TAG}: RightActivate: started"); };
            m_LeftActivateInput.action.started += context => { Debug.Log($"{TAG}: LeftActivate: started"); };

            m_RightActivateInput.action.performed += context =>
            {
                Debug.Log($"{TAG}: RightActivate: performed");
                ShootFireBall(m_RightBigFireBall, 
                              out m_RightSmallFireBall, 
                              m_RightGrabber,
                              m_RightBigFireballPoint,
                              m_RightSmallFireballPoint,
                              -m_RightGrabber.transform.right);
            };
            m_LeftActivateInput.action.performed += context =>
            {
                Debug.Log($"{TAG}: LeftActivate: performed");
                ShootFireBall(m_LeftBigFireBall, 
                              out m_LeftSmallFireBall, 
                              m_LeftGrabber,
                              m_LeftBigFireballPoint,
                              m_LeftSmallFireballPoint,
                              m_LeftGrabber.transform.right);
            };

            m_RightActivateInput.action.canceled += context => { Debug.Log($"{TAG}: RightActivate: canceled"); };
            m_LeftActivateInput.action.canceled += context => { Debug.Log($"{TAG}: LeftActivate: canceled"); };
        }

        private void ThrowBigFireBall(BigFireBall fireBall)
        {
            if (fireBall != null)
            {
                fireBall.Throw();
                fireBall = null;
            }
        }

        private void ThrowSmallFireBall(SmallFireBall fireBall, Grabber grabber, Vector3 direction)
        {
            if (fireBall != null)
            {
                fireBall.ThrowByDirection(grabber.transform.forward);
                //fireBall.ThrowForce(direction);
                fireBall = null;
            }
        }

        private void ShootFireBall(BigFireBall bigFireBall, 
                                   out SmallFireBall smallFireBall, 
                                   Grabber grabber,
                                   Transform bigFireballPoint,
                                   Transform smallFireballPoint,
                                   Vector3 direction)
        {
            if (bigFireBall != null)
            {
                //m_RightBigFireBall.Grabbable.DropItem(m_RightGrabber, true, true);
                bigFireBall.ThrowByDirection(direction);
            }
            else
            {
                //CreateSmallFireBall(out smallFireBall, grabber);
                CreateSmallFireBall(out smallFireBall, smallFireballPoint);
                ThrowSmallFireBall(smallFireBall, grabber, direction);
            }

            bigFireBall = null;
            smallFireBall = null;
        }

        private void CreateBigFireBall(out BigFireBall fireBall, Transform point)
        {
            fireBall = Instantiate(m_BigFireballPrefab);

            Transform fireBallTransform = fireBall.transform;
            fireBallTransform.parent = point.transform;
            fireBallTransform.localPosition = Vector3.zero;
            fireBallTransform.localRotation = Quaternion.identity;
        }

        private void CreateBigFireBall(out BigFireBall fireBall, Grabber grabber)
        {
            CreateBigFireBall(out fireBall, grabber.transform);
        }

        private void CreateSmallFireBall(out SmallFireBall fireBall, Grabber grabber)
        {
            CreateSmallFireBall(out fireBall, grabber.transform);
        }

        private void CreateSmallFireBall(out SmallFireBall fireBall, Transform point)
        {
            fireBall = Instantiate(m_SmallFireballPrefab);

            Transform fireBallTransform = fireBall.transform;
            fireBallTransform.parent = null;
            fireBallTransform.position = point.transform.position;
            fireBallTransform.rotation = Quaternion.identity;
        }
    }
}