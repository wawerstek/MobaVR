using MobaVR.Inputs;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class CustomComposites
{
    static CustomComposites()
    {
        Init();
        //InitEditor();
    }

    //[UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Init()
    {
        Debug.Log("CustomComposites INIT");

        InputSystem.RegisterBindingComposite<UniqueButton>();
        InputSystem.RegisterBindingComposite<CorrectAndWrongButtons>();
        InputSystem.RegisterBindingComposite<TwoButtons>();

        InputSystem.RegisterBindingComposite<TwoAnalogButtons>();
        InputSystem.RegisterBindingComposite<ThreeAnalogButtons>();
        InputSystem.RegisterBindingComposite<FourAnalogButtons>();

        InputSystem.RegisterBindingComposite<FourButtons>();
        InputSystem.RegisterBindingComposite<ThreeButtons>();

        
        InputSystem.RegisterInteraction<SyncHoldInteraction>();

        InputSystem.RegisterInteraction<SyncPressInteraction>();
        InputSystem.RegisterInteraction<SyncAnalogInteraction>();

        InputSystem.RegisterInteraction<DelayInteraction>();
        InputSystem.RegisterInteraction<HoldInteraction1>();
    }
}