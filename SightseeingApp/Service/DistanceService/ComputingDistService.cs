using Newtonsoft.Json.Linq;
using SightseeingApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;


namespace SightseeingApp.Service
{
    public class ComputingDistService
    {
        public async Task<DistanceResult> GetDistanceDrivingResult(Sight sight1, Sight sight2)
        {
            var distResult = new DistanceResult
            {
                distance = 0,
                time = 0,
                Success = false,
                Message = "Failed to get distance and time of travel"

            };

            var apiKey = "Ar_tX8MebguSGGlKc_j0PJRSGQhRGP7tyDGPDuagfBy3HiJ5LzSmD75RecbOCCdS";

            var latitude1 = sight1.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);          
            var longtitude1 = sight1.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            
            var latitude2 = sight2.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude2 = sight2.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var coordinate1 = latitude1 + "," +  longtitude1; //"37.779160067439079,-122.42004945874214";
            var coordinate2 = latitude2 + "," + longtitude2;


            var url = $"http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={coordinate1}&wp.1={coordinate2}&routeAttributes=excludeItinerary&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];

            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                distResult.Message = $"Could not find distance between'{sight1.Name}'  and '{sight2.Name}' as a location";
            }
            else
            {
                var travelDistance = resources[0]["travelDistance"];
                var travelDuration = resources[0]["travelDuration"];

                distResult.distance = (double)travelDistance;
                distResult.time = (double)travelDuration / 60/60; //time int hours
                distResult.Success = true;
               distResult.Message = "Success";
            }

            return distResult;
        }

        public async Task<DistanceResult> GetDistanceDrivingResultRandomSight(RandomSight sight1, RandomSight sight2)
        {
            var distResult = new DistanceResult
            {
                distance = 0,
                time = 0,
                Success = false,
                Message = "Failed to get distance and time of travel"

            };

            var apiKey = "Ar_tX8MebguSGGlKc_j0PJRSGQhRGP7tyDGPDuagfBy3HiJ5LzSmD75RecbOCCdS";

            var latitude1 = sight1.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude1 = sight1.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var latitude2 = sight2.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude2 = sight2.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var coordinate1 = latitude1 + "," + longtitude1; //"37.779160067439079,-122.42004945874214";
            var coordinate2 = latitude2 + "," + longtitude2;


            var url = $"http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={coordinate1}&wp.1={coordinate2}&routeAttributes=excludeItinerary&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];

            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                distResult.Message = $"Could not find distance between'{sight1.Name}'  and '{sight2.Name}' as a location";
            }
            else
            {
                var travelDistance = resources[0]["travelDistance"];
                var travelDuration = resources[0]["travelDuration"];

                distResult.distance = (double)travelDistance;
                distResult.time = (double)travelDuration / 60 / 60; //time int hours
                distResult.Success = true;
                distResult.Message = "Success";
            }

            return distResult;
        }

        public async Task<DistanceResult> GetDistanceWalkingResult(Sight sight1, Sight sight2)
        {
            var distResult = new DistanceResult
            {
                distance = 0,
                time = 0,
                Success = false,
                Message = "Failed to get distance and time of travel"

            };

            var apiKey = "Ar_tX8MebguSGGlKc_j0PJRSGQhRGP7tyDGPDuagfBy3HiJ5LzSmD75RecbOCCdS";

            var latitude1 = sight1.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude1 = sight1.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var latitude2 = sight2.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude2 = sight2.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var coordinate1 = latitude1 + "," + longtitude1; //"37.779160067439079,-122.42004945874214";
            var coordinate2 = latitude2 + "," + longtitude2;


            var url = $"http://dev.virtualearth.net/REST/V1/Routes/Walking?wp.0={coordinate1}&wp.1={coordinate2}&routeAttributes=excludeItinerary&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];

            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                distResult.Message = $"Could not find distance between'{sight1.Name}'  and '{sight2.Name}' as a location";
            }
            else
            {
                var travelDistance = resources[0]["travelDistance"];
                var travelDuration = resources[0]["travelDuration"];

                distResult.distance = (double)travelDistance;
                distResult.time = (double)travelDuration / 60 / 60; //time int hours
                distResult.Success = true;
                distResult.Message = "Success";
            }

            return distResult;
        }

        public async Task<DistanceResult> GetDistanceWalkingResult(RandomSight sight1, RandomSight sight2)
        {
            var distResult = new DistanceResult
            {
                distance = 0,
                time = 0,
                Success = false,
                Message = "Failed to get distance and time of travel"

            };

            var apiKey = "Ar_tX8MebguSGGlKc_j0PJRSGQhRGP7tyDGPDuagfBy3HiJ5LzSmD75RecbOCCdS";

            var latitude1 = sight1.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude1 = sight1.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var latitude2 = sight2.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude2 = sight2.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var coordinate1 = latitude1 + "," + longtitude1; //"37.779160067439079,-122.42004945874214";
            var coordinate2 = latitude2 + "," + longtitude2;


            var url = $"http://dev.virtualearth.net/REST/V1/Routes/Walking?wp.0={coordinate1}&wp.1={coordinate2}&routeAttributes=excludeItinerary&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];

            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                distResult.Message = $"Could not find distance between'{sight1.Name}'  and '{sight2.Name}' as a location";
            }
            else
            {
                var travelDistance = resources[0]["travelDistance"];
                var travelDuration = resources[0]["travelDuration"];

                distResult.distance = (double)travelDistance;
                distResult.time = (double)travelDuration / 60 / 60; //time int hours
                distResult.Success = true;
                distResult.Message = "Success";
            }

            return distResult;
        }


        public async Task<DistanceResult> GetDistanceTransitResult(Sight sight1, Sight sight2)
        {
            var distResult = new DistanceResult
            {
                distance = 0,
                time = 0,
                Success = false,
                Message = "Failed to get distance and time of travel"

            };

            var apiKey = "Ar_tX8MebguSGGlKc_j0PJRSGQhRGP7tyDGPDuagfBy3HiJ5LzSmD75RecbOCCdS";

            var latitude1 = sight1.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude1 = sight1.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var latitude2 = sight2.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            var longtitude2 = sight2.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat);

            var coordinate1 = latitude1 + "," + longtitude1; //"37.779160067439079,-122.42004945874214";
            var coordinate2 = latitude2 + "," + longtitude2;


            var url = $"http://dev.virtualearth.net/REST/V1/Routes/Transit?wp.0={coordinate1}&wp.1={coordinate2}&routeAttributes=excludeItinerary&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];

            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                distResult.Message = $"Could not find distance between'{sight1.Name}'  and '{sight2.Name}' as a location";
            }
            else
            {
                var travelDistance = resources[0]["travelDistance"];
                var travelDuration = resources[0]["travelDuration"];

                distResult.distance = (double)travelDistance;
                distResult.time = (double)travelDuration / 60 / 60; //time int hours
                distResult.Success = true;
                distResult.Message = "Success";
            }

            return distResult;
        }

    }
}