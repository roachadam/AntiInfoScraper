using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntiInfoScraper
{
    public partial class Form1 : Form
    {
        private DataObfuscator _do;
        public Form1()
        {
            InitializeComponent();
            _do = new DataObfuscator();
        }
        

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            pbPreview.Image = _do.GetData(tbData.Text);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png";
                sfd.Title = "Save Image";

                if(sfd.ShowDialog() == DialogResult.OK)
                    pbPreview.Image.Save(sfd.FileName);
            }
        }
    }
}
