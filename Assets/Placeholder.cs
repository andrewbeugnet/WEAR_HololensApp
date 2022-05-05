using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placeholder : MonoBehaviour
{
    private Text qr;
    public GameObject formButton;
    public GameObject formView;

    private void Start()
    {
        qr = GetComponent<Text>();
        formButton = GameObject.FindWithTag("formButton");
        formButton.SetActive(false);
        this.OnReset();
    }
  public void OnScan()
    {
    this.qr.text = "scanning";

#if !UNITY_EDITOR
    MediaFrameQrProcessing.Wrappers.ZXingQrCodeScanner.ScanFirstCameraForQrCode(
        result =>
        {
          UnityEngine.WSA.Application.InvokeOnAppThread(() =>
          {
            this.qr.text = result ?? "not found";
            if(result.Equals("test"))
                {
                    formButton.SetActive(true);
                }
          }, 
          false);
        },
        TimeSpan.FromSeconds(30));
#endif
    }
    public void OnRun()
  {
    this.qr.text = "running forever";

#if !UNITY_EDITOR
    MediaFrameQrProcessing.Wrappers.ZXingQrCodeScanner.ScanFirstCameraForQrCode(
        result =>
        {
          UnityEngine.WSA.Application.InvokeOnAppThread(() =>
          {
            this.qr.text = $"Got result {result} at {DateTime.Now}";
          }, 
          false);
        },
        null);
#endif
  }
  public void OnReset()
  {
    this.qr.text = "say scan to start";
    formView = GameObject.FindWithTag("formView");
    if(formView != null)
    {
            formView.SetActive(false);
    }
    if (formButton != null)
    {
        formButton.SetActive(false);
    }
    }
}
