using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RainfallAPI.Controllers.Readings
{
    [Route("api/[controller]")]
    [ApiController]
    public class Reading : ControllerBase
    {
        private static string _address = "http://environment.data.gov.uk/flood-monitoring/id/stations/";

        // GET: api/<Reading>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET rainfall/id/{stationId}/readings
        [HttpGet("{stationId}")]
        public async Task<string> Get(string stationId)
        {
            var result = await GetStationReadingsFromSorted(stationId);

            return result;
        }

        private static async Task<string> GetStationReadingsFromSorted(string Id)
        {
            //complete the response address
            var address = _address + Id + "/readings";

            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(address);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        #region Not in use
        //// POST api/<Reading>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Reading>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Reading>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        #endregion
    }
}
