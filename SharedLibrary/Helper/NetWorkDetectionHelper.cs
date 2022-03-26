using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{
    internal class NetWorkDetectionHelper
    {
        /// <summary>
        /// 检测链接有效性
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<int> GetUrlStatusAsync(string url)
        {
            HttpClient client = new HttpClient();
            if (!string.IsNullOrEmpty(url) && RegHelper.ISURLS(url))
            {
                
                var req = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    req.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                    req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:98.0) Gecko/20100101 Firefox/98.0");
                    
                    HttpResponseMessage response = await client.SendAsync(req);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return 0;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Exception Caught!\n");
                    return -1;
                }
                finally
                {
                    if (req != null)
                    {
                        req.Dispose();
                        client.Dispose();
                    }
                }
            }
            else
            {
                return -1;
            }
           
        }

    }
}
