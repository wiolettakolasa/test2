using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SightseeingApp.Service
{
    public class GeoCoordsService
    {
        public async Task<GeoCoordsResult> GetGeoCoords(string name)
        {
            var result = new GeoCoordsResult
            {
                Success = false,
                Message = "Failed to get coordinates"

        };
            var apiKey = "Ar_tX8MebguSGGlKc_j0PJRSGQhRGP7tyDGPDuagfBy3HiJ5LzSmD75RecbOCCdS";
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"http://dev.virtualearth.net/Rest/v1/Locations?q={encodedName}&key={apiKey}";

            var client = new HttpClient();
        
            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if(!results["resourceSets"][0]["resources"].HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if(confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location ";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longtitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

            return result;

        }
    }
}