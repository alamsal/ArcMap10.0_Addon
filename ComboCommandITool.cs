using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.SystemUI;

namespace ArcMapClassLibrary2
{
    /// <summary>
    /// Summary description for ComboCommandITool.
    /// </summary>
    [Guid("7f72fd9e-460d-408e-b027-5d668167ea3f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapClassLibrary2.ComboCommandITool")]
    public sealed class ComboCommandITool : ICommand, IToolControl
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
        private ComboBox myComboBox1 = null;
        public ComboCommandITool()
        {
            
        }

      

        public void OnCreate(object hook)
        {
            
            m_application = hook as IApplication;

            myComboBox1 = new ComboBox();
            myComboBox1.Width = 200;
            myComboBox1.Enabled = true;
            myComboBox1.Visible = true;
            // TODO:  Add other initialization code

            myComboBox1.Items.Add("Test1");
            myComboBox1.Items.Add("Test2");
            myComboBox1.Items.Add("Test3");
            myComboBox1.Items.Add("Test4");
            myComboBox1.Items.Add("Test5");
            myComboBox1.Items.Add("Test6");
            myComboBox1.SelectedIndex = 0;

            myComboBox1.SelectedIndexChanged += new System.EventHandler(ComboBox1_SelectedIndexChanged);
            
            
        }

       private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
       {
           MessageBox.Show(myComboBox1.SelectedIndex.ToString());
       }

        public void OnClick()
        {

        }
        public bool Enabled { get { return true; } }
        public bool Checked { get { return false; } }
        public string Name { get { return "Combo"; } }
        public string Caption { get { return "Combo"; } }
        public string Tooltip { get { return "Whatever"; } }
        public string Message { get { return ""; } }
        public string HelpFile { get { return ""; } }
        public int HelpContextID { get { return 0; } }
        public int Bitmap { get { return 0; } }
        public string Category { get { return ""; } }

        public int hWnd { get { return myComboBox1.Handle.ToInt32(); } }
        public void OnFocus(ICompletionNotify complete)
        {
            //complete.SetComplete();  // do not do this
            return;
        }
        public bool OnDrop(esriCmdBarType barType)
        {
            return true;
        }
    }
}
