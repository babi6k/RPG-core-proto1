using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        [SerializeField] TextMeshProUGUI xpTextContainer;
        [SerializeField] TextMeshProUGUI lvlTextContainer;


        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            xpTextContainer.text = Mathf.Round(baseStats.GetExperience().GetFraction() * 100).ToString() + "%";
            GetComponent<Slider>().value = baseStats.GetExperience().GetFraction();
            lvlTextContainer.text = baseStats.GetLevel().ToString();
        }
    }
}
