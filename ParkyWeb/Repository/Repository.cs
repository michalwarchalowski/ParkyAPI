using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFacotry;

        public Repository(IHttpClientFactory clientFacotry)
        {
            _clientFacotry = clientFacotry;
        }

        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }


            var client = _clientFacotry.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else { return false; }




        }

        public Task<bool> DeleteAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
