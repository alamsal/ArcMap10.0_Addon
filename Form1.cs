using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapClassLibrary2
{
    public partial class Form1 : Form
    {
        private IApplication m_application;

        public IApplication ArcMapApplication
        {
            get { return m_application; }
            set { m_application = value; }
        }

        private IGeometry _geometry;

        public IGeometry PolygonGeometry
        {
            get { return _geometry; }
            set { _geometry = value; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IMxDocument doc = m_application.Document as IMxDocument;
                IMap map = doc.FocusMap;
                ILayer mapLayer = map.get_Layer(0);


                //Access workspace
                const string path = "D:/Ashis_Work/TCCDefects/SampleDatasets/NewShp";
                const string fileGDBName = "sample.gdb";
                const string fileGDBAddress = path + "/" + fileGDBName;
                const string featureDatasetname = "polygonFeatureClasses";
                const string featureClassname = "MytestPolygons";

                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);


                //Create feature dataset
                IFeatureWorkspace featureWorkspace = workspaceFactory.OpenFromFile(fileGDBAddress, m_application.hWnd) as IFeatureWorkspace;

                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(featureClassname);
                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)featureWorkspace;




                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();





                IFeature feature = featureClass.CreateFeature();
                feature.Shape = PolygonGeometry;
                feature.set_Value(featureClass.FindField("Name_Test"), textBox1.Text);
                feature.Store();



                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                map.RecalcFullExtent();
                doc.ActiveView.Refresh();

                



                MessageBox.Show("saved");
                this.Close();
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
