
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace SharpConnect
{
    public class Connector
    {
        const int READ_BUFFER_SIZE = 255;
        const int PORT_NUM = 23;
        private TcpClient client;
        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
        public string strMessage = string.Empty;
        public string msg = string.Empty;

        public byte[] byteBuffer = new byte [255];
        int offset = 0;
        int stringPosition = 0;        

        public Connector() { }

        public string fnConnectResult(string sNetIP, int iPORT_NUM)
        {
            try
            {
                // The TcpClient is a subclass of Socket, providing higher level 
                // functionality like streaming.
                client = new TcpClient(sNetIP, PORT_NUM);
                // Start an asynchronous read invoking DoRead to avoid lagging the user
                // interface.
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                // Make sure the window is showing before popping up connection dialog.

                //AttemptLogin(sUserName);
                return "Connection Succeeded";
            }
            catch (Exception ex)
            {
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }

        public void fnDisconnect()
        {
            SendData("DISCONNECT");
        }

        private void DoRead(IAsyncResult ar)
        {

            //Debug.Log("ts");
            int BytesRead;
            try
            {
                // Finish asynchronous read into readBuffer and return number of bytes read.                
                BytesRead = client.GetStream().EndRead(ar);

                if (BytesRead < 1)
                {
                    // if no bytes were read server has close.  
                    msg = "Disconnected";
                    return;
                }

                strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead);


                //schreiben byteBuffer
                string tempstring= "";

                for (int i = 0; i < BytesRead; i++)
                {
                    byteBuffer[i+offset] = readBuffer[i];

                    int ausgabe = i + offset;
                    //Debug.Log("byteBuffer[]: "+ausgabe +"   readBuffer: "+(char)readBuffer[i]+ "   Offset: "+offset);
                    tempstring +=(char) byteBuffer[i];
                }


                //suche nach /n, offset berechnen
                stringPosition = 0;
                for (int i=0; i<255; i++)
                {
                    if ((char)byteBuffer[i] == '\n')
                    {
                        //Debug.Log("Zeichen erkannt");
                        stringPosition = i+1;
                        strMessage = Encoding.ASCII.GetString(byteBuffer, 0, stringPosition);


                        for (int u=0; u < 255-stringPosition; u++)
                        {
                            byteBuffer[u] = byteBuffer[u + stringPosition];
                        }

                        ProcessCommands(strMessage);

                        break;
                    }
                }
                offset = offset + BytesRead - stringPosition;       

                // Start a new asynchronous read into readBuffer.
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                

            }
            catch(System.Exception e)
            {
                Debug.Log(e);
                msg = "DISCOnnected";
            }
        }

        private void ProcessCommands(string strMessage)
        {
            msg = strMessage;
            Debug.Log(msg);


        }

        // Process the command received from the server, and take appropriate action.

        // Use a StreamWriter to send a message to server.
        private void SendData(string data)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.Write(data + "/n");
            writer.Flush();
        }

    }
}