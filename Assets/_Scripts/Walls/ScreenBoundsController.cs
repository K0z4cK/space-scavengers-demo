using UnityEngine;

namespace SpaceScavengers
{
    public class ScreenBoundsController : MonoBehaviour
    {
        [SerializeField] private ScreenBound _leftBound;
        [SerializeField] private ScreenBound _rightBound;
        [SerializeField] private ScreenBound _topBound;
        [SerializeField] private ScreenBound _botBound;

        private void Awake()
        {
            Camera camera = Camera.main;
            Vector2 screenBounds = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

            _leftBound.transform.position = new Vector2(-screenBounds.x, _leftBound.transform.position.y);
            _rightBound.transform.position = new Vector2(screenBounds.x, _rightBound.transform.position.y);
            _topBound.transform.position = new Vector2(_topBound.transform.position.x, screenBounds.y);
            _botBound.transform.position = new Vector2(_botBound.transform.position.x, -screenBounds.y);

            _leftBound.Init(true, _rightBound.AddToIgnoreList);
            _rightBound.Init(true, _leftBound.AddToIgnoreList);
            _topBound.Init(false, _botBound.AddToIgnoreList);
            _botBound.Init(false, _topBound.AddToIgnoreList);
        }

    }
}