using System;
using System.Collections.Generic;
using System.Text;

namespace DeepStack.Client
{
    public class DeepStackObject
    {

        public string Label { get; set; }

        public float Confidence { get; set; }

        public int Y_Min { get; set; }

        public int X_Min { get; set; }

        public int Y_Max { get; set; }

        public int X_Max { get; set; }


        public override string ToString()
        {
            return $"{Label}:{(int)Math.Round(Confidence * 100, 0)}%";
        }

    }
}
