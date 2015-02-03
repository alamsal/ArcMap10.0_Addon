using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapClassLibrary2
{
    public class QueryGeodatabase
    {
        private IApplication _arcMapApplication;
        
        public QueryGeodatabase(IApplication application)
        {
            _arcMapApplication = application;
        }


        public void GetResultsFromGDB()
        {
            //Access workspace
            const string path = "D:/Ashis_Work/TCCDefects/SampleDatasets/NewShp";
            const string fileGDBName = "sample.gdb";
            const string fileGDBAddress = path + "/" + fileGDBName;


            IWorkspace workspace = FileGdbWorkspaceFromPath(fileGDBAddress);

            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
            
            // Create the query definition.
            IQueryDef queryDef = featureWorkspace.CreateQueryDef();

            //Setting the SubFields, Tables, and WhereClause:
            //Single table with a WhereClause
            //queryDef.Tables = "STATES";
            //queryDef.SubFields = "*";
            //queryDef.WhereClause = "STATE_NAME = 'California'";

            try
            {
                // Provide a list of tables to join.
                queryDef.Tables = "Associated_MXDs,Model_Type,Production_Process";
                // Set the subfields and the where clause (the join condition in this case).
                queryDef.SubFields = "Associated_MXDs.Defect_FID,Associated_MXDs.MXD_ID,Model_Type.Model_Type,Production_Process.Process_Name";
                queryDef.WhereClause = "Associated_MXDs.Defect_FID=Model_Type.Defect_FID AND Associated_MXDs.Defect_FID=Production_Process.Defect_FID";

                //Using Evaluate:
                ICursor cursor;
                cursor = queryDef.Evaluate();

                IRow row = null;
                row = cursor.NextRow();

                int defectIndex = cursor.FindField("Associated_MXDs.Defect_FID");
                int mxdIndex = cursor.FindField("Associated_MXDs.MXD_ID");
                int modelTypeIndex = cursor.FindField("Model_Type.Model_Type"); 
                int processNameIndex = cursor.FindField("Production_Process.Process_Name"); 
                
                while (row!=null)
                {

                    MessageBox.Show(row.get_Value(defectIndex) + "--" + row.get_Value(mxdIndex) + "--" + row.get_Value(modelTypeIndex)+"--"+ row.get_Value(processNameIndex));
                    row = cursor.NextRow();
                    
                }




            }
            catch(Exception e)
            {
                MessageBox.Show("Can not query! :( " + e.Message);
            }

        }

        private IWorkspace FileGdbWorkspaceFromPath(String path)
        {
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            return workspaceFactory.OpenFromFile(path, _arcMapApplication.hWnd);
        }









    }
}
