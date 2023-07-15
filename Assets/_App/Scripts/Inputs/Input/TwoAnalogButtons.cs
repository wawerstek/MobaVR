using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{analogOne}+{analogTwo}")]
public class TwoAnalogButtons : InputBindingComposite<Vector2>
{
    public float pressPoint;
    
    [InputControl(name = "Analog One", layout = "Analog")]
    public int analogOne;
    
    [InputControl(name = "Analog Two", layout = "Analog")]
    public int analogTwo;
    
    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        float valueButton1 = context.ReadValue<float>(analogOne);
        float valueButton2 = context.ReadValue<float>(analogTwo);

        return new Vector2(valueButton1, valueButton2);
    }
    
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        Vector2 vector = ReadValue(ref context);
        //float value = vector.magnitude;
        //float value = (vector.x + vector.y) / 2f;
        
        float value1 = vector.x >= pressPoint ? 1f : 0f;
        float value2 = vector.y >= pressPoint ? 1f : 0f;
        
        float value = (value1 + value2) / 2f;
        return value;
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {} 
}