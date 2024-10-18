using UnityEngine;
using UnityEngine.UI;

public class OffScreenTargetIndicator : MonoBehaviour {
    public Transform character; // Reference to the enemy transform
    public RectTransform canvasRectTransform; // Reference to the canvas RectTransform
    public Camera mainCamera; // Reference to the main camera

    void Update() {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(character.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

        if (isOffScreen) {
            // Enable indicator if enemy is off-screen
            GetComponent<Image>().enabled = true;

            // Calculate direction from player to enemy
            Vector3 dir = (character.position - mainCamera.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Clamp the position to the screen edges
            screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
            screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

            Vector3 clampedScreenPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPos, mainCamera, out clampedScreenPos);
            transform.position = clampedScreenPos;
        }
        else {
            // Disable indicator if enemy is on-screen
            GetComponent<Image>().enabled = false;
        }
    }
}
