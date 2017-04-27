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
    // Directory structure: {creation date}/{rotation}/{location}/{period}
    class DateWriter
    {
        private readonly string creationDate;

        private readonly Dictionary<string, string> cropToRotation = new Dictionary<string, string>
        {
            {"spring canola","sC-wW" },
            {"spring peas","sP-wW" },
            {"spring wheat", "sW-wW" }
        };
        public DateWriter(string creationDate = "17-04-25")
        {
            
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
            csv.Configuration.RegisterClassMap<ObservedPlantingMap>();

            var records = csv.GetRecords<ObservedPlanting>().ToList();

            for(int i = 1; i < records.Count; i++)
            {
                // Skip first record -- row has descriptions not data
                WriteDateToRotationFile(records[i], pathToScenariosFolder);
            }
            

            //throw new NotImplementedException();
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

        private void WriteDateToRotationFile(ObservedPlanting record, string pathToScenariosFolder)
        {
            string rotation = getRotationFromCrop(record.CropName);
            string location = record.SimLocation;
            string period = getPeriodFromPlantingDate(record.PlantingDate);
            string plantingDoy = (record.PlantingDate.DayOfYear + 1000).ToString();
            string iniFileName = $"{rotation}.rot";
            string iniPath = Path.Combine(pathToScenariosFolder, this.creationDate, rotation, location, period, iniFileName);

            // Copy template ini file if one doesn't exist in the target dir
            if(!File.Exists(Path.Combine(iniPath)))
            {
                File.Copy(Path.Combine("Input", iniFileName), iniPath);
            }

            // Parse current file
            var parser = new FileIniDataParser();
            IniData rotFile = parser.ReadFile(iniPath);
            rotFile.Configuration.AssigmentSpacer = "";

            // Write new dates, save
            rotFile["sowing:1"]["event_date"] = plantingDoy;
            rotFile["sowing:1"]["date"] = plantingDoy;
            parser.WriteFile(iniPath, rotFile);

            // Write entry in the shell script to automate runs
            string pathToShellScript = Path.Combine(pathToScenariosFolder, this.creationDate, "run_specific-planting-dates.sh");
            string cropSystScenarioPath = $"{rotation}/{location}/{period}";
            writeRunShellScript(pathToShellScript, cropSystScenarioPath);
        }

        // Writes a linux shell script
        private void writeRunShellScript(string scriptFilePath, string cropSystScenarioPath)
        {
            using (var f = new StreamWriter(scriptFilePath, true))
            {
                f.NewLine = "\n";
                f.WriteLine($"pushd {cropSystScenarioPath}");
                f.WriteLine("CropSyst_5 --verbose=15 --progress");
                f.WriteLine("popd");
            }
        }
    }
}
