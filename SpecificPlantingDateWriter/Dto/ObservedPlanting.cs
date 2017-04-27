using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificPlantingDateWriter.Dto
{
    class ObservedPlanting
    {
        public string CropName { get; set; }
        public string SimLocation { get; set; }
        public DateTime PlantingDate { get; set; }
    }
}
