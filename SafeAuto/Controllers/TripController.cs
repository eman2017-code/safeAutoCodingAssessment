using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

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
                // extract the file from the request
                var file = Request.Form.Files[0];
                // create folder name for files
                var folderName = Path.Combine("Resources", "TextFiles");
                // path to save file
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                // ensures that file is not empty
                if(file.Length > 0)
                {
                    // get the name of the file
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    // path for store the file
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath});
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




        //[HttpGet]
        //public static void ListTrip()
        //{
        //    try
        //    {
        //        // read file
        //        using (var sr = new StreamReader(@"c:\textFile.txt"))
        //        {
        //            Console.WriteLine("---STARTED---");
        //            Console.WriteLine(sr.ReadLine());
        //            Console.WriteLine("---ENDED---");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        
        // if file is empty, throw errors

        // if "Driver"
        // register the driver


        //CALCULATE OUTPUT --
        // if "Trip, driverName, startTime, endTime, milesDriven"
        // time = endTime - startTime = 12 - num 
        // miles = milesDriven
        // mph = miles / time
        // return driverName + ":" + " miles" + "@" + mph


    }
}
