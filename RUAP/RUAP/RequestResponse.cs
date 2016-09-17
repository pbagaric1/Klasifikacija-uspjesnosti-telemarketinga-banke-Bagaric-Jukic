using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace CallRequestResponseService
{

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    class RequestResponse
    {
        static string res;
        public static string Result
        {
            get
            {
                return res;
            }
            set
            {
                res = value;
            }
        }

       public static async Task InvokeRequestResponseService(string[] data)
        {
            string[,] dataTosend = new string[1, data.Length];
            int i = 0;
            foreach (string temp in data)
            {
                dataTosend[0, i] = temp;
                i++;
            }
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"age", "job", "marital", "education", "default", "balance", "housing", "loan", "contact", "day", "month", "duration", "campaign", "pdays", "previous", "poutcome", "y"},
                                Values =  dataTosend
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "0CGoUQuhGOp1eMCgT/b+yVGaFMvr6eQ2mb8S4nyutqVjeKkqSWlYkhSrkV9BYDcIci4IGC82I3BmSn4gZrsprg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/7d7848cb001d46359a76b5561abb9c89/services/7310e01b80bc44ae83e4d73d52b8488a/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Result = result;
                }
                else
                {
                    Result = (string.Format("The request failed with status code: {0}\n\n", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Result += (response.Headers.ToString() + "\n\n");

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Result += (responseContent + "\n\n");
                }
            }
        }
    }
}