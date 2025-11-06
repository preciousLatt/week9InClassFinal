using UnityEngine;

namespace Chapter.Observer
{
    public class AchievementSystem : MonoBehaviour
    {
        private BikeController _bike;
        private float _lastDamageTime;
        private bool _unlocked;
        private const float WINDOW = 10f; // seconds without damage

        private void Start()
        {
            // start counting from engine start
            _lastDamageTime = Time.time;
            _bike.OnHealthChanged += HandleHealthChanged;
        }

        // time without damage
        private void Update()
        {
            if (Time.time - _lastDamageTime >= WINDOW)
            {
                _unlocked = true;
                Debug.Log("Achievement Unlocked: Untouchable");
            }
        }

        private void HandleHealthChanged(float unusedHealth)
        {
            _lastDamageTime = Time.time; // any damage resets the timer
        }

        private void OnDisable()
        {
            _bike.OnHealthChanged -= HandleHealthChanged;
        }
    }
}