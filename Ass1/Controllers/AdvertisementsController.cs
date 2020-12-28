using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Assign1.Data;
using Assign1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assign1.Models.ViewModels;

namespace Assign1.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly SchoolCommunityContext _context;
        private readonly string containerName = "blah";
        private readonly BlobServiceClient _blobServiceClient;


        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> Index(string id)
        {
            
            AdvertisementViewModel viewModel = new AdvertisementViewModel();
            if (id != null)
            {
                viewModel.Community = await _context.Communities.FindAsync(id);
                if (viewModel.Community == null)
                {
                    return NotFound();
                }
                viewModel.Advertisements = _context.Advertisements.Where(x => x.CommunityID == id).ToList();
            } else
            {
                return NotFound();
            }

            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Advertisement advert = await _context.Advertisements.Include(i => i.Community)
                                                         .FirstOrDefaultAsync(m => m.AdvertisementId == int.Parse(id)); //.Find(int.Parse(id));

            

            return View(advert);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {

            Advertisement advert = await _context.Advertisements.Include(i => i.Community).FirstOrDefaultAsync(m => m.AdvertisementId == int.Parse(id));
            Community community = advert.Community;
            BlobContainerClient containerClient;
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(advert.FileName);

                // Check if the blob exists
                if (await blockBlob.ExistsAsync())
                {
                    // if the blob already exists delete it.
                    await blockBlob.DeleteAsync();
                }
                // Remove the image from our database
                _context.Advertisements.Remove(advert);
                // Save our changes
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index), new { id = community.ID });
        }


        [HttpGet]
        public async Task<IActionResult> Upload(string id)
        {

            var Community = await _context.Communities.FindAsync(id);
            return View(Community);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile adImage, string id)
        {
            BlobContainerClient containerClient; // instanciate a class to allow us to manipulate the blob storage we made

            try
            {
                // Attempt to create a new container that will hold our blob images if it already exists we'll get a RequestFailedException
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Set the access to the container to be public allowing anyone to upload images to our blob container
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                // We've already made a blob container, continue with the old blob container.
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }

            try
            {
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(adImage.FileName);

                // check if the blob already exists
                if (await blockBlob.ExistsAsync())
                {

                    // if so lets delete it.
                    await blockBlob.DeleteAsync();
                }

                // get a new memory stream so we can upload to the storage container.
                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await adImage.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }

                // add the photo to the database if it uploaded successfully.
                var image = new Advertisement();
                image.Url = blockBlob.Uri.AbsoluteUri;
                image.FileName = adImage.FileName;

                Community community = null;
                if (id != null)
                {

                    community = await _context.Communities.FindAsync(id);
                    image.CommunityID = community.ID;
                    image.Community = community;
                }
                else
                {
                    return NotFound();
                }


                // add it to our array of answer images in our AnswerImageDataContext
                _context.Advertisements.Add(image);
                // update our data in the database with the new data added above.
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {

                // if the image fails to upload to our storage.
                View("Error");
            }
            // return to our index view.
            return RedirectToAction(nameof(Index), new { id = id });
        }
    }
}


