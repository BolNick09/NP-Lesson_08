using System.Diagnostics;
using System.Net;

namespace FTPDownloadFile
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            string ftpUrl = "ftp://ftp.dlptest.com/";
            string user = "dlpuser";
            string password = "rNrKYTX9g7z3RgJRmxWuGHbeu";
            string filePath = "Testfile311.txt";
            string fileUrl = $"{ftpUrl}{filePath}";



            FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
            uploadRequest.Credentials = new NetworkCredential(user, password);

            Stopwatch stopwatchUpload = Stopwatch.StartNew();

            byte[] fileContents;
            using (FileStream fs = File.OpenRead(filePath))
            {
                fileContents = new byte[fs.Length];
                await fs.ReadAsync(fileContents, 0, fileContents.Length);
            }

            using Stream requestStream = await uploadRequest.GetRequestStreamAsync();
            await requestStream.WriteAsync(fileContents, 0, fileContents.Length);

            stopwatchUpload.Stop();
            Console.WriteLine("Файл успешно отправлен.");

            double uploadTime = stopwatchUpload.Elapsed.TotalSeconds;
            long fileSize = new FileInfo(filePath).Length;

            double uploadSpeed = fileSize / uploadTime; 
            Console.WriteLine($"Время отправки: {uploadTime:N2} сек.");
            Console.WriteLine($"Скорость отправки: {uploadSpeed / 1024 / 1024:N2} Мбайт/сек.");


            FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
            downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            downloadRequest.Credentials = new NetworkCredential(user, password);//удостоверение, лог-пасс


            Stopwatch stopwatchDownload = Stopwatch.StartNew();
            //Выполнение запроса
            //на этой строке зависает, не пойму почему
            using FtpWebResponse response =  (FtpWebResponse) await downloadRequest.GetResponseAsync();
            if ((int)response.StatusCode < 100 || (int)response.StatusCode >= 400)
                throw new InvalidOperationException("Failed status response");

            Stream file = File.Create(filePath);
            await response.GetResponseStream().CopyToAsync(file);

            Console.WriteLine("Файл успешно скачен и записан");
            
            double downloadTime = stopwatchDownload.Elapsed.TotalSeconds;

            double downloadSpeed = fileSize / downloadTime;
            Console.WriteLine($"Время получения: {downloadTime:N2} сек.");
            Console.WriteLine($"Скорость получения: {downloadSpeed / 1024 / 1024:N2} Мбайт/сек.");

            
        }
    }
}
