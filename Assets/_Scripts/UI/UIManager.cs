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
        [SerializeField] private ButtonHold _brakeButton;
        [SerializeField] private Button _shootButton;

        public void Init(UnityAction accelerateStart, UnityAction accelerateStop, UnityAction brakeStart, UnityAction brakeStop, UnityAction shoot)
        {
            _asselerateButton.OnPressStart += accelerateStart;
            _asselerateButton.OnPressStop += accelerateStop;
            _brakeButton.OnPressStart += brakeStart;
            _brakeButton.OnPressStop += brakeStop;
            _shootButton.onClick.AddListener(shoot);
        }
    }
}