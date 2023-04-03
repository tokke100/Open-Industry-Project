using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using S7.Net;
using System;
using System.Threading.Tasks;
using System.Linq;

public class SiemensPLC : MonoBehaviour
{
    Plc plc = new Plc(CpuType.S71200, "192.168.0.1", 0, 1);
    // Start is called before the first frame update

    public bool con;

    public float readTimer = 0.5f;
    private float t = 0f;
    public bool[] input;
    public bool[] output;

    BitArray biArr = new BitArray(12);
    private byte[] outByte;

    private bool writingData=false;
    private bool readingData=false;
    private bool closing = false;

    public List<String> InputboolVars;

    private Dictionary<string, bool> boolDict = new Dictionary<string, bool>();

    void Start()
    {
        plc.Open();
        t = readTimer;

        foreach(var v in InputboolVars)
        {
            boolDict.Add(v, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if ( t > readTimer) 
        {
            if (!readingData && !closing)
            {
                readingData = true;
                readData();
            }
            outByte = ConvertBoolArrayToByteArray(output);
            if (!writingData && !closing)
            {
                writingData = true;
                writeData(outByte);
            }
 
            t = 0f;
        }
        

    }

    private void OnApplicationQuit()
    {
        closing = true;
        CloseCon();
        Debug.Log("con closed");
    }

    static byte[] ConvertBoolArrayToByteArray(bool[] boolArray)
    {
        int length = boolArray.Length;
        int byteLength = length / 8 + (length % 8 == 0 ? 0 : 1);
        byte[] byteArray = new byte[byteLength];

        for (int i = 0; i < length; i++)
        {
            int byteIndex = i / 8;
            int bitIndex = i % 8;

            if (boolArray[i])
            {
                byteArray[byteIndex] |= (byte)(1 << bitIndex);
            }
        }

        return byteArray;
    }

    async void writeData(byte[] data)
    {
        await Task.Run(() => plc.Write(DataType.DataBlock, 1, 2, outByte));
        writingData = false;

    }

    async void readData()
    {
        await Task.Run(() => { 
            biArr = (BitArray)plc.Read(DataType.DataBlock, 1, 0, VarType.Bit, 12);
            for (var i = 0; i < input.Length; i++)
            {
                if(i < boolDict.Count)
                {
                    boolDict[boolDict.ElementAt(i).Key] = true;
                }
                input[i] = biArr[i];
            }

        });
        readingData = false;
    }

    async void CloseCon()
    {
        await Task.Run(() =>
        {
            while (readingData || writingData)
            {
                //do nothing
            }
            plc.Close();
            Debug.Log("finished writing and reading");
        });
    }
}
