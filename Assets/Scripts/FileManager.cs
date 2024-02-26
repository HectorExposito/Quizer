using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public string fileSpanish="preguntas.tsv";
    public string fileEnglish = "questions.tsv";
    string filePath;
    
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
        if (PlayerPrefs.GetInt("Lenguage")==0)
        {
            filePath = Application.streamingAssetsPath + "/" + fileEnglish;
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/" + fileSpanish;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
