using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;

public class ArduinoController : MonoBehaviour
{
    public SerialPort stream = new SerialPort("/dev/tty.usbserial-AD0KBXJ2", 9600);
    public string data;
    private Thread thread;
    private Queue outputQueue;    // From Unity to Arduino
    private Queue inputQueue; // Arduino to Unity

    // Start is called before the first frame update
    void Start()
    {
        stream.ReadTimeout = 50;
        stream.Open();
        StartThread();
        WriteToArduino("PING");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string ReadFromArduino(int timeout = 0)
    {
        stream.ReadTimeout = timeout;
        try
        {
            return stream.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }

    void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    public void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());
        // Creates and starts the thread
        thread = new Thread(ThreadLoop);
        thread.Start();
    }

    public void ThreadLoop()
    {
        // Looping
        while (true)
        {
            // Send to Arduino
            if (outputQueue.Count != 0)
            {
                string command = (string)outputQueue.Dequeue();
                WriteToArduino(command);
            }
            // Read from Arduino
            data = ReadFromArduino(100);
            if (data != null)
            {
                inputQueue.Enqueue(data);
            }
        }
    }

    public void SendToArduino(string command)
    {
        outputQueue.Enqueue(command);
    }
    public string ReadFromArduino()
    {
        if (inputQueue.Count == 0)
            return null;
        return (string)inputQueue.Dequeue();
    }

}
