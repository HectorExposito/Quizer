using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public string file="preguntas.tsv";
    string filePath;
    void Start()
    {
    }

    public List<string[]> ReadFile()
    {
        List<string[]> questions=new List<string[]>();
        GetPath();
        StreamReader reader = new StreamReader(filePath);
        bool endOfFile = false;
        while(!endOfFile){
            string data = reader.ReadLine();
            Debug.Log(data);
            if (data==null)
            {
                endOfFile = true;
            }
            else
            {
                string[] dataValues = data.Split('\t');
                questions.Add(dataValues);
            }
        }
        return questions;
    }

    private void GetPath()
    {
        filePath= Application.streamingAssetsPath + "/"+file;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
