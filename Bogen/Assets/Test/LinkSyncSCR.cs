using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;

public class LinkSyncSCR : MonoBehaviour
{
    public Connector test = new Connector();
    string lastMessage;

    void Start()
    {
        Debug.Log(test.fnConnectResult("192.168.4.1", 23));
        if (test.msg != "")
        {
            Debug.Log(test.msg);
        }

    }
    void Update()
    {
        if (test.strMessage != "JOIN")
        {
            if (test.msg != lastMessage)
            {
                //Debug.Log(test.res);
                lastMessage = test.msg;
                HandleMessage(lastMessage);

            }
        }
        //Debug.Log(test.strMessage);
    }

    void OnApplicationQuit()
    {
        try { test.fnDisconnect(); }
        catch { }
    }

    private void HandleMessage(string msg)
    {
        //Debug.Log(msg);
    }

    public void ShotBow(string Power)
    {

    }

}