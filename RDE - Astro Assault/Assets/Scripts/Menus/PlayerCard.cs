using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    /// <summary>
    /// 
    /// Displays detailed information about a player character in a card format
    /// This class handles the setup and dynamic population of character data including stats and icons
    /// Each player card presents various attributes such as health, energy, heat, and speeds, which are visualized through text and sliders to provide a quick overview of the player's capabilities
    /// 
    /// @TODO:
    /// - Manually link components in inspector
    /// 
    /// </summary>
    public class PlayerCard : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]
        [SerializeField] private PlayerCharacter playerCharacter;

        [Header("Helper Variables")]
        private TextMeshProUGUI playerType;
        private Image playerIcon;
        private Slider moveSpeedSlider;
        private TextMeshProUGUI moveSpeed;
        private Slider attackSpeedSlider;
        private TextMeshProUGUI attackSpeed;
        private Slider healthSlider;
        private TextMeshProUGUI health;
        private Slider healthRegenSlider;
        private TextMeshProUGUI healthRegen;
        private Slider energySlider;
        private TextMeshProUGUI energy;
        private Slider energyRegenSlider;
        private TextMeshProUGUI energyRegen;
        private Slider heatSlider;
        private TextMeshProUGUI heat;
        private Slider heatRegenSlider;
        private TextMeshProUGUI heatRegen;

        #endregion

        #region Base Methods

        private void Awake()
        {
            playerType = transform.Find("Player Class").GetComponent<TextMeshProUGUI>();
            playerIcon = transform.Find("Icon").GetComponent<Image>();

            moveSpeedSlider = transform.Find("Slider Group/Move Speed").GetComponent<Slider>();
            moveSpeed = moveSpeedSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
            attackSpeedSlider = transform.Find("Slider Group/Attack Speed").GetComponent<Slider>();
            attackSpeed = attackSpeedSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();

            healthSlider = transform.Find("Slider Group/Health").GetComponent<Slider>();
            health = healthSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
            healthRegenSlider = transform.Find("Slider Group/Health Regen").GetComponent<Slider>();
            healthRegen = healthRegenSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();

            energySlider = transform.Find("Slider Group/Energy").GetComponent<Slider>();
            energy = energySlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
            energyRegenSlider = transform.Find("Slider Group/Energy Regen").GetComponent<Slider>();
            energyRegen = energyRegenSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();

            heatSlider = transform.Find("Slider Group/Heat").GetComponent<Slider>();
            heat = heatSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
            heatRegenSlider = transform.Find("Slider Group/Heat Regen").GetComponent<Slider>();
            heatRegen = heatRegenSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            playerType.text = playerCharacter.name;
            playerIcon.sprite = playerCharacter.characterIcon;

            moveSpeed.text = playerCharacter.movementSpeed.ToString();
            moveSpeedSlider.value = playerCharacter.movementSpeed;
            attackSpeed.text = playerCharacter.attackSpeed.ToString();
            attackSpeedSlider.value = playerCharacter.attackSpeed;

            health.text = playerCharacter.maxHealth.ToString();
            healthSlider.value = playerCharacter.maxHealth;
            healthRegen.text = playerCharacter.healthRegen.ToString();
            healthRegenSlider.value = playerCharacter.healthRegen;

            energy.text = playerCharacter.maxEnergy.ToString();
            energySlider.value = playerCharacter.maxEnergy;
            energyRegen.text = playerCharacter.energyRegen.ToString();
            energyRegenSlider.value = playerCharacter.energyRegen;

            heat.text = playerCharacter.maxHeat.ToString();
            heatSlider.value = playerCharacter.maxHeat;
            heatRegen.text = playerCharacter.heatRegen.ToString();
            heatRegenSlider.value = playerCharacter.heatRegen;
        }

        #endregion
    }
}