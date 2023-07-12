using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{correctButton}+{wrongButton}")]
public class CorrectAndWrongButtons : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int correctButton;

    [InputControl(layout = "Button")]
    public int wrongButton;
    
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        float valueWrongButton = context.ReadValue<float>(wrongButton);
        if (valueWrongButton >= 1f)
        {
            return 0f;
        }

        float valueCorrectButton = context.ReadValue<float>(correctButton);
        return valueCorrectButton;
    }
    
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {} 
}