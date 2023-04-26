using System.IO;
using UnityEngine;

public class TextSaveLoad
{
    public static void Save(string content, string path)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(content);
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log("Save success, cotent are\n"+reader.ReadToEnd());
        reader.Close();
    }

    public static string Load(string path)
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        var text = reader.ReadToEnd();
        reader.Close();
        return text;
    }
}
