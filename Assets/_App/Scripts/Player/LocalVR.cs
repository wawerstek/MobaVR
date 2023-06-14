using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaVR
{

    public class LocalVR : MonoBehaviour
    {
        [Header("DieSkin")]
        public SkinDieRespawn[] skinDieRespawnObjects;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        public void DieLocal()
        {

            // Âûïîëíÿåì ôóíêöèþ Die() íà êàæäîì îáúåêòå â ìàññèâå
            foreach (SkinDieRespawn obj in skinDieRespawnObjects)
            {
                //çàìåíÿåì ìàòåðèàëî íà ïðîçðà÷íûé
                obj.Die();
            }

            //íóæíî îòêëþ÷èòü âîçìîæíîñòü ñòðåëüáû
            //íóæíî çàíåñòè èíôó î ñìåðòè èãðîêà, à òîìó îò êîãî ïðèëåòåë ïîñëåäíèé øàð âíåñòè èíôó î óáèéñòâå

        }

        public void RespawnLocal()
        {
            //Âîçâðàùàòüñÿ âèäèìîñòü ñêèíà
            // Âûïîëíÿåì ôóíêöèþ Die() íà êàæäîì îáúåêòå â ìàññèâå
            foreach (SkinDieRespawn obj in skinDieRespawnObjects)
            {
                //çàìåíÿåì ìàòåðèàëî íà ïðîçðà÷íûé
                obj.Respawn();
            }
            

        }

    }
}