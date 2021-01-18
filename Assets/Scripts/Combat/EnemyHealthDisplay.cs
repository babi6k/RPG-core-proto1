using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{

    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;


        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                //GetComponent<Text>().text = Mathf.Round(fighter.GetTarget().GetPercentage()) + "%";
                GetComponent<Text>().text = fighter.GetTarget().GetHealthPoints().ToString() + "/" + fighter.GetTarget().GetMaxHealthPoints();
            }
        }
    }
}
