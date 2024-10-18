using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Canvas uiCanvas;

    private string uiResourcePath = "UI/";

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameObject LoadUI(string uiName) {
        GameObject uiPrefab = Resources.Load<GameObject>(uiResourcePath + uiName);
        if (uiPrefab != null) {
            GameObject uiInstance = Instantiate(uiPrefab, uiCanvas.transform);
            return uiInstance;
        }
        else {
            Debug.LogError("UI Prefab not found: " + uiName);
            return null;
        }
    }

    internal void ShowNoti(Character charater) {

    }


}
