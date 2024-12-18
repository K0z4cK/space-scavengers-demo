using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceScavengers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private ButtonHold _asselerateButton;
        [SerializeField] private Button _shootButton;

        public void Init(UnityAction accelerateStart, UnityAction accelerateStop, UnityAction shoot)
        {
            _asselerateButton.OnPressStart += accelerateStart;
            _asselerateButton.OnPressStop += accelerateStop;
            _shootButton.onClick.AddListener(shoot);
        }
    }
}