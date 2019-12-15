using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class AzureBlobServiceUtil
    {
        static CloudBlobClient blobClient;

        const string blobContainerName = "webappstoragedotnet-imagecontainer";

        static CloudBlobContainer blobContainer;

        private static bool IsInitialised = false;


        //To initialize it
        public static async Task<bool> Init()
        {
            if (!IsInitialised)
            {
                // Retrieve storage account information from connection string
                              

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
                                
                // Create a blob client for interacting with the blob service.

                blobClient = storageAccount.CreateCloudBlobClient();

                blobContainer = blobClient.GetContainerReference(blobContainerName);

                await blobContainer.CreateIfNotExistsAsync();
                                                                
                await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                
                IsInitialised = true;
            }
            return true;
        }
                      

        public static async Task<List<Uri>> DisplayAll()
        {

            try

            {
                await Init();

                // Gets all Cloud Block Blobs in the blobContainerName and passes them to the view

                List<Uri> allBlobs = new List<Uri>();

                foreach (IListBlobItem blob in blobContainer.ListBlobs())
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                    {
                        allBlobs.Add(blob.Uri);
                                                
                    }
                }

                return allBlobs;

            }

            catch (Exception ex)
            {
                
                throw ex;
            }

        }
                               
        public static async Task<string> UploadAsync(byte[] imageInbytes, string filename)
        {
            try
            {
                await Init();
                
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetBlobName(filename));

                MemoryStream ms = new MemoryStream(imageInbytes);
                await blob.UploadFromStreamAsync(ms);

                var blobname = blob.Uri.ToString();
                return blobname;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
                          
        
        public async Task<bool> DeleteImage(string name)
        {
            try
            {
                Uri uri = new Uri(name);

                string filename = "";

                var blob = blobContainer.GetBlockBlobReference(filename);

                await blob.DeleteIfExistsAsync();

                return true;
            }
            catch (Exception)
            {                               
                return false;
            }

        }
                            

        public async Task<bool> DeleteAll()
        {
            try
            {
                foreach (var blob in blobContainer.ListBlobs())
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))

                    {

                        await ((CloudBlockBlob)blob).DeleteIfExistsAsync();

                    }
                }
                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }
                       

        private static string GetBlobName(string filename)
        {
            return string.Format("{0:yyyyMMddHHmmss}_{1}", DateTime.Now, filename);
        }

    }
}

