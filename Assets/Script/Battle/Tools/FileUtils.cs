using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Battle.Tools
{
    class FileUtils
    {

        public static string readJSON(string fileName)
        {
            TextAsset text = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
            return text.text;
        }

        public static List<string> LoadFile(string fileName)
        {
            List<string> json = new List<string>();
            // Handle any problems that might arise when reading the text

            TextAsset text = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
            return TextAssetToList(text);

            //try
            //{
            //    string line;
            //    // Create a new StreamReader, tell it which file to read and what encoding the file
            //    // was saved as
            //    StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            //    // Immediately clean up the reader after this block of code is done.
            //    // You generally use the "using" statement for potentially memory-intensive objects
            //    // instead of relying on garbage collection.
            //    // (Do not confuse this with the using directive for namespace at the 
            //    // beginning of a class!)
            //    using (theReader)
            //    {
            //        // While there's lines left in the text file, do this:
            //        do
            //        {
            //            line = theReader.ReadLine();

            //            if (line != null)
            //            {
            //                // Do whatever you need to do with the text line, it's a string now
            //                // In this example, I split it into arguments based on comma
            //                // deliniators, then send that array to DoStuff()
            //                json.Add(line);
            //            }
            //        }
            //        while (line != null);
            //        // Done reading, close the reader and return true to broadcast success    
            //        theReader.Close();
            //        return json;
            //    }
            //}
            //// If anything broke in the try block, we throw an exception with information
            //// on what didn't work
            //catch (Exception e)
            //{
            //    //Debug.Log(e.Message);
            //    return null;
            //}
        }

        public static List<string> TextAssetToList(TextAsset ta)
        {
            List<string> listToReturn = new List<string>();
            String[] arrayString = ta.text.Split('\n');
            foreach (String line in arrayString)
            {
                listToReturn.Add(line);
            }
            return listToReturn;
        }
    }
}
