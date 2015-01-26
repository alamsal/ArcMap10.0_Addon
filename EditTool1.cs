using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapClassLibrary2
{
    /// <summary>
    /// Summary description for EditTool1.
    /// </summary>
    [Guid("536e517b-0296-455c-881a-db0b0e3a1673")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapClassLibrary2.EditTool1")]
    public sealed class EditTool1 : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
        public EditTool1()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "MyTestViewer"; //localizable text 
            base.m_caption = "Edit toolbar";  //localizable text 
            base.m_message = "toolbar";  //localizable text
            base.m_toolTip = "Edit toolbar";  //localizable text
            base.m_name = "Editdata_Tool";   //unique id, non-localizable (e.g. "MyCategory_ArcMapTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".png";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            m_application = hook as IApplication;

            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            //Create GDB addinto ArcMap
            StartUp();

            //Create tables inside GDB
            CreateTables createTable = new CreateTables();
            createTable.ArcMapApplication = m_application;
            createTable.GenerateTableSchema();
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {

            //Now editing starts

            //Access Map layer
            IMxDocument doc = m_application.Document as IMxDocument;
            

            
            
            //create geometery

            ESRI.ArcGIS.Display.IScreenDisplay screenDisplay = doc.ActiveView.ScreenDisplay;
            // Constant
            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast
            ESRI.ArcGIS.Display.IRgbColor rgbColor = new ESRI.ArcGIS.Display.RgbColorClass();
            rgbColor.Red = 255;

            ESRI.ArcGIS.Display.IColor color = rgbColor; // Implicit Cast
            ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
            simpleFillSymbol.Color = color;

            //Draw geometry
            ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
            ESRI.ArcGIS.Display.IRubberBand rubberBand = new ESRI.ArcGIS.Display.RubberPolygonClass();
            ESRI.ArcGIS.Geometry.IGeometry geometry = rubberBand.TrackNew(screenDisplay, symbol);
            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(geometry);
            screenDisplay.FinishDrawing();
            
            


            Form1 fm = new Form1();
            fm.ArcMapApplication = m_application;
            fm.PolygonGeometry = geometry;
            fm.Show();

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add EditTool1.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add EditTool1.OnMouseUp implementation
        }
        #endregion

        public void StartUp()
        {
            try
            {
                //create gdb
                const string path = "D:/Ashis_Work/TCCDefects/SampleDatasets/NewShp";
                const string fileGDBName = "sample.gdb";
                const string fileGDBAddress = path + "/" + fileGDBName;
                const string featureDatasetname = "polygonFeatureClasses";
                const string featureClassname = "MytestPolygons";

                if (System.IO.Directory.Exists(fileGDBAddress))
                {
                    Console.WriteLine("already exist");
                    // return;
                }

                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                IWorkspaceName workspaceName = workspaceFactory.Create(path, fileGDBName, null, m_application.hWnd);

                System.Windows.Forms.MessageBox.Show("gbd created!");


                //Create feature dataset
                IFeatureWorkspace fws = workspaceFactory.OpenFromFile(fileGDBAddress, m_application.hWnd) as IFeatureWorkspace;

                //check the existance of FeatureDataset
                IWorkspace2 ws = fws as IWorkspace2;

                IWorkspace ws1 = ws as IWorkspace;

                if (ws.get_NameExists(esriDatasetType.esriDTFeatureDataset, featureDatasetname) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Feature dataset is already exists");
                }


                //Create a spatial refrence/extract spatial refrence from map
                //Infuture will access through the arcmap

                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                int coordinateSystemID = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
                ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem(coordinateSystemID);

                //Create featuredataset

                IFeatureDataset fds = fws.CreateFeatureDataset(featureDatasetname, spatialReference);

                //Create featureClass
                IFieldsEdit fields = new FieldsClass();
                fields.FieldCount_2 = 3;

                IFieldEdit field = new FieldClass();
                field.Name_2 = "ObjectID";
                field.Type_2 = esriFieldType.esriFieldTypeOID;

                fields.Field_2[0] = field;

                IGeometryDefEdit geometryDef = new GeometryDefClass();
                geometryDef.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                geometryDef.SpatialReference_2 = spatialReference;

                field = new FieldClass();
                field.Name_2 = "Shape";
                field.Type_2 = esriFieldType.esriFieldTypeGeometry;
                field.GeometryDef_2 = geometryDef;

                fields.Field_2[1] = field;

                field = new FieldClass();
                field.Name_2 = "Name_Test";
                field.Type_2 = esriFieldType.esriFieldTypeString;

                fields.Field_2[2] = field;


                fds.CreateFeatureClass(featureClassname, fields, null, null, esriFeatureType.esriFTSimple, "shape", null);

                //Active dataframe
                IMxDocument mxDocument1 = m_application.Document as IMxDocument;
                IWorkspaceFactory wsf1 = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                IFeatureWorkspace fws1 = wsf1.OpenFromFile(fileGDBAddress, m_application.hWnd) as IFeatureWorkspace;

                IWorkspace ws2 = fws as IWorkspace;

                //get alll feature datasets inside fileGDB
                //Add featureclass from geodatabase to arcmap
                IEnumDataset enumDS = ws2.get_Datasets(esriDatasetType.esriDTFeatureDataset);

                try
                {
                    //first FeatureDataset
                    IDataset featureDataSet = enumDS.Next();
                    while (featureDataSet != null)
                    {
                        //get all FeatureClasses inside a FeatureDataset
                        IEnumDataset featureClassesInFDS = featureDataSet.Subsets;
                        IDataset singleFeatureClassAsDataset = featureClassesInFDS.Next();

                        while (singleFeatureClassAsDataset != null)
                        {
                            if (singleFeatureClassAsDataset is IFeatureClass)
                            {
                                IFeatureClass singleFeatureClass = singleFeatureClassAsDataset as IFeatureClass;
                                IFeatureLayer featureLayer = new FeatureLayerClass();
                                featureLayer.Name = singleFeatureClassAsDataset.Name;
                                featureLayer.FeatureClass = singleFeatureClass;
                                mxDocument1.AddLayer(featureLayer);
                            }
                            singleFeatureClassAsDataset = featureClassesInFDS.Next();
                        }
                        featureDataSet = enumDS.Next();
                    }
                    mxDocument1.ActiveView.Refresh();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Feature Error: " + e.Message);
                }























            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }









        }




    }
}
