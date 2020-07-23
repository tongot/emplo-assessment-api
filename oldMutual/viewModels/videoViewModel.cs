using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace oldMutual.viewModels
{
    public class videoViewModel
    {
        private readonly string _filename;
        public videoViewModel(string filename)
        {
            _filename = filename;
        }

        public async void WriteContentToSteam(Stream outputStream, HttpContent content, TransportContext transportContext)
        {

            //path of the video to play
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/videos/"+_filename);

            try
            {
                int bufferSize = 1000000;
                byte[] buffer = new byte[bufferSize];

                //reading file from using file sttream
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int totalSize = (int)fileStream.Length;
                    //reade the data from the stream aslong as the size is greater than zero 
                    while (totalSize > 0)
                    {
                        int count = totalSize > bufferSize ? bufferSize : totalSize;
                        //reading the buffer from the priginal file 
                        int sizeOfReadedBuffer = fileStream.Read(buffer,0, count);
                        //writing the readed buffer to output
                        await outputStream.WriteAsync(buffer, 0, sizeOfReadedBuffer);
                        //decrement the ouptput to the total size of the file
                        totalSize -= sizeOfReadedBuffer;

                    }
                }

            }
            catch (HttpException ex)
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
            //set the size of the buffer {set any size}


        }

    }
}