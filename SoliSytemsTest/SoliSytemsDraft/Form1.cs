using RestEase;
using SoliSytemsDraft.Interface;
using SoliSytemsDraft.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using OpenXmlPowerTools;

namespace SoliSytemsDraft
{
    public partial class SoliSystems : Form
    {
        public SoliSystems()
        {
            InitializeComponent();          
        }
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFname.Text = "";
            txtAge.Text = "";
            txtGender.Text = "";
            txtNationality.Text = "";
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            double tempprob = 0; string countryid = string.Empty;
            if (txtFname.Text == "")
            {
                MessageBox.Show("Please enter First Name");
            }
            else
            {
                
                // Fetch age from API
                IRestEase ageapi = RestClient.For<IRestEase>("https://api.agify.io/");
                UserDetails objuserage = ageapi.GetAgeAsync(txtFname.Text).Result;
                txtAge.Text = objuserage.age.ToString();

                //Fetch gender from API
                IRestEase genderapi = RestClient.For<IRestEase>("https://api.genderize.io/");
                UserDetails objusergender = genderapi.GetGenderAsync(txtFname.Text).Result;
                txtGender.Text = objusergender.gender;

                //Fetch Nationality from API
                IRestEase countryapi = RestClient.For<IRestEase>("https://api.nationalize.io/");
                UserDetails objusercountry = countryapi.GetCountryAsync(txtFname.Text).Result;
                               
                foreach (var item in objusercountry.country)
                    {                              
                    if (tempprob == 0)
                        {
                            countryid = item.country_Id;
                            tempprob = item.probability;
                        }
                      else
                        {
                            if(tempprob < item.probability)
                              {
                                 tempprob = item.probability;
                                 countryid = item.country_Id;  
                              }
                        }
                      
                    }
                txtNationality.Text = countryid;
            }        
        
        }

        private void btnExit_Click(object sender, EventArgs e)
        {           
            DialogResult = MessageBox.Show("Are you sure you want to exit the application","Exit Confirmation", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                SoliSystems.ActiveForm.Close();
            }              
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           //Fetch system cameras and capture video
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cmbCamera.Items.Add(filterInfo.Name);
                cmbCamera.SelectedIndex = 0;
                videoCaptureDevice = new VideoCaptureDevice();
            }          
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cmbCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            PlayVideo.Image = (Bitmap)eventArgs.Frame.Clone();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning == true)
                videoCaptureDevice.Stop();           
        }

        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            videoCaptureDevice.Stop();
        }
    }
}
