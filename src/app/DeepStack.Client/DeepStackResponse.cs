using System;
using System.Collections.Generic;
using System.Text;

namespace DeepStack.Client
{
    public class DeepStackResponse
    {
        public bool Success { get; set; }

        public DeepStackObject[] Predictions { get; set; }

        public override string ToString()
        {
            if (this.Success)
            {
                return string.Join(';', (object[])this.Predictions);
            }
            else
            {
                return "false";
            }
            
        }
    }
}
