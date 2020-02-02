using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
    // Start is called before the first frame update
    Text txtMessage;
    RectTransform rt;

    Transform target;

    void Start() {
        txtMessage = GetComponentInChildren<Text>();
        rt = GetComponent<RectTransform>();
        Hide();
    }
    public void ShowText(string text, Transform target) {
        rt.transform.position = target.transform.position;
        Show();
        this.target = target;
        //rt.transform.position = position;
        //rt.anchoredPosition = Camera.main.WorldToScreenPoint(position);
        txtMessage.text = text;
    }

    public void HideIfSameTarget(Transform target){
        if (this.target == target) {
            Hide();
        }
    }

    public void Hide() {
        SetAllActive(false);
    }

    public void Show() {
        SetAllActive(true);
    }

    private void Update( ) {
        if (target != null) {
            rt.transform.position = target.transform.position;
        }
    }

    private void SetAllActive(bool active) {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(active);
        }
    }
}
