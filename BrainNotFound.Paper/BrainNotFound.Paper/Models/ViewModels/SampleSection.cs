using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.Models.ViewModels
{
    public class SampleSection
    {
        public string CourseName { get; set; }
        public string DaysMet    { get; set; }
        public string FirstName  { get; set; }
        public string LastName   { get; set; }
        public int    Capacity   { get; set; }
        public string Location   { get; set; }

        public static IEnumerable<SampleSection> ParseCsv(string CsvFilePath)
        {
            IEnumerable<SampleSection> sampleSections;

            var reader = new StreamReader(CsvFilePath);
            var csv = new CsvReader(reader);

            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;


            sampleSections = csv.GetRecords<SampleSection>();
            return sampleSections;
        }
    }

   
}
