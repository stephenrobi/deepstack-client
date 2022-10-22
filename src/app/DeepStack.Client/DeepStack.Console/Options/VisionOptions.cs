using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeepStack.Console.Options
{

    [Verb("vision", isDefault: false, HelpText = "")]
    internal class VisionOptions
    {

        [Option('u', Required = true)]
        public string BaseUrl { get; set; }



        [Option('f', SetName = "file", Required = true)]
        public string File { get; set; }



        [Option('d', SetName = "directory", Required = true)]
        public string Directory { get; set; }


        [Option(HelpText = "Specifies whether or not the EXIF data with the result.")]
        public bool UpdateExifData { get; set; } = false;

    }
}
