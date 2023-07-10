using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{buttonOne}+{buttonTwo}")]
public class TwoButtons : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int buttonOne;

    [InputControl(layout = "Button")]
    public int buttonTwo;
    
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        float valueButton1 = context.ReadValue<float>(buttonOne);
        float valueButton2 = context.ReadValue<float>(buttonTwo);

        return (valueButton1 + valueButton2) / 2f;
    }
    
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {} 
}