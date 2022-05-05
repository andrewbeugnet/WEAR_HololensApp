using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.IO;
using UnityEngine.Networking;
using System.Threading;
using System.Net.Http;

#if !UNITY_EDITOR
using System.Threading.Tasks;
#endif


public class WebStream : MonoBehaviour {

#if !UNITY_EDITOR
    private bool _useUWP = true;
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
#endif

#if UNITY_EDITOR
    private bool _useUWP = false;
    System.Net.Sockets.TcpClient client;
    private Thread exchangeThread;
#endif

    [HideInInspector]
     public Byte [] JpegData;
     [HideInInspector]
     public string resolution = "480x360";
 //    public string resolution = "1280x720";
 
    private Texture2D texture;
    public bool connected = false;

    public static Stream stream;
    private WebResponse resp;
     public MeshRenderer frame;
    string ip_address_of_pi = "http://192.168.1.165:8081/";


    public void Start() {	// Start the stream
       GetVideo();
     }
 
     public void StopStream(){   // Stops the stream
         stream.Close ();
         resp.Close ();
     }
 
     public void GetVideo(){
        texture = new Texture2D(2, 2);
		// This will change based on the ip address of the pi used for the server
        string host = "192.168.1.165";
        string port = "8081";
        if (_useUWP)
        {
            ConnectUWP(host, port);
        }
        else
        {
            ConnectUnity(host, port);
        }

    }

    #if UNITY_EDITOR
        private void ConnectUWP(string host, string port) { }
#else
        private async void ConnectUWP(string host, string port)    {


                socket = new Windows.Networking.Sockets.StreamSocket();
                Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
                await socket.ConnectAsync(serverHost, port);

                stream = socket.InputStream.AsStreamForRead();


        }
#endif

    private void ConnectUnity(string host, string port)
    {

#if UNITY_EDITOR

        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ip_address_of_pi);
        //resp = req.GetResponse();
        //stream = resp.GetResponseStream();


        client = new System.Net.Sockets.TcpClient(host, Int32.Parse(port));
        stream = client.GetStream();
        if(stream != null)
        {
            connected = true;
        }
#endif

    }



    public void GetFrame (){
  
         Byte [] JpegData = new Byte[505536];
         //while(true) {
             int bytesToRead = FindLength(stream);
             //if (bytesToRead == -1) {
                // yield break;
            // }
 
             int leftToRead=bytesToRead;
 
             while (leftToRead > 0) {
                Debug.Log("Left To Read" + leftToRead);
                 leftToRead -= stream.Read (JpegData, bytesToRead - leftToRead, leftToRead);
                 //yield return null;
             }
 
             MemoryStream ms = new MemoryStream(JpegData, 0, bytesToRead, false, true);
 
             texture.LoadImage (ms.GetBuffer ());
             frame.material.mainTexture = texture;
             frame.material.color = Color.white;
             stream.ReadByte(); // CR after bytes
             stream.ReadByte(); // LF after bytes
         
     }
 
     public int FindLength(Stream stream)  {
         int b;
         string line="";
         int result=-1;
         bool atEOL=false;
         while ((b=stream.ReadByte())!=-1) {
             if (b==10) continue; // ignore LF char
             if (b==13) { // CR
                 if (atEOL) {  // two blank lines means end of header
                     stream.ReadByte(); // eat last LF
                     return result;
                 }
                 if (line.StartsWith("Content-Length:")) {
                     result=Convert.ToInt32(line.Substring("Content-Length:".Length).Trim());
                 } else {
                     line="";
                 }
                 atEOL=true;
             } else {
                 atEOL=false;
                 line+=(char)b;
             }
         }
         return -1;
     }

    void Update()
    {
        //if (stream.CanRead)
        //{

        //    Read the bytes
        //    using (var writer = new MemoryStream())
        //    {
        //        var readBuffer = new byte[client.ReceiveBufferSize];


        //        while (stream.DataAvailable)
        //        {

        //            int numberOfBytesRead = stream.Read(readBuffer, 0, readBuffer.Length);
        //            if (numberOfBytesRead <= 0)
        //            {
        //                break;
        //            }

        //            writer.Write(readBuffer, 0, numberOfBytesRead);


        //        }

        //        if (writer.Length > 0)
        //        {
        //            got whole data in writer
        //            Get the bytes and apply them to the texture
        //            var tex = new Texture2D(0, 0);
        //            tex.LoadImage(writer.ToArray());
        //            frame.material.mainTexture = tex;
        //            frame.material.color = Color.white;

        //        }
        //    }
        //}
        GetFrame();
    }
}