using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLA
{
    //Просмотрщик больших файлов
    public class FileViewer : UserControl
    {
        Stream stream;
        int CharHeight;
        int CharWidth;

        public FileViewer(Stream stream)
        {
            this.stream = stream;
            //
            Font = new Font(FontFamily.GenericMonospace, 10);
            CharHeight = 15;
            CharWidth = 9;
            //
            AutoScroll = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            stream.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (ClientRectangle.Width > 0)
            {
                int symbolsPerPageLine = ClientRectangle.Width / CharWidth;
                int linesPerPage = ClientRectangle.Height / CharHeight;
                int linesCount = (int)stream.Length / symbolsPerPageLine;
                AutoScrollMinSize = new Size(1, 1 + linesCount + ClientRectangle.Height - linesPerPage);
                VerticalScroll.SmallChange = 1;
                VerticalScroll.LargeChange = ClientRectangle.Height / CharHeight;
                Invalidate();
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //
            int symbolsPerLine = ClientRectangle.Width / CharWidth;
            int linesPerPage = ClientRectangle.Height / CharHeight;
            int startPos = VerticalScroll.Value * symbolsPerLine;
            stream.Seek(startPos, SeekOrigin.Begin);
            //
            byte[] buffer = new byte[symbolsPerLine];
            //
            for (int y = 0; y < linesPerPage; y++)
                if (stream.Position < stream.Length)
                {
                    int count = stream.Read(buffer, 0, buffer.Length);
                    char[] chars = Encoding.Default.GetChars(buffer, 0, count);
                    for (int x = 0; x < chars.Length; x++)
                        e.Graphics.DrawString(chars[x].ToString(), Font, Brushes.Black, x * CharWidth, y * CharHeight);
                }
        }
    }
}
