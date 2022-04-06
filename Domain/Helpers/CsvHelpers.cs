using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public class CsvHelpers
    {
        public static List<T> MountRecordsFromCsv<T>(StreamReader reader)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                Delimiter = ";",
            };

            var csv = new CsvReader(reader, config);
            return csv.GetRecords<T>().ToList();
        }
    }
}
