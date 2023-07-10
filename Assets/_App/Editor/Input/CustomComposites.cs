using MobaVR.Input;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

#if UNITY_EDITOR
[InitializeOnLoad] 
#endif
public class CustomComposites
{
    static CustomComposites()
    {
        InputSystem.RegisterBindingComposite<TwoButtons>();
        InputSystem.RegisterBindingComposite<FourButtons>();
        InputSystem.RegisterBindingComposite<ThreeButtons>();
        
        InputSystem.RegisterInteraction<SyncPressInteraction>();
        InputSystem.RegisterInteraction<DelayInteraction>();
        InputSystem.RegisterInteraction<HoldInteraction1>();
    }
}