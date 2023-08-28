using MobaVR;
using UnityEditor;
using UnityEngine;

public class CopyPasteRagdoll
{
    [MenuItem("MobaVR/RagDoll/Copy and Paste")]
    public static void CopyAndPaste()
    {
        Transform[] selections = Selection.transforms;
        Transform mainSelection = Selection.activeTransform;
        Rigidbody[] mainRigidbodies = mainSelection.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < selections.Length; i++)
        {
            Transform selection = selections[i];
            if (selection == mainSelection)
            {
                continue;
            }
            
            Rigidbody[] copyRigidbodies = selection.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody copyRigidbody in copyRigidbodies)
            {
                foreach (Rigidbody mainRigidbody in mainRigidbodies)
                {
                    if (mainRigidbody.name.Equals(copyRigidbody.name))
                    {
                        if (!copyRigidbody.TryGetComponent(out HitCollider hitCollider))
                        {
                            copyRigidbody.tag = mainRigidbody.tag;
                            copyRigidbody.gameObject.AddComponent<HitCollider>();
                        }

                        if (copyRigidbody.TryGetComponent(out Collider copyCollider)
                            && mainRigidbody.TryGetComponent(out Collider mainCollider))
                        {
                            EditorUtility.CopySerialized(mainCollider, copyCollider);
                        }
                        
                        if (copyRigidbody.TryGetComponent(out CharacterJoint copyCharacterJoint)
                            && mainRigidbody.TryGetComponent(out CharacterJoint mainCharacterJoint))
                        {
                            Rigidbody connectedBody = copyCharacterJoint.connectedBody;
                            EditorUtility.CopySerialized(mainCharacterJoint, copyCharacterJoint);
                            if (connectedBody != null)
                            {
                                copyCharacterJoint.connectedBody = connectedBody;
                            }
                        }

                        EditorUtility.CopySerialized(mainRigidbody, copyRigidbody);

                        break;
                    }
                }
            }
        }
    }
    

    /*
    [MenuItem("MobaVR/RagDoll/Copy and Paste", true)]
    static bool ValidateCopyPose()
    {
        return Selection.transforms is { Length: > 1 };
    }
    */
}