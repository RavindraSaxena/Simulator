using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobService
    {
        private static CloudBlobContainer profileBlobContainer;

        /// <summary>
        /// Initialize BLOB and Queue Here
        /// </summary>
        public BlobService()
        {
            //var storageAccount =
            //    CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=aspstoragecontainer;AccountKey=ryi+PLJ8q3ZeLzBdrFYuyCG+l/cfg3bpIskqJHRMoFbm9I/NgWd84v/QTYQyZSG1ToZXB60GHgPXzADKjTpbFg==;EndpointSuffix=core.windows.net");
            

            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            profileBlobContainer = new CloudBlobContainer(new Uri("https://aspstoragecontainer.blob.core.windows.net/?sv=2018-03-28&ss=b&srt=sco&sp=rw&se=2018-12-19T20:13:44Z&st=2018-12-17T12:13:44Z&spr=https&sig=AqrptJVeaN1OrOC8QhUOmpVFFhhelemEGXiAlBllkbw%3D"));


            // Get the blob container reference.
            //profileBlobContainer = blobClient.GetContainerReference("blobpoc");
            //Create Blob Container if not exist
            //profileBlobContainer.CreateIfNotExists();
        }


        /// <summary>
        /// Method to Upload the BLOB
        /// </summary>
        /// <param name=""profileFile"">
        /// <returns></returns>
        public void UploadBlob()
        {
            string blobName = "dynamic/"+GetDateFormat()+"/referencedata.json";
            string text = "Test";

            // GET a blob reference. 
            CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);
            profileBlob.UploadText(text);
            // Uploading a local file and Create the blob.
            //using (var stream = new MemoryStream(Encoding.Default.GetBytes(text), false))
            //{
            //     profileBlob.UploadFromStream(stream);
            //}
        }

        public string GetDateFormat()
        {
            var date = new DateTime(2019, 1, 3, 1, 1, 1);
            return  $"{date.Year}-{date.Month.ToString("00")}-{date.Day.ToString("00")}-{date.ToString("HH-mm")}"; ;
            //return $"{DateTime.UtcNow.Year}-{DateTime.UtcNow.Month.ToString("00")}-{DateTime.UtcNow.Day.ToString("00")}-{DateTime.UtcNow.ToString("HH-mm")}";
        }

    }
}
