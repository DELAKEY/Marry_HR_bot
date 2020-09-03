using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TgQuizBot.Networck
{
    public  class SheetsWorker
    {

        public void Send(string[] text)
        {
            SendData(text);
            
        }
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private const string SpreadsheetId = "1SkH7-1i1dyFj8jJv7ddmOkJcvVA_sqoNd4sbUR6krfo";
        private const string GoogleCredentialsFileName = "credentials.json";

        string count = "1";
        SpreadsheetsResource.ValuesResource valuesResource;
        void SendData(string[] args)
        {
            valuesResource = GetSheetsService().Spreadsheets.Values;
            ReadAsync().Wait();

            int pos = int.Parse(count);
             WriteAsync("A"+(pos+2)+":Z"+(pos+2),args).Wait();
             WriteAsync("A1:B1", new string[1] { (pos + 1).ToString() }).Wait();
            
            //await ReadAsync(serviceValues);
        }

        private static SheetsService GetSheetsService()
        {
            using (var stream =
                new FileStream(GoogleCredentialsFileName, FileMode.Open, FileAccess.Read))
            {
                var serviceInitializer = new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(Scopes)
                };
                return new SheetsService(serviceInitializer);
            }
        }

        private async Task ReadAsync()
        {
            var response = await valuesResource.Get(SpreadsheetId, "A1:B1").ExecuteAsync();
            var values = response.Values;

            if (values == null || !values.Any())
            {
                Console.WriteLine("No data found.");
                return;
            }

            var header = string.Join(" ", values.First().Select(r => r.ToString()));
            Console.WriteLine($"Header: {header}");

            var row = values[0];
            count = row[0].ToString();
            return;
        }

        private async Task WriteAsync(string pos, string[] text)
        {
            var valueRange = new ValueRange { Values = new List<IList<object>> { new List<object> (text) } };
            var update = valuesResource.Update(valueRange, SpreadsheetId, pos);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            var response = await update.ExecuteAsync();
            Console.WriteLine($"Updated rows: {response.UpdatedRows}");
        }
    }
}