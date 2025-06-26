using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;

public class DrillingDataManager : MonoBehaviour
{
    public List<DrillingDataCSV> DrillingData = new List<DrillingDataCSV>();

    
    
    
    
    public bool Load(string path)
    {
        string data = System.IO.File.ReadAllText(path);
        using (StringReader reader = new StringReader(data))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            try
            {
                DrillingData = csv.GetRecords<DrillingDataCSV>().ToList();
            }
            catch
            {
                return false;
            }
        }
        return true;
    }
}