using System;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ArcMapClassLibrary2
{
   public class CreateNewFeatureClass
   {
       public void CreateNewFeature(IWorkspace workspace,IApplication mApplication ,string featureClsName,string featureDatasetName)
       {
           try
           {
               IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;

               if (featureWorkspace != null && featureDatasetName!=null)
               {

                   IFeatureDataset fds = featureWorkspace.OpenFeatureDataset(featureDatasetName);

                   //Create featureClass
                   IFieldsEdit fields = new FieldsClass();
                   
                   //Count of generic fields
                   int countDefectAttributes = Enum.GetNames(typeof(DefectAttributes)).Length;
                   
                   //Total fields
                   fields.FieldCount_2 = 2+countDefectAttributes;

                   //Create OID
                   IFieldEdit field = new FieldClass();
                   field.Name_2 = "ObjectID";
                   field.Type_2 = esriFieldType.esriFieldTypeOID;

                   fields.Field_2[0] = field;
                   
                   //Create Geometery
                   ISpatialReference spatialReference = new GetSpatialReference(mApplication).GetSpatialRef();

                   IGeometryDefEdit geometryDef = new GeometryDefClass();
                   geometryDef.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                   geometryDef.SpatialReference_2 = spatialReference;

                   field = new FieldClass();
                   field.Name_2 = "Shape";
                   field.Type_2 = esriFieldType.esriFieldTypeGeometry;
                   field.GeometryDef_2 = geometryDef;

                   fields.Field_2[1] = field;

                   //Generic fields
                   // Create other fields using Defect atrribute enum..
                   var enumValues = Enum.GetValues(typeof(DefectAttributes)).Cast<DefectAttributes>();

                   for (int i = 0; i < countDefectAttributes; i++)
                   {
                       field = new FieldClass();
                       field.Name_2 = enumValues.ElementAt(i).ToString();
                       field.Type_2 = esriFieldType.esriFieldTypeString;

                       fields.Field_2[2 + i] = field;
                   }


                   fds.CreateFeatureClass(featureClsName, fields, null, null, esriFeatureType.esriFTSimple, "shape", null);
                   
               }
           }
           
           catch( Exception e)
           {
               MessageBox.Show("Can not create feature "+e.ToString());
           }


        }



   }
}
