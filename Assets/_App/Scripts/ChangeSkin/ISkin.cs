namespace MobaVR
{
    public interface ISkin
    {
        public void ActivateSkin(TeamType teamType = TeamType.RED);
        public void DeactivateSkin();
    }
}