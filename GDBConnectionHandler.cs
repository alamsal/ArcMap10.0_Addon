using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapClassLibrary2
{
    public class GDBConnectionHandler
    {
        #region properties

        public string GDBPath { get; set; }
        public string FeatureDatasetName { get; set; }
        public string FeatureClassName { get; set; }
        public IApplication ArcMapApplication { get; set; }

        public List<string> FeatureDatasetListInGDB { get; set; }
        public List<string> FeatureClassListInDataset { get; set; }

        #endregion properties

        #region members
        
        private Type factoryType;
        private IWorkspaceFactory workspaceFactory;
        private IMxDocument mxDocument;
        private IWorkspace workspace;

        #endregion members


        
        #region Ctor
        public GDBConnectionHandler()
        {
            factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
        }
         #endregion

        #region public functions
        public void GetAllFeatureDatasetsFromGDB()
        {


            if(CheckExistanceOfGDB())
            {
                try
                {
                    IFeatureWorkspace featureWorkspace = workspaceFactory.OpenFromFile(GDBPath, ArcMapApplication.hWnd) as IFeatureWorkspace;

                    workspace = featureWorkspace as IWorkspace;
                    FeatureDatasetListInGDB = GetAllDatasetsFromGDB(workspace);

                   // new CreateNewFeatureClass().CreateNewFeature(workspace, ArcMapApplication, "tt", "AnotherFeatureDataset");

                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }

       

        public void CreateNewFeatureDatasetInGDB()
        {
            //Do I need to create or not?
        }
        public void CreateNewFeatureInFeatureDataset()
        {
            // new CreateNewFeatureClass().CreateNewFeature(workspace, ArcMapApplication, "tt", "AnotherFeatureDataset");
        }
        
        public void UpdateFeatureClass()
        {
            FeatureClassListInDataset = GetAllFeatureClassFromDataset(workspace, FeatureDatasetName);
        }

        public void AddSelectedFeatureClassInMap()
        {

            AddFeatureClassToMap(workspace, FeatureDatasetName, FeatureClassName);

        }

        #endregion public methods

        #region private helpers
        private bool CreateNewGDB()
        {
            bool gdbStatus = false;

            string fileGDBName = Path.GetFileName(GDBPath);
            string pathDir = Path.GetDirectoryName(GDBPath);

            try
            {
                workspaceFactory.Create(pathDir, fileGDBName, null, ArcMapApplication.hWnd);

                gdbStatus = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not create File GDB" + e.Message);
            }

            return gdbStatus;

        }

        private bool CheckExistanceOfGDB()
        {
            bool existance = false;

            existance = Directory.Exists(GDBPath) || CreateNewGDB();

            return existance;
        }

        private List<string> GetAllDatasetsFromGDB(IWorkspace workspace)
        {
            List<string> datasetNameList = new List<string>();

            IEnumDataset dataset = workspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            
            if (dataset == null)
                return datasetNameList;

            dataset.Reset();
            
            IDataset dsn;
            
            while ((dsn = dataset.Next()) != null)
            {
                datasetNameList.Add(dsn.Name);
                
            }

            return datasetNameList;

        }

        private List<string> GetAllFeatureClassFromDataset(IWorkspace workspace, string fdsName)
        {
            List<string> featureClassList = new List<string>();

            IEnumDatasetName enumDS = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            
            //first FeatureDataset name
            IDatasetName featureDataSet = enumDS.Next();
            
            while (featureDataSet != null)
            {
                if( featureDataSet.Name==fdsName)
                {
                    //get all FeatureClasses name inside a FeatureDataset
                    IEnumDatasetName featureClassesInFDS = featureDataSet.SubsetNames;
                    IDatasetName singleFeatureClassAsDataset = featureClassesInFDS.Next();

                    while (singleFeatureClassAsDataset != null)
                    {
                        if (singleFeatureClassAsDataset is IFeatureClassName)
                        {
                            featureClassList.Add(singleFeatureClassAsDataset.Name);
                        }
                        singleFeatureClassAsDataset  = featureClassesInFDS.Next();
                    }

                }
                featureDataSet = enumDS.Next();
            }


            return featureClassList;


        }

        private void AddFeatureClassToMap(IWorkspace workspace, string datasetName ,string featureClassName )
        {
            try
            {
                mxDocument = ArcMapApplication.Document as IMxDocument;

                IEnumDataset enumDS = workspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset featureDataSet = enumDS.Next();

                while (featureDataSet != null)
                {
                    if (featureDataSet.Name == datasetName)
                    {
                        IEnumDataset featureClassesInFDS = featureDataSet.Subsets;
                        IDataset uniqueFeatrueClassAsDataSet = featureClassesInFDS.Next();

                        while (uniqueFeatrueClassAsDataSet != null)
                        {
                            if (uniqueFeatrueClassAsDataSet is IFeatureClass && uniqueFeatrueClassAsDataSet.Name == featureClassName)
                            {
                                IFeatureClass uniqueFeatureClass = uniqueFeatrueClassAsDataSet as IFeatureClass;
                                IFeatureLayer uniqueFeatrueLayer = new FeatureLayerClass();
                                uniqueFeatrueLayer.Name = uniqueFeatrueClassAsDataSet.Name;
                                uniqueFeatrueLayer.FeatureClass = uniqueFeatureClass;
                                mxDocument.AddLayer(uniqueFeatrueLayer);
                            }
                            uniqueFeatrueClassAsDataSet = featureClassesInFDS.Next();
                        }
                    }

                    featureDataSet = enumDS.Next();

                }
                mxDocument.ActiveView.Refresh();

            }
            catch(Exception e)
            {
                MessageBox.Show(" Unable to add feature class into ArcMap- " + e.ToString());
            }

        }








        #endregion private helpers
















    }

}
