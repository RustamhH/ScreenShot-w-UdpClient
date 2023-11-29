using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;


    

byte[] CaptureAndSaveScreenshot()
{

    
    int width = 1920;
    int height = 1080;

    using (Bitmap bmp = new Bitmap(width, height))
    {
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
        }

        


        using (MemoryStream stream = new MemoryStream())
        {
            bmp.Save(stream, ImageFormat.Jpeg);


            return stream.ToArray();
        }




    }

    
    

}


UdpClient udpClient = new UdpClient(27001);
var remoteEP = new IPEndPoint(IPAddress.Any, 0);

while (true)
{
    var result=await udpClient.ReceiveAsync();
    
    new Task(async () =>
    {
        remoteEP = result.RemoteEndPoint;
        while(true)
        {
            byte[] image=CaptureAndSaveScreenshot();
            var chunks = image.Chunk(ushort.MaxValue - 29);
            foreach (var item in chunks)
            {
                await udpClient.SendAsync(item,item.Length,remoteEP);
            }
        }
    }).Start();

    
}
        
    
