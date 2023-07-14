using System.ComponentModel;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Scripting;
#if UNITY_EDITOR
using UnityEngine.InputSystem.Editor;
#endif

namespace UnityEngine.InputSystem.Interactions
{
    /// <summary>
    /// Performs the action if the control is pressed and held for at least the
    /// set duration (which defaults to <see cref="InputSettings.defaultHoldTime"/>).
    /// </summary>
    /// <remarks>
    /// The action is started when the control is pressed. If the control is released before the
    /// set <see cref="duration"/>, the action is canceled. As soon as the hold time is reached,
    /// the action performs. The action then stays performed until the control is released, at
    /// which point the action cancels.
    ///
    /// <example>
    /// <code>
    /// // Action that requires A button on gamepad to be held for half a second.
    /// var action = new InputAction(binding: "&lt;Gamepad&gt;/buttonSouth", interactions: "hold(duration=0.5)");
    /// </code>
    /// </example>
    /// </remarks>
    [DisplayName("Hold")]
    public class HoldInteraction1 : IInputInteraction
    {
        /// <summary>
        /// Duration in seconds that the control must be pressed for the hold to register.
        /// </summary>
        /// <remarks>
        /// If this is less than or equal to 0 (the default), <see cref="InputSettings.defaultHoldTime"/> is used.
        ///
        /// Duration is expressed in real time and measured against the timestamps of input events
        /// (<see cref="LowLevel.InputEvent.time"/>) not against game time (<see cref="Time.time"/>).
        /// </remarks>
        public float duration;

        /// <summary>
        /// Magnitude threshold that must be crossed by an actuated control for the control to
        /// be considered pressed.
        /// </summary>
        /// <remarks>
        /// If this is less than or equal to 0 (the default), <see cref="InputSettings.defaultButtonPressPoint"/> is used instead.
        /// </remarks>
        /// <seealso cref="InputControl.EvaluateMagnitude()"/>
        public float pressPoint;

        private float durationOrDefault => duration > 0.0 ? duration : InputSystem.settings.defaultHoldTime;
        private float pressPointOrDefault => 0.5f;

        private double m_TimePressed;

        /// <inheritdoc />
        public void Process(ref InputInteractionContext context)
        {
            Debug.Log("Hold Interaction 1: Process");
            if (context.timerHasExpired)
            {
                Debug.Log("Hold Interaction 1: timerHasExpired");
                context.PerformedAndStayPerformed();
                return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    Debug.Log("Hold Interaction 1: Waiting 1");
                    if (context.ControlIsActuated(pressPointOrDefault))
                    {
                        Debug.Log("Hold Interaction 1: Waiting 2");

                        m_TimePressed = context.time;

                        context.Started();
                        context.SetTimeout(durationOrDefault);
                    }

                    break;

                case InputActionPhase.Started:
                    // If we've reached our hold time threshold, perform the hold.
                    // We do this regardless of what state the control changed to.

                    Debug.Log("Hold Interaction 1: Started 1");

                    if (context.time - m_TimePressed >= durationOrDefault)
                    {
                        Debug.Log("Hold Interaction 1: Started 2");

                        context.PerformedAndStayPerformed();
                    }

                    if (!context.ControlIsActuated())
                    {
                        Debug.Log("Hold Interaction 1: Started 3");

                        // Control is no longer actuated so we're done.
                        context.Canceled();
                    }

                    break;

                case InputActionPhase.Performed:
                    Debug.Log("Hold Interaction 1: Performed 1");

                    if (!context.ControlIsActuated(pressPointOrDefault))
                    {
                        Debug.Log("Hold Interaction 1: Performed 2");

                        context.Canceled();
                    }

                    break;
            }
        }

        /// <inheritdoc />
        public void Reset()
        {
            m_TimePressed = 0;
        }
    }
}