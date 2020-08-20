using System;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using SafeAuto.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SafeAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : Controller
    {
        //POST /api/trips/upload
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

                    //ensures that no null value will be passed into ListTrip()
                    if (fullPath == "")
                    {
                        //no need to continue on from here
                        //user has not inputted a file yet
                        return Ok();
                    }
                    else
                    {
                        return RedirectToAction("ListTrips", new { file = fullPath });
                    }
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

        //GET api/trips/listTrips
        //list all trips that are in the input file
        [HttpGet("listTrips")]
        public List<Trip> ListTrips(string file)
        {
            if (file == null)
            {
                return new List<Trip>();
            }

            //create list of trips
            var listOfTrips = new List<Trip> { };

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

                            listOfTrips.Add(trip);
                        }
                    }
                }

                return listOfTrips;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return StatusCode(500, $"Internal Server Error: {ex}");
                return new List<Trip>();
            }
        }
    }
}
