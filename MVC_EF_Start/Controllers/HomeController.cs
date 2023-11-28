using Microsoft.AspNetCore.Mvc;
using MVC_EF_Start.Models;
using MVC_EF_Start.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.IO;
using MVC_EF_Start.Models;
using System.Diagnostics;
using MVC_EF_Start.Data;
using System.Text.Json;

namespace MVC_EF_Start.Controllers
{


    public class HomeController : Controller
    {

        public Dictionary<string, string> golbalCountyRegionMapping = new Dictionary<string, string>
            {
                { "Stevens", "Northeast" },
                { "Spokane", "Northeast" },
                { "Skagit", "Northwest" },
                { "Snohomish", "Northwest" },
                { "Island", "Northwest" },
                { "Chelan", "Southeast" },
                { "Grant", "Southeast" },
                { "Whitman", "Southeast" },
                { "Yakima", "Southeast" },
                { "Klickitat", "Southeast" },
                { "Walla Walla", "Southeast" },
                { "King", "South Puget Sound" },
                { "Thurston", "South Puget Sound" },
                { "Kitsap", "South Puget Sound" },
                { "Clallam", "Olympic" },
                { "Jefferson", "Olympic" },
                { "Cowlitz", "Pacific Cascade" },
                { "Clark", "Pacific Cascade" }
            };


        HttpClient? httpClient;

        static string BASE_URL = "https://data.wa.gov/resource/f6w7-q2d2.json?$query=SELECT%20vin_1_10%2C%20county%2C%20city%2C%20state%2C%20zip_code%2C%20model_year%2C%20make%2C%20model%2C%20ev_type%2C%20cafv_type%2C%20electric_range%2C%20base_msrp%2C%20legislative_district%2C%20dol_vehicle_id%2C%20geocoded_column%2C%20electric_utility%2C%20_2020_census_tract%20ORDER%20BY%20%3Aid%20ASC";

        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }


        private DRegion createOrGetExistingRegion(string regionName)
        {
            Debug.WriteLine("Trying to locate Region :" + regionName);

            var existingRegion = dbContext.DRegions
                .FirstOrDefault(r => r.RegionName == regionName);

            if (existingRegion == null)
            {
                // Region does not exist, create a new one
                var newRegion = new DRegion
                {
                    RegionName = regionName
                };

                // Add the new region to the Regions DbSet
                dbContext.DRegions.Add(newRegion);

                // Save changes to the database
                dbContext.SaveChanges();

                Debug.WriteLine("New region added to the database with ID: " + newRegion.RegionID);
                return newRegion;
            }

            return existingRegion;

        }

        private DMake createOrGetMake(DMake carMake)
        {
            Debug.WriteLine("Trying to locate Make :" + carMake.MakeName);

            var currentMake = dbContext.DMakes
                .FirstOrDefault(r => r.MakeName == carMake.MakeName);

            if (currentMake == null)
            {
                // Add the new region to the Regions DbSet
                dbContext.DMakes.Add(carMake);
                // Save changes to the database
                dbContext.SaveChanges();

                Debug.WriteLine("New region added to the database with ID: " + carMake.MakeID);
                return carMake;
            }

            return currentMake;

        }

        private DModel createOrGetModel(DModel newModel)
        {
            Debug.WriteLine("Trying to locate Model :" + newModel);

            var currentModel = dbContext.DModels
                .FirstOrDefault(r => r.ModelName == newModel.ModelName);

            if (currentModel == null)
            {

                // Add the new region to the Regions DbSet
                dbContext.DModels.Add(newModel);
                // Save changes to the database
                dbContext.SaveChanges();

                Debug.WriteLine("New MOdel added to the database with Name: " + newModel.ModelName);
                return newModel;
            }

            return currentModel;

        }

        private DVehicle createNewVehcile(DVehicle vehicle)
        {

            Debug.WriteLine("Trying to create  vehicle :" + vehicle);
            // Add the new region to the Regions DbSet


            var currentVehicle = dbContext.DVehicles
                .FirstOrDefault(v => v.VIN == vehicle.VIN);

            if (currentVehicle == null)
            {
                dbContext.DVehicles.Add(vehicle);
                dbContext.SaveChanges();
                return vehicle;
            }

            // Save changes to the database
            Debug.WriteLine("Vehicle Already Present " + vehicle.VIN);
            return currentVehicle;
        }




        public IActionResult ImportData()
        {
            string filePath = "./Data/EVData.xlsx";

            DataReader excelReader = new DataReader();
            List<ExcelDataViewModel> excelDataList = excelReader.ReadExcel(filePath);

            foreach (var excelData in excelDataList)
            {
                var county = new DCounty
                {
                    CountyName = excelData.County,
                };

                // Check if the county already exists
                var existingCounty = dbContext.DCounties.SingleOrDefault(c => c.CountyName == county.CountyName);

                if (existingCounty == null)
                {
                    Debug.WriteLine("Need to create new county " + excelData.County);
                    var countyRegion = createOrGetExistingRegion(golbalCountyRegionMapping[excelData.County]);

                    county.RegionID = countyRegion.RegionID;

                    // If not, add the county to the database
                    dbContext.DCounties.Add(county);
                    dbContext.SaveChanges();

                }
                else
                {
                    county = existingCounty;
                }

                // Map ExcelDataViewModel to DMake
                var make = new DMake
                {
                    MakeName = excelData.Make,
                    CountyID = county.CountyID,
                };
                make = createOrGetMake(make);

                // Map ExcelDataViewModel to DModel
                var model = new DModel
                {
                    ModelName = excelData.Model,
                    CountyID = county.CountyID,
                    // Map other properties for DModel if needed
                };
                model = createOrGetModel(model);

                // Map ExcelDataViewModel to DVehicle
                var vehicle = new DVehicle
                {
                    VIN = excelData.VIN,
                    Make = excelData.Make,
                    MakeID = make.MakeID,
                    Model = excelData.Model,
                    ModelID = model.ModelID,
                    Range = excelData.ElectricRange,
                    CountyID = county.CountyID,
                    // Map other properties for DVehicles if needed
                };

                vehicle = createNewVehcile(vehicle);
            }

            // Save changes to the database
            dbContext.SaveChanges();

            return RedirectToAction("RegionGrouping");

        }




        public IActionResult EVHomePage()
        {
            return View();
        }

        public IActionResult RegionGrouping()
        {
            var countyRegionMapping = new Dictionary<string, string>
            {
                { "Stevens", "Northeast" },
                { "Spokane", "Northeast" },
                { "Skagit", "Northwest" },
                { "Snohomish", "Northwest" },
                { "Island", "Northwest" },
                { "Chelan", "Southeast" },
                { "Grant", "Southeast" },
                { "Whitman", "Southeast" },
                { "Yakima", "Southeast" },
                { "Klickitat", "Southeast" },
                { "Walla Walla", "Southeast" },
                { "King", "South Puget Sound" },
                { "Thurston", "South Puget Sound" },
                { "Kitsap", "South Puget Sound" },
                { "Clallam", "Olympic" },
                { "Jefferson", "Olympic" },
                { "Cowlitz", "Pacific Cascade" },
                { "Clark", "Pacific Cascade" }
            };

            var countyNames = dbContext.DCounties.Select(c => c.CountyName).ToList();


            var vehiclesForCounties = dbContext.DVehicles
                .Include(v => v.County) // Include the County navigation property
                .Where(v => v.County != null && countyNames.Contains(v.County.CountyName))
                .ToList();


            var regionData = vehiclesForCounties
                .GroupBy(v => v.County.CountyName)
                .Select(g => new RegionViewModel
                {
                    RegionName = countyRegionMapping.GetValueOrDefault(g.Key, "Unknown"),
                    TotalCars = g.Count()
                })
                .GroupBy(r => r.RegionName)
                .Select(g => new RegionViewModel
                {
                    RegionName = g.Key,
                    TotalCars = g.Sum(r => r.TotalCars)
                })
                .ToList();

            // Convert the complex object to a Dictionary
            var serializableRegionData = regionData.ToDictionary(r => r.RegionName, r => (object)r.TotalCars);
            var regionDataJson = System.Text.Json.JsonSerializer.Serialize(regionData);

            // Store the region data in Session instead of TempData
            // HttpContext.Session.Set("RegionData", serializableRegionData);
            TempData["RegionData"] = regionDataJson;
            return RedirectToAction("MainPage");
        }


        public IActionResult MainPage()
        {
            Debug.WriteLine("Inside Function call for Main Page ******************************************** ");
            try
            {

                // Retrieve the JSON string from TempData
                var regionDataJson = TempData["RegionData"] as string;

                Debug.WriteLine("JSON TO VIEW : " + regionDataJson);

                // Deserialize the JSON string back to your object
                // var storedData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(regionDataJson);

                // Deserialize the JSON string into a collection of RegionData
                var storedData = System.Text.Json.JsonSerializer.Deserialize<List<RegionData>>(regionDataJson);

                // var storedData = TempData["RegionData"] as Dictionary<string, object>;
                //var storedData = TempData["RegionData"] as Dictionary<string, object>;
               
                var regionData = storedData;

                Debug.WriteLine("Retreived Region Data from Session ");

                if (storedData == null)
                {
                    return View(new ChartModel()); // Provide an empty ChartModel if TempData is null
                }

                Debug.WriteLine("Contents of storedData:");

                foreach (var data in storedData)
                {
                    Debug.WriteLine($"RegionName: {data.RegionName}, TotalCars: {data.TotalCars}");
                }

                string[] regionNames = storedData.Select(data => data.RegionName).ToArray();

                // Extract TotalCars into an array
                var chartLabels = storedData.Select(data => data.RegionName).ToArray();
                var chartColors = new string[] { "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9" }; // Colors for the chart
                var chartData = storedData.Select(data => Convert.ToInt32(data.TotalCars)).ToArray();


                var chartModel = new ChartModel
                {
                    ChartType = "horizontalBar",
                    // Labels = string.Join(",", chartLabels.Select(d => "'" + d + "'")),
                    Labels = storedData.Select(data => data.RegionName).ToList(),
                    Data = storedData.Select(data => data.TotalCars).ToList(),

                    Colors = chartColors.ToList(),
                    //Data = string.Join(",", chartData.Select(d => d)),
                    Title = "Electric Vehicles in Washington State"
                };

               /* if (chartModel.Labels == null || chartModel.Data == null)
                {
                    // Handle the case where the database query returned no results
                    chartModel.ChartType = "bar";
                    chartModel.Labels = new string[] { "No Data" };
                    int[] values = new int[0];
                    chartModel.Data = values;
                    chartModel.Colors = "['#ff0000']";
                    chartModel.Title = "No Data Available";
                }*/

                return View(chartModel);
            }
            catch (Exception ex)
            {
                // Log the exception details, you can replace this with your preferred logging mechanism
                Console.WriteLine($"Exception in MainPage action: {ex.Message}");
                return View(new ChartModel()); // Provide an empty ChartModel in case of an exception
            }
        }
        // connects to detail page with tables and CRUD functions
        public IActionResult DetailPage()
        {
            return View();
        }


        // connects to the about us page
        public IActionResult AboutUsPage()
        {
            return View();
        }

    }
}