using UnityEngine;

public class TransparencyController : MonoBehaviour {
    public GameObject character;
    public GameObject obstacle;
    private Renderer obstacleRenderer;

    void Start() {
        obstacleRenderer = obstacle.GetComponent<Renderer>();
    }

    void Update() {
        float distance = Vector3.Distance(character.transform.position, obstacle.transform.position);

        if (distance < 5.0f) // Adjust this value to set the proximity
        {
            Color color = obstacleRenderer.material.color;
            color.a = Mathf.Lerp(1.0f, 0.2f, (5.0f - distance) / 5.0f); // Adjust transparency
            obstacleRenderer.material.color = color;
        }
    }
}
