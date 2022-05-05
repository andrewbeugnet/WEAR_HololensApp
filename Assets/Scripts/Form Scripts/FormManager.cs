using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    public Form CurrentForm;
    public static Button currentAttribute;
    //public GameObject TempFormButton;
    public GameObject TempFormScrollView;
    public GameObject TextInputPanel;
    //public GameObject currentView;
    public GameObject imageView;

    public InputField dictationOutputText;

    private DateTime Today;
    public static bool ActiveView;

    void Start()
    {
        #if WINDOWS_UWP
            SaveData();
        #endif
        Debug.Log("FormManager Start");
        ActiveView = false;

        Button[] TempFormButtons = TempFormScrollView.GetComponentsInChildren<Button>(true);

        foreach (Button b in TempFormButtons)
            b.onClick.AddListener(() => SetAttribute(b));
            
    }

    void SetAttribute(Button bt)
    {
        FormManager.currentAttribute = bt;
        FormManager.ActiveView = false;
        Debug.Log($"Added {bt.name}? Fm says {FormManager.currentAttribute.name}");
    }

    public void NewForm()
    {
        if (imageView.activeInHierarchy)
            imageView.SetActive(false);

         CurrentForm = new TempForm();//going to the form
        //currentView = TempFormButton;


            

        //Debug.Log($"Current form is now a {CurrentForm.GetType().Name}");

         //if (CurrentForm == null)

        Today = DateTime.Today;
    }

    public void StoreTextInfo(Text t)
    {
        var type = CurrentForm.GetType();
        var field = type.GetField(currentAttribute.name);

        Debug.Log($"Stored {t.text} at {field.Name}");
        field.SetValue(CurrentForm, Convert.ToDouble(t.text.Trim().Trim('.')));




        //Text[] buttonText = TextInputPanel.GetComponentsInChildren<Text>();
        //ChangeButtonText(buttonText[0], t.text);
       
        SaveForm();
        dictationOutputText.text = "";
        TextInputPanel.SetActive(false);

    }

    /*public void StoreYesNo(Toggle t)
    {
        var type = CurrentForm.GetType();
        var field = type.GetField(currentAttribute.name);
        string value = "no";

        if (t.isOn)
            value = "yes";

        Debug.Log($"Stored {value} at {field.Name}");
        field.SetValue(CurrentForm, value);

        Text buttonText = currentAttribute.GetComponentInChildren<Text>();
        ChangeButtonText(buttonText, value);

        SaveForm();
    }*/

    /*   public void StoreOkNot(Toggle t)
       {
           var type = CurrentForm.GetType();
           var field = type.GetField(currentAttribute.name);
           string value = "not okay";

           if (t.isOn)
               value = "ok";

           Debug.Log($"Stored {value} at {field.Name}");
           field.SetValue(CurrentForm, value);

           Text buttonText = currentAttribute.GetComponentInChildren<Text>();
           ChangeButtonText(buttonText, value);

           SaveForm();
       }*/

    public void SaveForm()
    {
        //currentView.SetActive(true);
        ActiveView = true;
       
        if(CurrentForm != null)
        {
            string jsonPath = Path.Combine(Application.persistentDataPath, $"{CurrentForm.GetType().Name}-{Today.Day}-{Today.Month}-{Today.Year}.json");
            string json = JsonUtility.ToJson(CurrentForm, true);
            Debug.Log($"Saving at {jsonPath}");

            using (var fileStream = new FileStream(jsonPath, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
                streamWriter.WriteLine(json);

            CurrentForm.WriteForm(Today);
            //WriteCollection.Save(Path.Combine(Application.persistentDataPath, "FormData.xml"));
            return;
        }
    }

    public void ChangeButtonText(Text t, string value)
    {
        int index = t.text.IndexOf('=');
        if (index > 0)
            t.text = t.text.Substring(0, index) + $"= {value}";
        else
            t.text += $" = {value}";
    }

#if WINDOWS_UWP
    static async void SaveData()
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.KnownFolders.CameraRoll;
        Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("hellO.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
    }
#endif

    public void ChangeImage()
    {
        if(imageView.activeInHierarchy)
        {
            imageView.SetActive(false);
        }
        else
        {
            string imagePath = Path.Combine(Application.persistentDataPath, "capture.jpg");
            byte[] fileData;
            Texture2D tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            var image = imageView.GetComponentInChildren<RawImage>();

            if (File.Exists(imagePath))
            {
                fileData = File.ReadAllBytes(imagePath);
                tex.LoadImage(fileData);
                image.texture = tex;
                imageView.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }
}