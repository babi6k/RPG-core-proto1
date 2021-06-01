using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        // Config Data
        [Tooltip("How far the pickups be scatterd from the dropper.")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] DropLibrary dropLibrary;
        [SerializeField] CoinDrop coinDrop;

        //Constants
        const int ATTEMPTS = 30;
        protected override Vector3 GetDropLocation()
        {
            //for trying more then once to get a position on the navmesh if cant find return transofrm.positon
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();
            int level = baseStats.GetLevel();
            int numberOfDrops = Random.Range(1,level);
            int coinAmount = Random.Range(level * 10, 10 * level * level/4) + 20;
            var newCoin = Instantiate(coinDrop);
            newCoin.DropGold(coinAmount,GetDropLocation());
            List<string> dropsEncounterd = new List<string>();
            for (int i = 0; i< numberOfDrops; i++)
            {
                Drop drop = dropLibrary.GetRandomDrop(level);
                if (drop.item != null)
                {
                    if (dropsEncounterd.Contains(drop.item.GetItemID())) continue;
                    dropsEncounterd.Add(drop.item.GetItemID());
                    DropItem(drop.item,drop.count);
                }
            } 
        }
    }
}
