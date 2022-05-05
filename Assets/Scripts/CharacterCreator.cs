using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterCreator : MonoBehaviour {

    //Name Input Field
    public InputField nameField;

    //Character name string
    private string charName;

    public void OnSubmit()
    {
        //Set charname string to text in namefield
        charName = nameField.text;

        //Display character name in console
        Debug.Log("The ambient temperature is" + charName);

    }
}
