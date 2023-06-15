using UnityEngine;
using UnityEngine.Animations;

namespace MobaVR
{
    public class MonsterStateSMB : StateMachineBehaviour
    {
        public enum UpdateType
        {
            ENTER,
            UPDATE,
            EXIT
        }

        public UpdateType CurrentUpdateType = UpdateType.EXIT;
        public Monster.MonsterState MonsterState;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex, controller);
            if (CurrentUpdateType == UpdateType.ENTER)
            {
                UpdateState(animator, stateInfo, layerIndex);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex, controller);
            if (CurrentUpdateType == UpdateType.UPDATE)
            {
                UpdateState(animator, stateInfo, layerIndex);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (CurrentUpdateType == UpdateType.EXIT)
            {
                UpdateState(animator, stateInfo, layerIndex);
            }
        }

        private void UpdateState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Monster monsterController = animator.GetComponent<Monster>();
            if (monsterController != null)
            {
                monsterController.CurrentState = MonsterState;
            }
        }
    }
}