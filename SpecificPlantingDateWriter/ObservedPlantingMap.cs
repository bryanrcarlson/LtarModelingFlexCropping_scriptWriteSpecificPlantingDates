using CsvHelper.Configuration;
using SpecificPlantingDateWriter.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificPlantingDateWriter
{
    class ObservedPlantingMap : CsvClassMap<ObservedPlanting>
    {
        public ObservedPlantingMap()
        {
            Map(m => m.CropName).Name("Crop");
            Map(m => m.PlantingDate).Name("Planting-Date");
            Map(m => m.SimLocation).Name("Sim-Location");
        }
    }
}
