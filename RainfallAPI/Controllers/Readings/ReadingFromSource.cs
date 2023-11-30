using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainfallAPI.Models.RainfallReadings;
using RainfallAPI.Models.Readings;
using RainfallAPI.Models.Readings.ResponseHandler;
using RainfallAPI.Models.Readings.ResponseHandler.Components.Metas;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RainfallAPI.Controllers.Readings
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingFromSource : ControllerBase
    {
        private static string _address = "http://environment.data.gov.uk/flood-monitoring/id/stations/";

        // GET rainfall/id/{stationId}/readings
        [HttpGet("{stationId}")]
        public async Task<ReadingResponseHandler> Get(string stationId)
        {
            var result = await GetStationReadingsFromSorted(stationId);

            return result;
        }

        private static async Task<ReadingResponseHandler> GetStationReadingsFromSorted(string Id)
        {
            //complete the response address
            var address = _address + Id + "/readings";

            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(address);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return await ExternalOutputToReadingModelMapper(result);
        }

        private static async Task<ReadingResponseHandler> ExternalOutputToReadingModelMapper(string responseMessage)
        {
            try
            {
                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(responseMessage);

                //var jsonObject = JObject.Parse(responseMessage);
                //JsonElement data = jsonValues.RootElement;

                #region Parse Meta.HasFormat values
                var meta = JsonConvert.DeserializeObject<JObject>(jsonObject["meta"].ToString());
                var metaFormats = meta["hasFormat"].ToArray();

                List<MetaFormat> formats = new List<MetaFormat>();

                foreach (var item in metaFormats)
                {
                    formats.Add(
                         new MetaFormat
                         {
                             Format = item.ToString()
                         });
                }
                #endregion

                #region Convert JSON Property Items
                // NOTE: "items" is in itself a List of objects. Error is occurring here.

                List<RainfallReadingFromSource> readings = new List<RainfallReadingFromSource>();
                foreach (var item in jsonObject["items"])
                {
                    var i = JsonConvert.DeserializeObject<JObject>(item.ToString());

                    readings.Add(
                        new RainfallReadingFromSource
                        {
                            Id = i["@id"].ToString(),
                            DateMeasured = (DateTime)i["dateTime"],
                            Measure = i["measure"].ToString(),
                            AmountMeasured = (double)i["value"]
                        });
                }
                #endregion

                ReadingResponseHandler result = new()
                {
                    Context = jsonObject["@context"].ToString(),
                    Meta = new Meta()
                    {
                        Publisher = meta.Root["publisher"].ToString(),
                        Licence = meta["licence"].ToString(),
                        Documentation = meta["documentation"].ToString(),
                        Version = meta["version"].ToString(),
                        Comment = meta["comment"].ToString(),
                        HasFormat = formats,
                        Limit = (int)meta["limit"]
                    },
                    Items = readings
                };
                return result;
            }
            catch(Exception ex)
            {
                string innerException = string.Empty;
                if (ex.InnerException != null)
                    innerException = $"\nInnerException:{ex.InnerException.Message}";

                Console.WriteLine(innerException);
                throw;
            }
        }

        #region Not in use
        // GET: api/<Reading>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

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
