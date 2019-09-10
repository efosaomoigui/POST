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

                // How to create a storage connection string - http://msdn.microsoft.com/en-us/library/azure/ee758697.aspx

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());



                // Create a blob client for interacting with the blob service.

                blobClient = storageAccount.CreateCloudBlobClient();

                blobContainer = blobClient.GetContainerReference(blobContainerName);

                await blobContainer.CreateIfNotExistsAsync();



                // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  

                // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  

                // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  

                // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 

                await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });


                IsInitialised = true;
            }
            return true;
        }


        /// <summary> 

        /// Task<ActionResult> Index() 

        /// Documentation References:  

        /// - What is a Storage Account: http://azure.microsoft.com/en-us/documentation/articles/storage-whatis-account/ 

        /// - Create a Storage Account: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#create-an-azure-storage-account

        /// - Create a Storage Container: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#create-a-container

        /// - List all Blobs in a Storage Container: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#list-the-blobs-in-a-container

        /// </summary> 

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

                        //clear the entire system
                        // await ((CloudBlockBlob)blob).DeleteIfExistsAsync();
                    }
                }

                return allBlobs;

            }

            catch (Exception ex)
            {

                //ViewData["message"] = ex.Message;

                //ViewData["trace"] = ex.StackTrace;

                throw ex;
            }

        }



        /// <summary> 

        /// Task<ActionResult> UploadAsync() 

        /// Documentation References:  

        /// - UploadFromFileAsync Method: https://msdn.microsoft.com/en-us/library/azure/microsoft.windowsazure.storage.blob.cloudpageblob.uploadfromfileasync.aspx

        /// </summary> 


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
                //ViewData["message"] = ex.Message;

                //ViewData["trace"] = ex.StackTrace;
                throw ex;
            }

        }



        /// <summary> 

        /// Task<ActionResult> DeleteImage(string name) 

        /// Documentation References:  

        /// - Delete Blobs: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#delete-blobs

        /// </summary> 


        public async Task<bool> DeleteImage(string name)
        {
            try
            {
                Uri uri = new Uri(name);

                string filename = "";   // Path.GetFileName(uri.LocalPath);

                var blob = blobContainer.GetBlockBlobReference(filename);

                await blob.DeleteIfExistsAsync();

                return true;
            }
            catch (Exception ex)
            {

                //ViewData["message"] = ex.Message;

                //ViewData["trace"] = ex.StackTrace;

                return false;
            }

        }



        /// <summary> 

        /// Task<ActionResult> DeleteAll(string name) 

        /// Documentation References:  

        /// - Delete Blobs: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#delete-blobs

        /// </summary> 

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

            catch (Exception ex)
            {
                //ViewData["message"] = ex.Message;

                //ViewData["trace"] = ex.StackTrace;

                return false;
            }

        }


        /// <summary> 

        /// string GetRandomBlobName(string filename): Generates a unique random file name to be uploaded  

        /// </summary> 

        private static string GetBlobName(string filename)
        {
            return string.Format("{0:yyyyMMddHHmmss}_{1}", DateTime.Now, filename);
        }

    }
}

