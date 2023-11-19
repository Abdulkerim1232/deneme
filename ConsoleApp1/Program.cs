using System;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string githubCsvUrl = "https://raw.githubusercontent.com/RamunasAdamonis/vilnius-tech-software-design-2023/master/sport_activities.csv";

        try
        {
            List<List<string>> columns = await ReadCsvToColumns(githubCsvUrl);

            double v1 = Convert.ToDouble(columns[3][1]);
            int i = 0, row = 0;
            foreach (string values in columns[3])
            {
                i++;
                if (values == columns[3][0]) continue;
                else {
                    if (values == "") continue;
                    else {
                        double v2 = Convert.ToDouble(values);
                        if (v2 >= v1)
                        {
                            row = i;
                            v1 = v2;
                        }
                    }
                    
                }
                
            }
            Console.WriteLine("Longest activity by duration:\nLine: " + row + "\nID: " + columns[0][row-1] + "\nActivity-date: " +
                columns[1][row - 1] + "\nActivity-type: "+ columns[2][row - 1] + "\nElapsed-time: " + columns[3][row - 1] +
                "\nMoving-time: " + columns[4][row - 1] + "\nDistance: " + columns[5][row - 1] + "\nAverage-speed: " +
                columns[6][row - 1] + "\nAverage-heart-rate: " + columns[7][row - 1]);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred: " + ex.Message);
        }
    }

    static async Task<List<List<string>>> ReadCsvToColumns(string csvUrl)
    {
        List<List<string>> columns = new List<List<string>>();

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(csvUrl);
            if (response.IsSuccessStatusCode)
            {
                string csvContent = await response.Content.ReadAsStringAsync();
                string[] rows = csvContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                if (rows.Length > 0)
                {
                    int columnCount = rows[0].Split(',').Length;
                    columns = Enumerable.Range(0, columnCount).Select(_ => new List<string>()).ToList();

                    foreach (string row in rows)
                    {
                        string[] cells = row.Split(',');
                        for (int i = 0; i < cells.Length && i < columnCount; i++)
                        {
                            columns[i].Add(cells[i]);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Failed to fetch CSV file. Status code: " + response.StatusCode);
            }
        }

        return columns;
    }
}
