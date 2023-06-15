using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobaVR
{
    public class Treasure : MonoBehaviour
    {
        //public float Height = 1f;
        [SerializeField] public List<TreasureData> Treasures = new List<TreasureData>();

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void Generate(Vector3 position)
        {
            foreach (var treasureData in Treasures)
            {
                if (Random.value <= treasureData.Chance)
                {
                    //position.y += Height;
                    var treasure = Instantiate(treasureData.TreasurePrefab, transform);
                    //treasure.transform.localPosition = position;
                    treasure.transform.position = transform.position;
                }
            }
        }
    }

    [Serializable]
    public class TreasureData
    {
        public GameObject TreasurePrefab;
        public float Chance;
    }
}