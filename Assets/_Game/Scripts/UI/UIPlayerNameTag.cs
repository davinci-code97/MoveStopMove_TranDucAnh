using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerNameTag : MonoBehaviour {

    [SerializeField] private TMP_Text playerScore; 
    [SerializeField] private Vector3 offset;

    void Update() {
        transform.position = Vector3.Lerp(transform.position, 
            Camera.main.WorldToScreenPoint(Player.Instance.TF.position + offset), Time.deltaTime * 10f);
    }


}
