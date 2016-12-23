using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WebTask.Controllers.Provider
{
    public class ImageProvider
    {
        private readonly Font font;
        private readonly Color textColor;
        private readonly Color backColor;

        public ImageProvider()
            :this(new Font("Microsoft Sans Serif", 14f), Color.Black, Color.White)
        {
        }

        public ImageProvider(Font font, Color textColor, Color backColor)
        {
            this.font = font;
            this.textColor = textColor;
            this.backColor = backColor;
        }

        public byte[] GetImage(string text)
        {
            var textSize = GetMeasureString(text, font);
            using (var img = new Bitmap((int)textSize.Width, (int)textSize.Height))
            using (var drawing = Graphics.FromImage(img))
            using (Brush textBrush = new SolidBrush(textColor))
            using (var stream = new MemoryStream())
            {
                drawing.Clear(backColor);
                drawing.DrawString(text, font, textBrush, 0, 0);
                drawing.Save();
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private static SizeF GetMeasureString(string text, Font font)
        {
            using (var img = new Bitmap(1, 1))
            using (var drawing = Graphics.FromImage(img))
            {
                return drawing.MeasureString(text, font);
            }
        }
    }
}