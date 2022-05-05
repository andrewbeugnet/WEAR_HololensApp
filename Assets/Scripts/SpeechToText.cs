using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

public class SpeechToText : MonoBehaviour, HoloToolkit.Unity.InputModule.IDictationHandler
{
    public InputField dictationOutputText;
//    private new Renderer renderer;
    public GameObject objectToBeManipulated;
    private bool isRecording;
    private void Awake()
    {
//        StartCoroutine(DictationInputManager.StartRecording(objectToBeManipulated, 5f, 10f, 10)); // uncomment to automatically start listening
        //        renderer = GetComponent<Renderer>();
    }

    public void onSubmit()
    {
        isRecording = false;
        StartCoroutine(DictationInputManager.StopRecording());
    }

    public void OnVoiceCommand()
    {
//        renderer = objectToBeManipulated.GetComponent<Renderer>();
        ToggleRecording();
    }
    public void ClearText()
    {
        dictationOutputText.text = "";
    }

    public void OnDictationComplete(DictationEventData eventData)
    {
        dictationOutputText.text = eventData.DictationResult;
    }

    public void OnDictationError(DictationEventData eventData)
    {
        isRecording = false;
        StartCoroutine(DictationInputManager.StopRecording());
    }

    public void OnDictationHypothesis(DictationEventData eventData)
    {
        dictationOutputText.text = eventData.DictationResult;
    }

    public void OnDictationResult(DictationEventData eventData)
    {
        dictationOutputText.text = eventData.DictationResult;
    }

    private void ToggleRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            StartCoroutine(DictationInputManager.StopRecording());
        }
        else
        {
            isRecording = true;
            StartCoroutine(DictationInputManager.StartRecording(objectToBeManipulated, 5f,10f,10));
            // 5 s initial timeout, 10 s timeout in between recording, 10 s of recording
        }
    }
}