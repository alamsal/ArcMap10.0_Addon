using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;

namespace ArcMapClassLibrary2
{
    class ArcMapWrapper:IWin32Window
    {
        private IApplication _arcMapApplication;

        public ArcMapWrapper( IApplication mApplication)
        {
            _arcMapApplication = mApplication;
        }

        public IntPtr Handle
        {
            get { return new IntPtr(_arcMapApplication.hWnd); }
            
        }
    }
}
