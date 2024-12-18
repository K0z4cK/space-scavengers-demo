using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceScavengers
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNumberTMP;
        private Button _levelButton;

        public void Init(int levelNumber, bool isOpen, UnityAction<int> onButtonClick)
        {
            _levelNumberTMP.text = (levelNumber + 1).ToString();

            _levelButton = GetComponent<Button>();
            if (!isOpen)
            {
                _levelButton.interactable = false;
                return;
            }

            _levelButton.onClick.AddListener(delegate { onButtonClick?.Invoke(levelNumber); });
        }
    }
}