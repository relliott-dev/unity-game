using TMPro;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the world space elements for the player, including health, energy, and heat bars
    /// It provides methods to update these bars based on gameplay events
    /// 
    /// </summary>
    public class PlayerUIWorldManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar energyBar;
        [SerializeField] UI_StatBar heatBar;

        private void Start()
        {
            if(nameText == null)
            {
                Debug.LogError("PlayerUIWorldManager: Name Text is not assigned");
            }
            if (healthBar == null)
            {
                Debug.LogError("PlayerUIWorldManager: Health bar is not assigned");
            }

            if (heatBar == null)
            {
                Debug.LogError("PlayerUIWorldManager: Energy bar is not assigned");
            }

            if (energyBar == null)
            {
                Debug.LogError("PlayerUIWorldManager: Heat bar is not assigned");
            }
        }

        public void SetName(string playerName)
        {
            nameText.text = playerName;
        }

        //Sets the health value for the health bar
        public void SetHealthValue(float health)
        {
            healthBar.SetStat(health);
        }

        //Sets the max health value for the health bar
        public void SetMaxHealthValue(float health)
        {
            healthBar.SetMaxStat(health);
        }

        //Sets the energy value for the energy bar
        public void SetEnergyValue(float energy)
        {
            energyBar.SetStat(energy);
        }

        //Sets the max energy value for the energy bar
        public void SetMaxEnergyValue(float energy)
        {
            energyBar.SetMaxStat(energy);
        }

        //Sets the heat value for the heat bar
        public void SetHeatValue(float heat)
        {
            heatBar.SetStat(heat);
        }

        //Sets the max heat value for the heat bar
        public void SetMaxHeatValue(float heat)
        {
            heatBar.SetMaxStat(heat);
        }
    }
}