using System;

namespace BlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {

            BlobService b = new BlobService();
            //b.UploadBlob();
            Console.WriteLine(b.GetDateFormat());
            Console.ReadLine();
        }
    }
}
