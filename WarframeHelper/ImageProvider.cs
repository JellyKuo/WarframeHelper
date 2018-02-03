using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Net;

namespace WarframeHelper
{
    class ImageProvider
    {
        List<CachedImage> ImgList;

        public ImageProvider()
        {
            if (File.Exists(@".\Cached\Index.dat"))
            {
                ImgList = JsonConvert.DeserializeObject<List<CachedImage>>(File.ReadAllText(@".\Cached\Index.dat"));
            }
            else
            {
                ImgList = new List<CachedImage>();
            }
        }

        public BitmapImage GetImage(string url)
        {
            foreach (var Img in ImgList)
            {
                if(Img.Url == url)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@".\Cached\"+Img.Filename,UriKind.Relative);
                    bitmap.EndInit();

                    return bitmap;
                }
            }

            var newImg = new CachedImage();
            newImg.Url = url;
            newImg.CreationTime = DateTime.Now;
            newImg.Filename = ImgList.Count.ToString();
            DlImgFile(url, @".\Cached\" + newImg.Filename);

            var newBmp = new BitmapImage();
            newBmp.BeginInit();
            newBmp.UriSource = new Uri(@".\Cached\" + newImg.Filename, UriKind.Relative);
            newBmp.EndInit();

            return newBmp;
        }

        private static void DlImgFile(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }

    }

    class CachedImage
    {
        public string Url { get; set; }
        public string Filename { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
