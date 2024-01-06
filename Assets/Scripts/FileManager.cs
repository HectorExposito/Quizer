using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private const string path = "C:\\Users\\hecto\\Downloads\\Preguntas.tsv";
    void Start()
    {
        
    }

    public List<string[]> ReadFile()
    {
        List<string[]> questions=new List<string[]>();
        StreamReader reader = new StreamReader(path);
        bool endOfFile = false;
        while(!endOfFile){
            string data = reader.ReadLine();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
