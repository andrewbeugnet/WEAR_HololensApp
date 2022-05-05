using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_UWP
using ZXing;
#endif

public class BarcodeCam : MonoBehaviour
{

    private Color[] orginalc;       //the colors of the camera data.
    private Color32[] targetColorARR;       //the colors of the camera data.
    private byte[] targetbyte;      //the pixels of the camera image.
    private int W, H, WxH;          //width/height of the camera image
    int byteIndex = 0;
    int framerate = 0;
    string dataText = null;
    WebCamTexture _webTex;
    bool isInit = false;
    int blockWidth = 350;
    public bool decoding = true;
    bool tempDecodeing = false;
    public TextMesh textMeshURL;
    Texture2D testQR;

#if UNITY_UWP
        BarcodeReader barReader;
       
#endif

    // Use this for initialization
    void Start()
    {
        _webTex = new WebCamTexture();
        _webTex.Play();
        //GetComponent<Renderer>().material.mainTexture = _webTex;


#if UNITY_UWP
        barReader = new BarcodeReader ();
		barReader.AutoRotate = true;
		barReader.TryInverted = true;
        barReader.Options.PureBarcode = false;
        barReader.Options.Hints.Add(DecodeHintType.TRY_HARDER, true);
        barReader.Options.PossibleFormats =
          new BarcodeFormat[] { BarcodeFormat.QR_CODE };

#endif
    }



    // Update is called once per frame
    void Update()
    {
        if (!decoding)
        {

            if (framerate++ % 10 == 0)
            {
                if (_webTex.isPlaying)
                {

                    W = _webTex.width;                 // get the image width
                    H = _webTex.height;                // get the image height 

                    if (W < 100 || H < 100)
                    {
                        return;
                    }
                    if (!isInit && W > 100 && H > 100)
                    {

                        blockWidth = (int)((Math.Min(W, H) / 3f) * 2);

                        isInit = true;
                    }

                    if (targetColorARR == null)
                    {
                        targetColorARR = new Color32[blockWidth * blockWidth];

                    }

                    if (targetbyte == null)
                    {
                        // targetbyte = new byte[blockWidth * blockWidth * 3];
                        targetbyte = new byte[W * H * 4];
                    }

                    int posx = ((W - blockWidth) >> 1);//
                    int posy = ((H - blockWidth) >> 1);

                    Debug.Log(W);
                    Debug.Log(H);
                    Debug.Log(posx);
                    Debug.Log(posy);
                    Debug.Log(blockWidth);


                    // orginalc = _webTex.GetPixels(posx, posy, blockWidth, blockWidth);// get the webcam image colors
                    orginalc = _webTex.GetPixels(0, 0, W, H);// get the webcam image colors


                    int pixelPos = 0;
                    for (int i = 0; i < H; i++)
                    {
                        for (int j = 0; j < W; j++)
                        {
                            targetbyte[pixelPos * 4] = (byte)(orginalc[pixelPos].b * 255);
                            targetbyte[pixelPos * 4 + 1] = (byte)(orginalc[pixelPos].g * 255);
                            targetbyte[pixelPos * 4 + 2] = (byte)(orginalc[pixelPos].r * 255);
                            targetbyte[pixelPos * 4 + 3] = (byte)(255);
                            pixelPos++;
                        }
                    }


#if UNITY_UWP
                try
                {
                    // Debug.Log("calling barReader.Decode");
                    Result data;
                    // data = barReader.Decode(targetbyte, blockWidth, blockWidth, RGBLuminanceSource.BitmapFormat.RGB24);
                    // data = barReader.Decode(targetbyte, blockWidth, blockWidth, RGBLuminanceSource.BitmapFormat.RGB24);
                    data = barReader.Decode(targetbyte, W, H, RGBLuminanceSource.BitmapFormat.BGR32);
                    // data = barReader.Decode(targetColorARR, blockWidth, blockWidth);//start decode
                    if (data != null) // if get the result success
                    {
                        decoding = true;    // set the variable is true
                        dataText = data.Text;   // use the variable to save the code result
                    }
                }
                catch (Exception e)
                {
                    decoding = false;
                }

       // 	    if(decoding)
			    //{
				   // // if the status variable is change
				   // if(tempDecodeing != decoding)
				   // {
       //                 textMeshURL.text = dataText ;
       //                 // onQRScanFinished(dataText);//triger the scan finished event;
       //             }
       //             tempDecodeing = decoding;
			    //}

#endif
                }
            }
        }
    }
}