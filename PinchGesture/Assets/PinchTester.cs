using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PinchTester : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI outputText;
    [SerializeField] TextMeshProUGUI touchCountText;

    // Update is called once per frame
    void Update()
    {
        outputText.text = "Zoom " + PinchDetection.Instance.CurrentDirection.ToString();
        touchCountText.text = "Extra Touch Count: " + PinchDetection.Instance.OtherTouches.Count;
    }
}
