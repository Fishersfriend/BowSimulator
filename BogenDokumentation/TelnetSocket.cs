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
    // Use this for initialization

    void Start () {
        OpenConnection();
	}
	
	// Update is called once per frame
	void Update () {
        ReadSocket();

        if (Input.GetKeyDown(KeyCode.A))
        {
            theWriter.WriteLine("1/1/");
            theWriter.Flush();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            theWriter.WriteLine("1/0/");
            theWriter.Flush();
        }
    }


    void OnDestroy()
    {
        mySocket.Close();
        theStream.Dispose();
        theWriter.Dispose();
        theReader.Dispose();
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

            theWriter.Write("Z");
            theWriter.Flush();
            theWriter.Write("Z");
            theWriter.Flush();
            theWriter.Write("Z");
            theWriter.Flush();

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
        //Debug.Log(mySocket.Connected);

        if (mySocket.Connected)
        {

            if (theStream.DataAvailable)
            {
                ParseMSG(theReader.ReadLine());
                data = true;
            }
            else if (data)
            {
                data = false;
                Debug.Log("nothing to read");
            }
        }

    }

    private void ParseMSG(string msg)
    {
        Debug.Log("data: " + msg);
    }
}