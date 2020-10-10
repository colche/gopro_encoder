using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter media directory:");
            var directory = Console.ReadLine();
            if (!directory.EndsWith("\\"))
                directory += ("\\");

            MediaInfo mi = new MediaInfo();
            Console.WriteLine(mi.Option("Info_Version"));
            Console.WriteLine();

            var dir = new DirectoryInfo(directory);
            foreach (FileInfo fi in dir.GetFiles())
            {
                //found gopro filename
                if (fi.Name.StartsWith("GX") && (fi.Extension.Equals(".MP4") || fi.Extension.Equals(".mp4")))
                {
                    Console.WriteLine(fi.Name);
                    mi.Open(fi.FullName);

                    /******************************************************************************************************/
                    /*NOTE: Check the CSV in Reference Directory/folder for full list of possible mi.get() property names */
                    /******************************************************************************************************/
                    Console.WriteLine("Found - " + mi.Get(StreamKind.General, 0, "FileNameExtension"));

                    var width = mi.Get(StreamKind.Video, 0, "Width");
                    var height = mi.Get(StreamKind.Video, 0, "Height");
                    //parse the string "UTC 2018-11-21 15:55:09"
                    DateTime dtCreated = DateTime.Parse(mi.Get(StreamKind.Video, 0, "Encoded_Date").Substring(4));

                    Console.WriteLine("Height: " + height);
                    Console.WriteLine("Width: " + width);
                    Console.WriteLine("Date Created: " + dtCreated);

                    var datestr = mi.Get(StreamKind.Video, 0, "Encoded_Date");

                    var fileName = $"{dir}{dtCreated.ToString("GP_yyyy_MM_dd_HHmmss")}";
                    var extension = "mp4";
                    int? uniqueIndex = null;

                    if (File.Exists($"{fileName}.{extension}")) {
                        uniqueIndex = 0;
                        while (File.Exists($"{fileName}_{uniqueIndex}.{extension}"))
                        {
                            uniqueIndex++;
                        }
                    }

                    var outputFileName = $"{fileName}{(uniqueIndex != null ? $"_{uniqueIndex.ToString()}" : String.Empty)}.{extension}";

                    Console.WriteLine("Output Filename: " + outputFileName);

                    //todo log to file

                    var encodingcodec = " -e x265"; //x265 encoding
                    var quality_preset = " -q 25";  //scale of 25 out of 30
                    //var frame_rate = "";
                    var frame_rate = " --cfr";  //constant frame rate
                    var encoder_preset = " --encoder-preset medium";
                    var psi = new ProcessStartInfo("HandBrakeCLI.exe")
                    {
                        Arguments = "-i " + fi.FullName + " -o " + outputFileName + encodingcodec + encoder_preset// + frame_rate + encoder_preset, 
                    };

                    var process = Process.Start(psi);
                    process.WaitForExit();

                    Console.WriteLine();

                }
            }

            Console.WriteLine("Finished! Press enter to exit.");
            Console.ReadLine();
        }
    }
}
