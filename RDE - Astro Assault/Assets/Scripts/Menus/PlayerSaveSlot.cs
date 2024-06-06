using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the UI representation of a player save slot
    /// This script handles the display of player details like name, level, and playtime
    /// It also updates the icons based on the player's class type
    /// 
    /// </summary>
    public class PlayerSaveSlot : MonoBehaviour
    {
        #region Variables

        [Header("UI Objects")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI timePlayedText;

        [Header("Character Info")]
        private Button slotButton;

        #endregion

        #region Base Methods

        private void Awake()
        {
            slotButton = GetComponent<Button>();

            if (slotButton == null || nameText == null || timePlayedText == null)
            {
                Debug.LogError("CharacterSaveSlot: One or more UI components are missing");
            }
        }

        #endregion

        #region Slot Methods

        //Populates the save slot with data from the player save file
        public void PopulateSlot(PlayerSaveData playerSaveData)
        {
            nameText.text = playerSaveData.playerName;
            timePlayedText.text = playerSaveData.timeStamp.ToString();
        }

        //Selects the player Slot
        public void SelectSlot()
        {
            SaveGameManager.instance.currentPlayerData = SaveGameManager.instance.playerData.Find(saveData => saveData.playerName == nameText.text);

            SoundFXManager.instance.PlaySound(SoundFXManager.instance.GetHoverMenuSFX());

            slotButton.Select();
        }

        #endregion
    }
}