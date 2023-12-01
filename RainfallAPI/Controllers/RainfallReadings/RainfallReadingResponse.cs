using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RainfallAPI.Controllers.Readings;
using RainfallAPI.Models.RainfallReadings;
using RainfallAPI.Models.Readings.ResponseHandler;
using RainfallAPI.Models.Readings.ResponseHandler.Components.Metas;
using System.Net;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RainfallAPI.Controllers.RainfallReadings
{
    [System.Web.Http.Route("api/[controller]")]
    [ApiController]
    public class RainfallReadingResponse : ControllerBase
    {
        private static string _address = "http://environment.data.gov.uk/flood-monitoring/id/stations/";

        // GET: api/<RainfallReadings>
        [HttpGet("{stationId}")]
        public async Task<ActionResult<Models.RainfallReadings.RainfallReadingResponse>> Get(string stationId)
        {
            var result = await GetStationReadingsFromSorted(stationId);

            if(result.Readings.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return result;
        }

        private static async Task<Models.RainfallReadings.RainfallReadingResponse> GetStationReadingsFromSorted(string Id)
        {
            //complete the response address
            var address = _address + Id + "/readings";

            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(address);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return await ExternalOutputToReadingModelMapper(result);
        }

        private static async Task<Models.RainfallReadings.RainfallReadingResponse> ExternalOutputToReadingModelMapper(string responseMessage)
        {
            try
            {
                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(responseMessage);

                Models.RainfallReadings.RainfallReadingResponse result = new();
                List<RainfallReading> rainfallReading = new();

                #region Convert JSON Property Items
                foreach (var item in jsonObject["items"])
                {
                    var i = JsonConvert.DeserializeObject<JObject>(item.ToString());

                    rainfallReading.Add(
                        new RainfallReading
                        {
                            Id = i["@id"].ToString(),
                            DateMeasured = (DateTime)i["dateTime"],
                            AmountMeasured = (double)i["value"]
                        });
                }
                result.Readings = rainfallReading;
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                string innerException = string.Empty;
                if (ex.InnerException != null)
                    innerException = $"\nInnerException:{ex.InnerException.Message}";

                Console.WriteLine(innerException);
                throw;
            }
        }
    }
}
