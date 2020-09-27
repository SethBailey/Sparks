using System.Collections.Generic;
using System.IO;

class Utils
{
    public static List<string[]> LoadCSVFile(string file)
    {
        var fileData = new List<string[]>();
        using (StreamReader sr = new StreamReader(file))
        {
            string currentLine;
            // currentLine will be null when the StreamReader reaches the end of file
            while ((currentLine = sr.ReadLine()) != null)
            {
                fileData.Add( currentLine.Split(","));
            }
        }
        return fileData;
    }

};