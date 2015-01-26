using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;

namespace ArcMapClassLibrary2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public IApplication MapApplication { get; set; }

        private DialogResult result;

        private GDBConnectionHandler connectionHandler = new GDBConnectionHandler();


        private void Select_Click(object sender, EventArgs e)
        {
          try
          {
              FolderBrowserDialog fbd = new FolderBrowserDialog();
              result = fbd.ShowDialog();

              if (result == DialogResult.OK)
              {
                  //textBox1.Text = fbd.SelectedPath;
                  textBox1.Text = @"D:\Ashis_Work\TCCDefects\SampleDatasets\NewShp\sample.gdb";

              }


             
              connectionHandler.ArcMapApplication = MapApplication;
              connectionHandler.GDBPath = textBox1.Text;

              connectionHandler.GetAllFeatureDatasetsFromGDB();


              comboBox1.DataSource = connectionHandler.FeatureDatasetListInGDB;
             
             

          }
            catch(Exception mException)
            {
                MessageBox.Show(mException.ToString());
            }

            






        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectionHandler.FeatureDatasetName = comboBox1.SelectedValue.ToString();
            connectionHandler.UpdateFeatureClass();
            comboBox2.DataSource = connectionHandler.FeatureClassListInDataset;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            connectionHandler.FeatureClassName = comboBox2.SelectedValue.ToString();
            connectionHandler.AddSelectedFeatureClassInMap();
        }

        private void NewFeatureDatasets_Click(object sender, EventArgs e)
        {

        }




    }
}
