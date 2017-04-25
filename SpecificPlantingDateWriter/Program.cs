using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificPlantingDateWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            DateWriter dw = new DateWriter();

            dw.WriteDatesToRotationFiles(@"Input/observed_spring_canola_planting_dates.csv","");
        }
    }
}
