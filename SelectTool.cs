using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;


namespace ArcMapClassLibrary2
{
    /// <summary>
    /// Summary description for SelectTool.
    /// </summary>
    [Guid("8b98661d-867c-4cda-8967-97c10cd54a7a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapClassLibrary2.SelectTool")]
    public sealed class SelectTool : BaseTool
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
        
        public SelectTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "MyTestViewer"; //localizable text 
            base.m_caption = "Select toolbar";  //localizable text 
            base.m_message = "Select toolbar";  //localizable text
            base.m_toolTip = "Select toolbar";  //localizable text
            base.m_name = "Selectdata_Tool";   //unique id, non-localizable (e.g. "MyCategory_ArcMapTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
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
            // TODO: Add SelectTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            IMxDocument doc = m_application.Document as IMxDocument;
            IMap map=null;
            
            IPoint clickedPoint = doc.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            
            if(doc.ActiveView is Map)
            {
                map = doc.FocusMap;
            }


            IActiveView activeView = (IActiveView)map;
            IRubberBand rubberEnv = new RubberEnvelopeClass();
            IGeometry geom = rubberEnv.TrackNew(activeView.ScreenDisplay, null);
            IArea area = (IArea)geom;

            //Extra logic to cater for the situation where the user simply clicks a point on the map 
            //or where envelope is so small area is zero 
            if ((geom.IsEmpty == true) || (area.Area == 0))
            {

                //create a new envelope 
                IEnvelope tempEnv = new EnvelopeClass();

                //create a small rectangle 
                ESRI.ArcGIS.esriSystem.tagRECT RECT = new tagRECT();
                RECT.bottom = 0;
                RECT.left = 0;
                RECT.right = 5;
                RECT.top = 5;

                //transform rectangle into map units and apply to the tempEnv envelope 
                IDisplayTransformation dispTrans = activeView.ScreenDisplay.DisplayTransformation;
                dispTrans.TransformRect(tempEnv, ref RECT, 4); //4 = esriDisplayTransformationEnum.esriTransformToMap)
                tempEnv.CenterAt(clickedPoint);
                geom = (IGeometry)tempEnv;
            }

            //Set the spatial reference of the search geometry to that of the Map 
            ISpatialReference spatialReference = map.SpatialReference;
            geom.SpatialReference = spatialReference;

            map.SelectByShape(geom, null, false);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);
            
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add SelectTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add SelectTool.OnMouseUp implementation
        }
        #endregion


       




    }
}
