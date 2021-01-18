using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textContainer;
        Health health;
  
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            textContainer.text = health.GetHealthPoints().ToString()+ "/" + health.GetMaxHealthPoints();
            GetComponent<Slider>().value = health.GetFraction();
        }
    }
}
