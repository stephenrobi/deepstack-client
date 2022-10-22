using Flurl;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DeepStack.Client
{
    public class DeepStackClient
    {

        private HttpClient _client;

        public DeepStackClient(HttpClient httpClient, string deepStackServerBaseUrl)
        {
            _client = httpClient;
            this.BaseUrl = deepStackServerBaseUrl;
        }

        public string BaseUrl { get; private set; }

        public HttpResponseHeaders LastResponseHeaders { get; private set; }


        async public Task<DeepStackResponse> DetectObjects(Stream imageStream, string imageName = null, CancellationToken cancellationToken = default)
        {
            var request = new MultipartFormDataContent();
            
            request.Add(new StreamContent(imageStream), "image", imageName);

            //make sure we pass something as the imagename
            imageName = string.IsNullOrEmpty(imageName) ? "image1.jpg" : imageName;

            var url = this.BaseUrl.AppendPathSegments("v1", "vision", "detection");

            
            var output = await _client.PostAsync(url, request, cancellationToken);

            this.LastResponseHeaders = output.Headers;

            

            if (!output.IsSuccessStatusCode)
            {
                // an http error occurred, so throw the exception here
                string message = await output.Content.ReadAsStringAsync();

                throw new HttpResponseException(message, output.StatusCode);
            }

            var jsonString = await output.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<DeepStackResponse>(jsonString);

            return response;
        }

        async public Task<DeepStackResponse> DetectObjects(string imagePath, CancellationToken cancellationToken = default)
        {
            using (var imageStream = File.OpenRead(imagePath))
            {
                return await DetectObjects(imageStream, Path.GetFileName(imagePath), cancellationToken);
            }

        }





    }
}
