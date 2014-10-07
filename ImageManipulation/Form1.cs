using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ImageManipulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UserInitializeComponent();
        }

        /// <summary>
        /// All initialization related code that would upset the Visual Studio designer.
        /// </summary>
        private void UserInitializeComponent()
        {
            foreach (Type t in FilterFramework.FindFilters())
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = t.Name;
                menuItem.Click += menuItem_Click;
                this.filtersToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        void menuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var filterName = menuItem.Text;

            var allFilters = FilterFramework.FindFilters();
            var filterType = allFilters.Where(f => f.Name == filterName).FirstOrDefault();
            var filter = (IImageFilter)Activator.CreateInstance(filterType);
            this.pictureBox1.Image = FilterFramework.ApplyFilter(this.pictureBox1.Image, filter);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "JPEG Images|*.jpg";
            dialog.ShowDialog();
            var imagePath = dialog.FileName;

            if (dialog.FileName == string.Empty)
            {
                return;
            }

            this.pictureBox1.Image = GetImageFromFile(dialog.FileName);
        }


        private static Image GetImageFromFile(string path)
        {
            FileStream fs;

            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to open file " + path + ". Error: " + exception.Message);
                return null;
            }

            JpegBitmapDecoder decoder = new JpegBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create(bitmapSource));
                enc.Save(outStream);

                fs.Dispose();

                return new Bitmap(outStream);
            }
        }
    }
}
