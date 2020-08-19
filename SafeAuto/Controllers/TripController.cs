using System;
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
        public IActionResult Upload(  )
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

                    foreach (string line in lines)
                    {
                        //creates a new driver object
                        var driver = new Driver();

                        //creates a new trip object
                        var trip = new Trip();

                        char[] separator = { ' ' };
                        int count = 5;
                        String[] strlist = line.Split(separator,
                                       count, StringSplitOptions.None);

                        //this is where you will register a driver
                        if (line.Contains("Driver"))        
                        {
                            driver.DriverName = strlist[1];
                        }

                        if (line.Contains("Trip"))
                        {
                            trip.DriverName = strlist[1];
                            trip.StartTime = strlist[2];
                            trip.EndTime = strlist[3];
                            trip.MilesDriven = strlist[4];

                            TripController.CalculateTrip(trip);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Internal Server Error: {ex}.");
            }
        }

        //calculate trip information and send to frontend
        public static void CalculateTrip(Trip trip)
        {
            //get the time difference between start and end
            TimeSpan duration = DateTime.Parse(trip.EndTime).Subtract(DateTime.Parse(trip.StartTime));

            //calculate mph = distance / time

            //create a new trip
        }
    }
}
