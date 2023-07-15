using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{buttonOne}")]
public class UniqueButton : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int button;
    
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        float valueButton = context.ReadValue<float>(button);
        IEnumerator<InputBindingCompositeContext.PartBinding> enumerator = context.controls.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var controlParent = enumerator.Current.control.parent;
        }
          
        
            
        return valueButton;
    }
    
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {} 
}