using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textContainer;
        Mana mana;

        private void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }

        private void Update()
        {
            textContainer.text = mana.GetManaPoints().ToString() + "/" + mana.GetMaxManaPoints();
            GetComponent<Slider>().value = mana.GetFraction();
        }
    }
}
