using CsvHelper;
using IniParser;
using IniParser.Model;
using SpecificPlantingDateWriter.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificPlantingDateWriter
{
    // CsvHelper: https://joshclose.github.io/CsvHelper/
    // ini-parser: https://github.com/rickyah/ini-parser

    class DateWriter
    {
        private readonly string creationDate;

        private readonly Dictionary<string, string> cropToRotation = new Dictionary<string, string>
        {
            {"spring canola","sC-wW" },
            {"spring pea","sP-wW" },
            {"spring wheat", "sW-wW" }
        };
        public DateWriter(string creationDate = "17-04-25")
        {
            // Directory structure: {creation date}/{rotation}/{location}/{period}
            this.creationDate = creationDate;
        }

        public void WriteDatesToRotationFiles(
            string pathToPlantingDatesFile,
            string pathToScenariosFolder)
        {
            // Read csv file
            // Loop through csv file
            //      Extract location, period
            //      Load appropriate ini file
            //      Overwrite "event_date" and "date" with values from csv
            //      Save ini

            var csvReader = new StreamReader(pathToPlantingDatesFile);
            CsvReader csv = new CsvReader(csvReader);
            var records = csv.GetRecords<csvDto>().ToList();

            foreach(var record in records)
            {
                WriteDateToRotationFile(record);
            }

            throw new NotImplementedException();
        }

        private string getRotationFromCrop(string crop)
        {
            //string rotation = "";
            //switch(crop.ToLower())
            //{
            //    case "spring canola":
            //        rotation = "sC-wW";
            //        break;
            //    case "spring pea":
            //        rotation = "sP-wW";
            //        break;
            //    case "spring wheat":
            //        rotation = "sW-wW";
            //        break;
            //}
            string rotation = cropToRotation[crop.ToLower()];
            return rotation;
        }
        private string getPeriodFromPlantingDate(DateTime plantingDate)
        {
            int year = plantingDate.Year;
            int previousYear = plantingDate.AddYears(-1).Year;

            string period = $"{previousYear}-{year}";

            return period;
        }

        private void WriteDateToRotationFile(csvDto record)
        {
            //string rotation = getRotationFromCrop(record.Crop);
            string location = record.SimLocation;
            string period = getPeriodFromPlantingDate(record.PlantingDate);
            string plantingDoy = (record.PlantingDate.DayOfYear + 1000).ToString();
            //string iniPath = Path.Combine(this.creationDate, rotation, location, period);

            var parser = new FileIniDataParser();
            //IniData rotFile = parser.ReadFile(iniPath);
            //rotFile["sowing:1"]["event_date"] = plantingDoy;
            //rotFile["sowing:1"]["date"] = plantingDoy;

            //rotFile.save()?

        }
    }
}
