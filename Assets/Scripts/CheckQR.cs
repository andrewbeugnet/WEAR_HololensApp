using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckQR : MonoBehaviour
{
    BarcodeCam bc = new BarcodeCam();
    private Text qr;
    // Start is called before the first frame update
    void Awake()
    {
        qr = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (bc.decoding)
        {
            qr.text = "Sucess!";
        }
        else
        {
            qr.text = "Working...";
        }
    }
}
