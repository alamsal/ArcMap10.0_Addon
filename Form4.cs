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
    public partial class Form4 : Form
    {
        private IApplication m_application;

        public IApplication ArcMapApplication
        {
            get { return m_application; }
            set { m_application = value; }
        }

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueryGeodatabase queryGeodatabase = new QueryGeodatabase(m_application);
            queryGeodatabase.GetResultsFromGDB();
        }
    }
}
