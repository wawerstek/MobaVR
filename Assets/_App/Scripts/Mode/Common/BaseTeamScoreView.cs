using UnityEngine;

namespace MobaVR
{
    public abstract class BaseTeamScoreView : MonoBehaviour, IViewVisibility
    {
        public abstract void SetScore(int score);
        public abstract void Show();
        public abstract void Hide();
    }
}