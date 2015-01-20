using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace ArcMapClassLibrary2
{
    public class CreateTables
    {
        private IApplication m_application;

        public IApplication ArcMapApplication
        {
            get { return m_application; }
            set { m_application = value; }
        }
        
        public void GenerateTableSchema()
        {
            //Access workspace
            const string path = "D:/Ashis_Work/TCCDefects/SampleDatasets/NewShp";
            const string fileGDBName = "sample.gdb";
            const string fileGDBAddress = path + "/" + fileGDBName;


            IWorkspace workspace = FileGdbWorkspaceFromPath(fileGDBAddress);
 

            CreateAssociatedMXDTable(workspace, "Associated_MXDs");
            CreateLandsatScenesTable(workspace, "Landsat_Scenes");
            CreateSourceTable(workspace, "Source");
            CreateModelTypeTable(workspace, "Model_Type");
            CreateMRLCVintageTable(workspace,"MRLC_Vintage");
            CreateProductionProcessTable(workspace,"Production_Process");

        }

        private IWorkspace FileGdbWorkspaceFromPath(String path)
        {
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            return workspaceFactory.OpenFromFile(path, ArcMapApplication.hWnd) ;
        }

        private void CreateLandsatScenesTable(IWorkspace workspace, String tableName)
        {
            var scenceAttributes= new List<string> {"Scene_ID", "Defect_FID"};
            GenerateTable(scenceAttributes, workspace, tableName);
        }
        private void CreateSourceTable(IWorkspace workspace, String tableName)
        {
            var sourceAttributes = new List<string> {"Source", "Defect_FID"};
            GenerateTable(sourceAttributes, workspace, tableName);
        }
        private void CreateAssociatedMXDTable(IWorkspace workspace, String tableName)
        {
            var mxdAttributes = new List<string> { "MXD_ID", "Defect_FID" };
            GenerateTable(mxdAttributes, workspace, tableName);
        }
        private void CreateModelTypeTable(IWorkspace workspace, String tableName)
        {
            var modelAttributes = new List<string> { "Model_Type", "Defect_FID" };
            GenerateTable(modelAttributes, workspace, tableName);
        }
        private void CreateMRLCVintageTable(IWorkspace workspace, String tableName)
        {
            var mrlcAttributes = new List<string> { "Product_Year", "Defect_FID" };
            GenerateTable(mrlcAttributes, workspace, tableName);
        }
        private void CreateProductionProcessTable(IWorkspace workspace, String tableName)
        {
            var processAttributes = new List<string> { "Process_Name", "Defect_FID" };
            GenerateTable(processAttributes, workspace, tableName);
        }

        private void GenerateTable(List<string> attributeFields, IWorkspace workspace, String tableName)
        {
            // Cast the workspace to the IFeatureWorkspace interface.
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

            // Create the fields collection.
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

            
            // Add the ObjectID field.
            
            IField oidField = new FieldClass();
            IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "OID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);
            
            
            // Add the text field.
            foreach (string attributeField in attributeFields)
            {
                IField nameField = new FieldClass();
                IFieldEdit nameFieldEdit = (IFieldEdit)nameField;
                nameFieldEdit.Name_2 = attributeField;
                nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                fieldsEdit.AddField(nameField);
            }

            // Use IFieldChecker to create a validated fields collection.
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            //fieldChecker.ValidateWorkspace = workspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

            // The enumFieldError enumerator can be inspected at this point to determine 
            // which fields were modified during validation.

            // Create a UID for the CLSID parameter of CreateTable. For a regular object class,
            // you should use esriGeodatabase.Object.
            UIDClass instanceUID = new UIDClass();
            instanceUID.Value = "esriGeodatabase.Object";

            // Create create the table
            IWorkspace2 ws2 = workspace as IWorkspace2;
            if(!ws2.get_NameExists(esriDatasetType.esriDTTable,tableName))
            {
                ITable table = featureWorkspace.CreateTable(tableName, validatedFields, instanceUID, null, "");
            }
           
        }

    }
}
