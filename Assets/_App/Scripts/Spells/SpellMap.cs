using System;

namespace MobaVR
{
    [Serializable]
    public class SpellMap
    {
        public SpellBehaviour SpellBehaviour;
        public int Priority;

        public SpellMap()
        {
        }

        public SpellMap(SpellBehaviour spellBehaviour, int priority)
        {
            SpellBehaviour = spellBehaviour;
            Priority = priority;
        }
    }
}