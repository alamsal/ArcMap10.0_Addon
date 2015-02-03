using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;

namespace ArcMapClassLibrary2
{
    /// <summary>
    /// Summary description for QueryGDB.
    /// </summary>
    [Guid("04a31b07-59f8-494b-9692-0a9a5c856e72")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapClassLibrary2.QueryGDB")]
    public sealed class QueryGDBCommand : BaseCommand
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
        public QueryGDBCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "MyTestViewer"; //localizable text
            base.m_caption = "QueryGDB";  //localizable text
            base.m_message = "QueryGDB";  //localizable text 
            base.m_toolTip = "QueryGDB";  //localizable text 
            base.m_name = "QueryGDB";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".png";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;

            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            //Wpf example to make model popup
            /*
            QueryForm qs = new QueryForm();
            ArcMapWrapper wrapper= new ArcMapWrapper(m_application);

            var helper = new WindowInteropHelper(qs);
            helper.Owner = wrapper.Handle;
            qs.ShowInTaskbar = false;
            qs.ShowDialog();
            */

            
            //Wpf example to make wpf form
            /*
            QueryForm qs = new QueryForm();
            ArcMapWrapper wrapper = new ArcMapWrapper(m_application);

            var helper = new WindowInteropHelper(qs);
            helper.Owner = wrapper.Handle;
            qs.ShowInTaskbar = false;
            qs.Show();
            */

            Form4 fm4 = new Form4();
            ArcMapWrapper wrapper = new ArcMapWrapper(m_application);
            fm4.ArcMapApplication = m_application;
            fm4.ShowInTaskbar = false;
            fm4.Show(wrapper);
            
            





        }

        #endregion
    }
}
