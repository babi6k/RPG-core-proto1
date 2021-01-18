using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject damageTextPrefab = null;
       

        public void Spawn(float damageAmount)
        {
           GameObject instance = Instantiate(damageTextPrefab, transform);
            instance.GetComponent<DamageText>().SetValue(damageAmount);
        }
        
    }
}
