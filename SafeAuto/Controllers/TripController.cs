﻿using System;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using SafeAuto.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace SafeAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : Controller
    {
        // handles uploading of txt file
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                //extract the file from the request
                var file = Request.Form.Files[0];

                //create folder name for files
                var folderName = Path.Combine("Resources", "TextFiles");

                //path to save file
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                //ensures that file is not empty
                if (file.Length > 0)
                {
                    //get the name of the file
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    //path for store the file
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                   
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {

                        file.CopyTo(stream);
                        stream.Close();
                    }

                    TripController.ReadFiles(fullPath);

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }

        public static void ReadFiles(string file)
        {
            try
            {
                using (var sr = new StreamReader(file))
                {
                    //read each line into a string array
                    string[] lines = System.IO.File.ReadAllLines(file);

                    //creates a new driver object
                    var driver = new Driver();

                    //creates a new trip object
                    var trip = new Trip();

                    foreach (string line in lines)
                    {
                        char[] separator = { ' ' };
                        int count = 5;
                        String[] strlist = line.Split(separator,
                                       count, StringSplitOptions.None);

                        //register a Driver
                        if (line.Contains("Driver"))        
                        {
                            driver.DriverName = strlist[1];
                        }

                        //populate Trip
                        if (line.Contains("Trip"))
                        {
                            trip.DriverName = strlist[1];
                            trip.StartTime = strlist[2];
                            trip.EndTime = strlist[3];
                            trip.MilesDriven = strlist[4];
                        }
                    }

                    //CalculateTrip(trip);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Internal Server Error: {ex}.");
            }
        }

        //[HttpGet]
        //public IActionResult CalculateTrip(Trip trip) Get()
        //{
        //    try
        //    {
        //        //get time difference
        //        TimeSpan duration = DateTime.Parse(trip.EndTime).Subtract(DateTime.Parse(trip.StartTime));
        //        return duration;
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusCode(500, $"Interal Server Error: {ex}.");
        //    }
        //}

        ////calculate trip information and send to frontend
        //public static string CalculateTrip(Trip trip)
        //{
        //    try
        //    {
        //        ////get the time difference between start and end
        //        //TimeSpan duration = DateTime.Parse(trip.EndTime).Subtract(DateTime.Parse(trip.StartTime));
        //        //double milesDrivenToInt = Convert.ToInt32(trip.MilesDriven);

        //        ////calculate mph = distance / duration
        //        //var mph = milesDrivenToInt / duration;

        //        //Console.WriteLine(mph);

        //        //return $"{trip.DriverName}: {trip.MilesDriven} @ {mph}";
        //    }
        //    catch (Exception ex)
        //    {
               
        //    }
        //}
    }
}
