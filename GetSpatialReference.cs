using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;

namespace ArcMapClassLibrary2
{
    public class GetSpatialReference
    {
        private IMxDocument _mxDocument;
        private IApplication _mapApplication;

        public GetSpatialReference(IApplication mApplication)
        {
            _mapApplication = mApplication;
        }


        public ISpatialReference GetSpatialRef()
        {
            _mxDocument = _mapApplication.Document as IMxDocument;
            ISpatialReference spatialReference = _mxDocument.FocusMap.SpatialReference;
            
            if(spatialReference ==null)
            {
                spatialReference = GetDefaultSpatialRef();
            }

            return spatialReference;

        }

        private ISpatialReference GetDefaultSpatialRef()
        {
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            int coordinateSystemID = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
            ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem(coordinateSystemID);

            return spatialReference;

        }

    }
}
