using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Net.Sockets;

public class TelnetSocket : MonoBehaviour
{
    public string IP;
    public int Port;

    public TcpClient mySocket;
    public NetworkStream theStream;
    public StreamWriter theWriter;
    public StreamReader theReader;

    public GameObject arrow;
    public GameObject Bow;
    GameObject newArrow;

    int counter = 0;
    public int setShotDetection = 10;
    public bool voltOut = true;
    public bool colorOut = true;
    public bool irActive = false;
    public bool Debugging = false;

    public bool isShot = false;
    public bool conectToBow = false;
    public bool changeColor = false;

    public int pull = 0;
    public int powerInt;


    // Use this for initialization
    void Start () {

        if (conectToBow)
        {
            OpenConnection();
        }
        //ShotBow("Shot 1500");
    }
	
	// Update is called once per frame
	void Update () {

        if (conectToBow)
        {
            ReadSocket();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShotBow("Shot 1500");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            theWriter.WriteLine("enableTotal\n");
            theWriter.Flush();
            Debug.Log("enableTotal");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            theWriter.WriteLine("disableTotal\n");
            theWriter.Flush();
            Debug.Log("disableTotal");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            string setShotDetectionString = "setShotDetection " + setShotDetection.ToString() + "\n";
            theWriter.WriteLine(setShotDetectionString);
            theWriter.Flush();
            Wait(1);
            theWriter.WriteLine("enableShotDetection\n");
            theWriter.Flush();
            Debug.Log("enableShot");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

            theWriter.WriteLine("disableShotDetection\n");
            theWriter.Flush();
            Debug.Log("disableShot");
        }



        if(Input.GetKeyDown(KeyCode.O))
        {
            theWriter.WriteLine("enableIR1\n");
            theWriter.Flush();
            Debug.Log("IR 1 aktiv");
            Wait(1);
            theWriter.WriteLine("enableIR2\n");
            theWriter.Flush();
            Debug.Log("IR 2 aktiv");
            irActive = true;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            theWriter.WriteLine("disableIR1\n");
            theWriter.Flush();
            Debug.Log("IR 1 inaktiv");
            Wait(1);
            theWriter.WriteLine("disableIR2\n");
            theWriter.Flush();
            Debug.Log("IR 2 inaktiv");
            irActive = false;
        }
    }


    void OnDestroy()
    {
        if (conectToBow)
        {
            mySocket.Close();
            theStream.Dispose();
            theWriter.Dispose();
            theReader.Dispose();
        }
    }

    private void PingSomeone()
    {
        System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();

        int timeout = 300;

        System.Net.IPAddress address = System.Net.IPAddress.Loopback;
        PingReply pr = p.Send(IP, timeout);

        if (pr.Status == IPStatus.Success)
            OpenConnection();

    }

    private void OpenConnection()
    {
        try
        {
            mySocket = new TcpClient(IP, Port);
            theStream = mySocket.GetStream();    
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            /*
            theWriter.Write("Z");
            theWriter.Flush();
            theWriter.Write("Z");
            theWriter.Flush();
            theWriter.Write("Z");
            theWriter.Flush();*/

            Debug.Log("connected");
        }
        catch (System.Exception)
        {
            Debug.Log("not connected");
        }
    }

    bool data = true;
    private void ReadSocket()
    {
        if (mySocket.Connected)
        {
            if (theStream.DataAvailable)
            {          
                //Debug.Log(theReader.ReadLine());
                //ParseMSG(theReader.ReadLine());

                data = true;
            }
            else if (data)
            {
                data = false;
                //Debug.Log("nothing to read");
            }
        }

    }

    private void ParseMSG(string msg)
    {
        //Debug.Log(msg);        //alle Ausgaben

        if (Debugging)
        {
            Debug.Log(Time.time + " Message: " + msg);
        }

        if (msg.StartsWith("Shot "))
        {
            ShotBow(msg);
        }

        if (msg.Contains("Color")&& colorOut)
        {
            Debug.Log("data: " + msg);
        }

        if(msg.Contains("Volt") && voltOut)
        {
            Debug.Log("data: " + msg);
        }

        if(msg.Contains("Total:"))
        {
            pull = int.Parse(msg.Substring(6));
            //Debug.Log(pull);
        }


    }

    private void ShotBow(string Power)
    {

        //Debug.Log(Power);
        powerInt = int.Parse(Power.Substring(4));
        if (powerInt >= 100)
        {
            isShot = true;
            Debug.Log(" Shotpower: " + powerInt);

            //Color


            if (changeColor)
            {
                int y = Random.Range(1, 4);
                //Debug.Log(y);
                if (y == 1)
                {
                    theWriter.WriteLine("shotColor red\n");
                    theWriter.Flush();
                    Debug.Log("Red");
                }
                else if (y == 2)
                {
                    theWriter.WriteLine("shotColor green\n");
                    theWriter.Flush();
                    Debug.Log("Green");
                }
                else if (y == 3)
                {
                    theWriter.WriteLine("shotColor blue\n");
                    theWriter.Flush();
                    Debug.Log("Blue");
                }
                else
                {
                    Debug.Log("ErrorColor");
                }
            }
        }
        pull = 0;

    }
    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}