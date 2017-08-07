using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            //MessageBox.Show(tLayout.ColumnCount + "x" + tLayout.RowCount);
        }

        public static Bitmap ImageToByte(System.Drawing.Image img)
        {
            Bitmap bm;
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                bm = new Bitmap(stream);
                return bm;
            }
        }


        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image files (jpg, jpeg, bmp, png)|*.jpg;*.jpeg;*.bmp;*.png";
            fd.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    PictureBox pb = new PictureBox();
                    pb.Size = new Size(board.Width, board.Height);
                    //pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Load(fd.FileName);

                    //MessageBox.Show("" + pb.Image.Width + " " + pb.Image.Height);
                    // this.Controls.Add(pb);

                    //pb.BringToFront();

                    // Bitmap bp = new Bitmap(board.Width, board.Height);

                    pb.Image = ResizeImage(pb.Image, 640, 480);


                    //MessageBox.Show("" + pb.Image.Width + " " + pb.Image.Height);


                    explode((Bitmap)pb.Image, board.ColumnCount, board.RowCount);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }

            }


        }

       // private Image[] puzl_im;
             
       
        private void explode(Bitmap bm, int x, int y)
        {
            int c_w = bm.Width / board.ColumnCount;
            int c_h = bm.Height / board.RowCount;

            int pos_y = 0;

            for (int i = 0; i < x; i++)
            {
                int pos_x = 0;
                

                for (int j = 0; j < y; j++)
                {
                    //int w = (int)board.ColumnStyles[i].Width;
                    //int h = (int)board.RowStyles[i].Height;
                    PictureBox im = new PictureBox();
                    
                    //im.SizeMode = PictureBoxSizeMode.Zoom;
                    im.Size = new Size(c_w,c_h);
                    im.Margin = new Padding(0);
                    im.Padding = new Padding(0);
                    //im.SizeMode = PictureBoxSizeMode.StretchImage;
                    board.Controls.Add(im);
                    
                    TableLayoutPanelCellPosition cellPos = new TableLayoutPanelCellPosition(j,i);
                    im.Image = bm.Clone(new Rectangle(pos_x, pos_y, c_w, c_h)
                                                     , System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    
                    board.SetCellPosition(im, cellPos);
                    im.BringToFront();
                    pos_x = pos_x + c_w;
                    //im.Dispose();
                }
                pos_y = pos_y + c_h;
                //                    puzl_im = new Bitmap(bm, new Rectangle(tLayout.GetColumn(Col))

            }
        }

    }
}

