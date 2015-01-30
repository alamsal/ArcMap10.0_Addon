using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.SystemUI;
using Microsoft.Win32;
namespace ArcMapClassLibrary2
{
    /// <summary>
    /// Summary description for ComboCommandIComboBox.
    /// </summary>
    [Guid("30dc8175-3468-4e95-96c2-96976a1bb85c")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapClassLibrary2.ComboCommandIComboBox")]
    public sealed class ComboCommandIComboBox : BaseCommand,IComboBox
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

    private IMxDocument m_doc;
    private int m_cookie;
    private IComboBoxHook m_comboBoxHook;
    private Dictionary<int, string> m_list;

    public ComboCommandIComboBox()
    {
        base.m_category = "MyTestViewer";
      base.m_caption = "Selection Target";
      base.m_message = "Select the selection target C#.";
      base.m_toolTip = "Select the selection target C#.";  
      base.m_name = "ESRI_SelectionCOMSample_SelectionTargetComboBox";

      m_cookie = -1;
      


      try
      {
        base.m_bitmap = new Bitmap(GetType().Assembly.GetManifestResourceStream("SelectionCOMSample.Images.SelectionTargetComboBox.png"));
      }
      catch (Exception ex)
      {
        System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
      }
    }



    #region Overriden Class Methods

    /// <summary>
    /// Occurs when this command is created
    /// </summary>
    /// <param name="hook">Instance of the application</param>
    public override void OnCreate(object hook)
    {
      if (hook == null)
        return;

      m_comboBoxHook = hook as IComboBoxHook;

      IApplication application = m_comboBoxHook.Hook as IApplication;
      m_doc = application.Document as IMxDocument;

      //Disable if it is not ArcMap
      if (m_comboBoxHook.Hook is IMxApplication)
        base.m_enabled = true;
      else
        base.m_enabled = false;
      
        //
      
        m_list = new Dictionary<int, string>();

        m_cookie = m_comboBoxHook.Add("test1");
        m_list.Add(m_cookie, "test1");

        m_cookie = m_comboBoxHook.Add("test2");
        m_list.Add(m_cookie, "test2");

        m_cookie = m_comboBoxHook.Add("test3");
        m_list.Add(m_cookie, "test3");

       // m_comboBoxHook.Select(m_cookie);
        m_comboBoxHook.Value = "test";

    }

    /// <summary>
    /// Occurs when this command is clicked
    /// </summary>
    public override void OnClick()
    {

    }

    #endregion

    #region IComboBox Members

    public int DropDownHeight
    {
      get { return 4; }
    }

    public string DropDownWidth
    {
      get { return "WWWWWWWWWWWWWWWWW"; }
    }

    public bool Editable
    {
      get { return true; }
    }

    public string HintText
    {
      get { return "Choose a ashis test layer."; }
    }

    public void OnEditChange(string editString)
    {
    }

    public void OnEnter()
    {
    }

    public void OnFocus(bool set)
    {
    }

    public void OnSelChange(int cookie)
    {
        bool exitloop = false;
      if (cookie == -1)
        return;
       
      foreach (KeyValuePair<int, string> item in m_list)
      {
        //All feature layers are selectable if "Select All" is selected;
        //otherwise, only the selected layer is selectable.
        
        //string fl = item.Value;
        //if (fl == null)
        //  continue;

        if(cookie==item.Key)
        {
            MessageBox.Show(item.Value);
           
            break;

        }
          
        


      }
     

      //Fire ContentsChanged event to cause TOC to refresh with new selected layers.
      m_doc.ActiveView.ContentsChanged(); ;

    }

    public bool ShowCaption
    {
      get { return true; }
    }

    public string Width
    {
      get { return "WWWWWWWWWWWWWX"; }
    }

    #endregion
    }
}
