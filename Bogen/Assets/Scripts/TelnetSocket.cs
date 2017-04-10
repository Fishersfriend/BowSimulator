﻿using System.Collections;
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

    public Calibration bowCalibration;
    public Bow bow;

    int counter = 0;
    public int setShotDetection = 10;
    public bool voltOut = true;
    public bool Debugging = false;

    public bool conectToBow = false;

    public int pull = 0;
    public int powerInt;
    bool waitforshot = false;
    bool pullCorrect = true;


    // Use this for initialization
    void Start()
    {

        if (conectToBow)
        {
            StartCoroutine(activateBow());
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (conectToBow)
        {
            ReadSocket();
        }

        //ShotBow("Shot 1500");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShotBow("Shot 1500");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            theWriter.WriteLine("enableIR1\n");
            theWriter.Flush();
            Debug.Log("IR 1 aktiv");
            Wait(1);
            theWriter.WriteLine("enableIR2\n");
            theWriter.Flush();
            Debug.Log("IR 2 aktiv");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            theWriter.WriteLine("disableIR1\n");
            theWriter.Flush();
            Debug.Log("IR 1 inaktiv");
            Wait(1);
            theWriter.WriteLine("disableIR2\n");
            theWriter.Flush();
            Debug.Log("IR 2 inaktiv");
        }
    }


    void OnDestroy()
    {
        if (conectToBow)
        {
            theWriter.WriteLine("disableTotal\n");
            theWriter.Flush();
            Debug.Log("disableTotal");

            theWriter.WriteLine("disableShotDetection\n");
            theWriter.Flush();
            Debug.Log("disableShot");

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
                ParseMSG(theReader.ReadLine());

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
            Debug.Log(" Message: " + msg);
        }

        if (msg.StartsWith("Shot ") && waitforshot == false)
        {
            //Debug.Log("shot");
            ShotBow(msg);
        }
        else if (msg.Contains("Total:") && pullCorrect)
        {
            pull = int.Parse(msg.Substring(6));
            if(pull<0)
            {
                pull = 0;
            }
            if(pull > 1500)
            {
                pull = 1500;
            }

            bow.pull = pull;

            //Debug.Log(pull);
        }
        else if (msg.Contains("Calibration"))
        {
            bowCalibration.calibrationStart = true;
        }

        else if (msg.Contains("Volt") && voltOut)
        {
            Debug.Log("data: " + msg);
        }




    }

    private void ShotBow(string Power)
    {
        waitforshot = true;
        //Debug.Log(waitforshot);
        StartCoroutine(WaitforShotPause(1));
        //Debug.Log(Power);
        powerInt = int.Parse(Power.Substring(4));


        if (powerInt >= 100)
        {
            bow.Shoot(powerInt);
            pullCorrect = false;
            pull = 0;
            bow.pull = pull;

            Debug.Log(" Shotpower: " + powerInt);


            int y = Random.Range(1, 4);
            //Debug.Log(y);
            if (y == 1)
            {
                theWriter.WriteLine("shotRed: " + powerInt + "/n");
                theWriter.Flush();
                Debug.Log("RedShot");
                StartCoroutine(Wait(0.01f));
                theWriter.WriteLine("clear /n");
                theWriter.Flush();
            }
            else if (y == 2)
            {
                theWriter.WriteLine("shotBlue: " + powerInt + "/n");
                theWriter.Flush();
                Debug.Log("BlueShot");

            }
            else if (y == 3)
            {
                theWriter.WriteLine("shotGreen: " + powerInt + "/n");
                theWriter.Flush();
                Debug.Log("GreenShot");

            }
            else
            {
                Debug.Log("ErrorColor");
            }
            StartCoroutine(WaitforFlush(0.75f));

        }
    }

    IEnumerator activateBow()
    {
        OpenConnection();

        yield return new WaitForSeconds(0.05f);
        theWriter.WriteLine("enableTotal\n");
        theWriter.Flush();
        Debug.Log("enableTotal");

        yield return new WaitForSeconds(0.01f);
        string setShotDetectionString = "setShotDetection " + setShotDetection.ToString() + "\n";
        theWriter.WriteLine(setShotDetectionString);
        theWriter.Flush();

        yield return new WaitForSeconds(0.01f);
        theWriter.WriteLine("enableShotDetection\n");
        theWriter.Flush();
        Debug.Log("enableShot");


    }

    IEnumerator deactivateBow()
    {
        yield return new WaitForSeconds(0.01f);
        theWriter.WriteLine("disableTotal\n");
        theWriter.Flush();
        Debug.Log("disableTotal");

        yield return new WaitForSeconds(0.01f);
        theWriter.WriteLine("disableShotDetection\n");
        theWriter.Flush();
        Debug.Log("disableShot");

        mySocket.Close();
        theStream.Dispose();
        theWriter.Dispose();
        theReader.Dispose();
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
    IEnumerator WaitforShotPause(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        waitforshot = false;
        //Debug.Log(waitforshot);
    }
    IEnumerator WaitforFlush(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        theWriter.WriteLine("clear/n");
        theWriter.Flush();
        Debug.Log("Flush");

        yield return new WaitForSeconds(0.01f);
        pullCorrect = true;

    }


}