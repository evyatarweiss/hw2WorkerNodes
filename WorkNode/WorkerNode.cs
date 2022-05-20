using Newtonsoft.Json;
using System.Text;

namespace WorkNode.WorkNode
{
    public class WorkerNode : IWorkerNode
    {
        public bool isRunning;
        public WorkerNode()
        {
            isRunning = false;
        }

        
        public OutputObject Calculate(string jobId, string buffer, int interations)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(buffer);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                for (int i = 1; i < interations; i++)
                {
                    hashedInputBytes = hash.ComputeHash(hashedInputBytes);
                }

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                string hashedString = hashedInputStringBuilder.ToString();
                OutputObject result = new OutputObject(jobId, hashedString);
                return result;
            }
        }
        public bool CalculateAndPost(InputObject currentItem)
        {
            try
            {
                var HttpClient = new HttpClient();

                OutputObject result = Calculate(currentItem.id, currentItem.buffer, currentItem.iterations);
                var returnObjectJson = JsonConvert.SerializeObject(result);
                while (true)
                {
                    var sendResults = HttpClient.PostAsync(string.Format(ManagementNodeConstant.managementPostCompletedUrl), new StringContent(returnObjectJson, Encoding.UTF8, "application/json"));
                    if (sendResults.IsCompletedSuccessfully)
                    {
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }


        }



        public async void GetItemsFromQueue()
        {
            while (CurrentVirtualMachine.isOn)
            {
                try
                {
                    var httpClient = new HttpClient();
                    while (true)
                    {
                        var response = httpClient.GetAsync(ManagementNodeConstant.managementGetIteamUrl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            if((int?)response.StatusCode == 200)
                            {
                                isRunning = true;
                                var responseContent = await response.Content.ReadAsStringAsync();
                                InputObject currentItem = JsonConvert.DeserializeObject<InputObject>(responseContent);

                                var calculateItem = CalculateAndPost(currentItem);
                                return;
                            }
                            else
                            {
                                Thread.Sleep(5000);
                            }
                        }
                    }

                }
                catch(Exception ex)
                {
                    return;
                }


            }
        }
    }
}
