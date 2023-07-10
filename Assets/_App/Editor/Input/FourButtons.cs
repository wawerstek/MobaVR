using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{buttonOne}+{buttonTwo}+{buttonThree}+{buttonFour}")]
public class FourButtons : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int buttonOne;

    [InputControl(layout = "Button")]
    public int buttonTwo;
    
    [InputControl(layout = "Button")]
    public int buttonThree;
    
    [InputControl(layout = "Button")]
    public int buttonFour;
    
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        float valueButton1 = context.ReadValue<float>(buttonOne);
        float valueButton2 = context.ReadValue<float>(buttonTwo);
        float valueButton3 = context.ReadValue<float>(buttonThree);
        float valueButton4 = context.ReadValue<float>(buttonFour);

        return (valueButton1 + valueButton2 + valueButton3 + buttonFour) / 4f;
    }
    
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {} 
}