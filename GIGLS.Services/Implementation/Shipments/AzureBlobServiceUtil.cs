using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Shipments
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
            catch (Exception)
            {
                throw;
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

        public static async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                string strContainerName = "uploads";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = GenerateFileName(strFileName);

                //if (await cloudBlobContainer.CreateIfNotExistsAsync())
                //{
                //    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                //}

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }


        public static async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            string strContainerName = "uploads";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
            CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
            // get block blob refarence    
            CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

            // delete blob from container        
            await blockBlob.DeleteAsync();
        }


    }
}

