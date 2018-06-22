using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using Android.Runtime;
using System.Net;
using System.IO;
using System.Text;

namespace GeoManagerField
{
    [Activity(Label = "GeoManager Field V4.3 - Powered by Geomaps.", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        private int contentHeight;
        private MapView androidMap;
        private ImageButton editButton;
        private ImageButton searchButton;
        private ImageButton gpsButton;
        private Button addButton;
        private ImageButton lineButton;
        private ImageButton pointButton;
        private ImageButton clearButton;
        private ImageButton cursorButton;
        private ImageButton circleButton;
        private ImageButton polygonButton;
        private ImageButton ellipseButton;
        private ImageButton rectangleButton;
        private ImageButton drawButton;
        private LinearLayout trackLinearLayout;
        private Vertex endVertex;
        string clickcount = "0";

        //location variables   
        Android.Locations.Location _currentLocation;
        Android.Locations.LocationManager _locationManager;
        String _locationProvider;

        bool locationInitialised = false;
        LayerOverlay layerOverlay = new LayerOverlay();
        InMemoryFeatureLayer pointLayer = new InMemoryFeatureLayer();
        ManagedProj4Projection proj4 = new ManagedProj4Projection();


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            // WorldMapKitOverlay worldOverlay = new WorldMapKitOverlay();
            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.TrackOverlay.VertexAdded += (sender, e) => { endVertex = e.AddedVertex; };
            androidMap.MapUnit = GeographyUnit.Meter;

            ShapeFileFeatureLayer shapeFileFeatureLayer = new ShapeFileFeatureLayer(System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), "cadastral.shp"));
            shapeFileFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(
           GeoColor.FromArgb(100, GeoColor.StandardColors.White), GeoColor.StandardColors.Green);
            shapeFileFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("Plotno_1", new GeoFont("Arail", 9, DrawingFontStyles.Bold), new GeoSolidBrush(GeoColor.SimpleColors.Black));
            shapeFileFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer buildingLayer = new ShapeFileFeatureLayer(System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), "building.shp"));
            buildingLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(
            GeoColor.FromArgb(100, GeoColor.StandardColors.Gray), GeoColor.StandardColors.Blue);
            buildingLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("BLD_NO", new GeoFont("Arail", 9, DrawingFontStyles.Bold), new GeoSolidBrush(GeoColor.SimpleColors.Black));
            buildingLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer hLayer = new ShapeFileFeatureLayer(System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), "BLOCKs.shp"), ShapeFileReadWriteMode.ReadWrite);
            hLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("BLOCKSch", new GeoFont("Arail", 9, DrawingFontStyles.Bold), new GeoSolidBrush(GeoColor.SimpleColors.Black));
            hLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.StandardColors.Blue);
            hLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Blue;
            hLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.OuterPen = new GeoPen(GeoColor.FromArgb(200, GeoColor.StandardColors.Red), 5);
            hLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.SymbolPen = new GeoPen(GeoColor.FromArgb(255, GeoColor.StandardColors.Green), 8);
            hLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            pointLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.CreateSimpleCircleStyle(GeoColor.StandardColors.Red, 12, GeoColor.StandardColors.Black);
            pointLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            //  Feature gmp = new Feature(new PointShape(256721.2508062, 9855780.04717357));         
            //  pointLayer.InternalFeatures.Add(gmp);
            LayerOverlay pointOverlay = new LayerOverlay();
            pointOverlay.Layers.Add("PointLayer", pointLayer);

            //  LayerOverlay layerOverlay = new LayerOverlay();  moved to public 
            layerOverlay.Layers.Add("Cadastral", shapeFileFeatureLayer);

            LayerOverlay hOverlay = new LayerOverlay();
            hOverlay.Layers.Add("Blocks", hLayer);

            LayerOverlay bOverlay = new LayerOverlay();
            bOverlay.Layers.Add("Building", buildingLayer);

            androidMap.Overlays.Add("building", bOverlay);
            androidMap.Overlays.Add("cadastral", layerOverlay);
            androidMap.Overlays.Add("blocks", hOverlay);
            androidMap.Overlays.Add("gmpoint", pointOverlay);

            //  var v = Android.OS.Environment.GetExternalStoragePublicDirectory ();
            //Internal projection string from the PRJ file. Note that the false easting value (x_0) has to be expressed in meter for proj4 string.
            string internalProjectionString = "+proj=utm +zone=37 +south +ellps=clrk80 +units=m +no_defs";
            proj4.InternalProjectionParametersString = internalProjectionString;
            //External projection string as Geodetic (21037).
            proj4.ExternalProjectionParametersString = ManagedProj4Projection.GetEpsgParametersString(21037);
            proj4.Open();

            shapeFileFeatureLayer.FeatureSource.Projection = proj4;
            buildingLayer.FeatureSource.Projection = proj4;
            pointLayer.FeatureSource.Projection = proj4;

            hLayer.RequireIndex = false;
            buildingLayer.RequireIndex = false;
            shapeFileFeatureLayer.RequireIndex = false;
            shapeFileFeatureLayer.Open();
            androidMap.MapDoubleTap += AndroidMap_MapDoubleTap;

            androidMap.CurrentExtent = shapeFileFeatureLayer.GetBoundingBox();
            shapeFileFeatureLayer.Close();
            proj4.Close();
            androidMap.Refresh();

            gpsButton = GetButton(Resource.Drawable.Gps, TrackButtonClick);
            addButton = GetLayers(LayerOn);
            searchButton = GetButton(Resource.Drawable.Search, TrackButtonClick);
            cursorButton = GetButton(Resource.Drawable.Cursor, TrackButtonClick);
            drawButton = GetButton(Resource.Drawable.Draw, TrackButtonClick);
            pointButton = GetButton(Resource.Drawable.Point, TrackButtonClick);
            lineButton = GetButton(Resource.Drawable.Line, TrackButtonClick);
            rectangleButton = GetButton(Resource.Drawable.Rectangle, TrackButtonClick);
            circleButton = GetButton(Resource.Drawable.Circle, TrackButtonClick);
            polygonButton = GetButton(Resource.Drawable.Polygon, TrackButtonClick);
            ellipseButton = GetButton(Resource.Drawable.Ellipse, TrackButtonClick);
            editButton = GetButton(Resource.Drawable.Edit, TrackButtonClick);
            clearButton = GetButton(Resource.Drawable.Clear, TrackButtonClick);

            trackLinearLayout = new LinearLayout(this);
            trackLinearLayout.Orientation = Orientation.Horizontal;
            trackLinearLayout.Visibility = ViewStates.Gone;
            trackLinearLayout.AddView(pointButton);
            trackLinearLayout.AddView(lineButton);
            trackLinearLayout.AddView(rectangleButton);
            trackLinearLayout.AddView(polygonButton);
            trackLinearLayout.AddView(ellipseButton);

            LinearLayout toolsLinearLayout = new LinearLayout(this);
            toolsLinearLayout.AddView(searchButton);
            toolsLinearLayout.AddView(addButton);
            toolsLinearLayout.AddView(gpsButton);
            toolsLinearLayout.AddView(cursorButton);
            toolsLinearLayout.AddView(drawButton);
            toolsLinearLayout.AddView(trackLinearLayout);
            toolsLinearLayout.AddView(editButton);
            toolsLinearLayout.AddView(clearButton);
            InitializeInstruction(toolsLinearLayout);
        }

        private ImageButton GetButton(int imageResId, EventHandler handler)
        {
            ImageButton button = new ImageButton(this);
            button.Id = imageResId;
            button.SetImageResource(imageResId);
            button.Click += handler;
            button.SetBackgroundResource(Resource.Drawable.buttonbackground);
            return button;
        }

        private Button GetLayers(EventHandler handler)
        {
            Button button = new Button(this);
            //button.Id = imageResId;
            //button.SetImageResource(imageResId);
            button.Click += handler;
            button.SetBackgroundResource(Resource.Drawable.buttonbackground);
            return button;
        }

        private IEnumerable<ImageButton> GetButtons()
        {
            yield return editButton;
            yield return lineButton;
            yield return pointButton;
            yield return clearButton;
            yield return cursorButton;
            yield return circleButton;
            yield return polygonButton;
            yield return ellipseButton;
            yield return rectangleButton;
            yield return drawButton;
            yield return searchButton;
            //yield return addButton;
            yield return gpsButton;
        }
        private IEnumerable<Button> GetButtonss()
        {
            yield return addButton;  
        }
        private void TrackButtonClick(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            foreach (ImageButton tempButton in GetButtons())
            {
                tempButton.SetBackgroundResource(Resource.Drawable.buttonbackground);
            }
            button.SetBackgroundResource(Resource.Drawable.buttonselectedbackground);

            androidMap.TrackOverlay.LongPress(new InteractionArguments() { WorldX = endVertex.X, WorldY = endVertex.Y });

            switch (button.Id)
            {
                case Resource.Drawable.Cursor:
                    foreach (var item in androidMap.EditOverlay.EditShapesLayer.InternalFeatures)
                    {
                        if (!androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Contains(item))
                        {
                            androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Add(item);

                        }

                    }

                    androidMap.TrackOverlay.TrackMode = TrackMode.None;
                    androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Clear();
                    androidMap.EditOverlay.ClearAllControlPoints();
                    trackLinearLayout.Visibility = ViewStates.Gone;
                    drawButton.Visibility = ViewStates.Visible;
                    editButton.Visibility = ViewStates.Visible;
                    clearButton.Visibility = ViewStates.Visible;
                    androidMap.Refresh();
                    break;

                case Resource.Drawable.Clear:
                    androidMap.EditOverlay.ClearAllControlPoints();
                    androidMap.EditOverlay.EditShapesLayer.Open();
                    androidMap.EditOverlay.EditShapesLayer.Clear();
                    androidMap.TrackOverlay.TrackShapeLayer.Open();

                    androidMap.TrackOverlay.TrackShapeLayer.Clear();
                    androidMap.Refresh();
                    break;

                case Resource.Drawable.Point: //save changes 
                    // androidMap.TrackOverlay.TrackMode = TrackMode.Point;
                    foreach (Feature feature in androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures)
                    {
                        androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Add(feature);


                    }
                    androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();
                    androidMap.EditOverlay.CalculateAllControlPoints();
                    androidMap.Refresh();
                    break;
                case Resource.Drawable.Search:
                    // GPS OFF

                    Toast.MakeText(this, " GeoManager GPS Status : OFF  ", ToastLength.Short).Show();
                    PlotSearch();
                    break;
                case Resource.Drawable.Add:
                    //AddLayers();
                    break;
                case Resource.Drawable.Gps:
                    if (locationInitialised)
                    {
                        // GPS OFF
                        locationInitialised = false;
                        Toast.MakeText(this, " GeoManager GPS Status : OFF  ", ToastLength.Short).Show();

                    } else
                    {  // GPS ON
                        locationInitialised = true;
                        InitializeLocationManager();

                        Toast.MakeText(this, " GeoManager GPS Status : ON  ", ToastLength.Short).Show();

                    }
                    break;
                case Resource.Drawable.Line:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Line;
                    break;

                case Resource.Drawable.Rectangle:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Rectangle;
                    break;

                case Resource.Drawable.Polygon:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Polygon;
                    break;

                case Resource.Drawable.Circle:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Circle;
                    break;

                case Resource.Drawable.Ellipse:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Ellipse;
                    break;

                case Resource.Drawable.Edit:
                    GetGVBFromUser();
                    androidMap.TrackOverlay.TrackMode = TrackMode.None;
                    foreach (Feature feature in androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures)
                    {
                        androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Add(feature);


                    }
                    androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();
                    androidMap.EditOverlay.CalculateAllControlPoints();
                    androidMap.Refresh();
                    break;

                case Resource.Drawable.Draw:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Polygon;
                    trackLinearLayout.Visibility = ViewStates.Visible;
                    drawButton.Visibility = ViewStates.Gone;
                    editButton.Visibility = ViewStates.Gone;
                    clearButton.Visibility = ViewStates.Gone;
                    pointButton.SetBackgroundResource(Resource.Drawable.buttonselectedbackground);

                    androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();// necessary to reset the search selection 
                    androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Clear();// necessary to reset the search selection 
                    androidMap.Refresh();
                    foreach (Feature feature in androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures)
                    {
                        androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Add(feature);


                    }

                    break;

                default:
                    androidMap.TrackOverlay.TrackMode = TrackMode.Polygon;
                    break;


            }
        }
        private void LayerOn(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(clickcount);
            //Convert.ToInt32(clickcount)++;
            value++;
            //clickcount++;
            Button button = (Button)sender;
            foreach (Button tempButton in GetButtonss())
            {
                tempButton.SetBackgroundResource(Resource.Drawable.buttonbackground);
            }
            button.SetBackgroundResource(Resource.Drawable.buttonselectedbackground);
           
            button.Click += delegate {
                button.Text = "Times clicked: " + value;
                
            };
           
            androidMap.TrackOverlay.LongPress(new InteractionArguments() { WorldX = endVertex.X, WorldY = endVertex.Y });
            AddLayers(value.ToString());
        }

        void postLayerChange(string GVB)
        {
            //iterate edit and new layer features
            foreach (var item in androidMap.EditOverlay.EditShapesLayer.InternalFeatures)
            {
                if (!androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Contains(item))
                {
                    androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Add(item);
                    editsave(item, GVB);
                }

            }

        }
        void editsave(Feature feature, string GVB)
        {
            LayerOverlay layerOverlay = (LayerOverlay)androidMap.Overlays["blocks"];
            ShapeFileFeatureLayer blkLayer = (ShapeFileFeatureLayer)layerOverlay.Layers["Blocks"];

            //Edit the geometry of a street feature.
            blkLayer.Open();
            blkLayer.FeatureSource.BeginTransaction();

            feature.ColumnValues.Add("BLOCKS", GVB);
            blkLayer.FeatureSource.AddFeature(feature);

            blkLayer.FeatureSource.CommitTransaction();
            blkLayer.Close();

            androidMap.Refresh();


        }
        private void AndroidMap_MapDoubleTap(object sender, Android.Views.MotionEvent e)
        {
            androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();// necessary to reset the search selection 
            androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Clear();// necessary to reset the search selection 
            PointF location = new PointF(e.GetX(), e.GetY());
            PointShape position = ExtentHelper.ToWorldCoordinate(androidMap.CurrentExtent, location.X,
            location.Y, androidMap.Width, androidMap.Height);
            var inputDialog = new AlertDialog.Builder(this);
            inputDialog.SetTitle("Choose Action");
            inputDialog.SetPositiveButton(
                "Get LR No.",
                (see, ess) =>
                {
                    try
                    {
                        LayerOverlay highlightOverlay = (LayerOverlay)androidMap.Overlays["cadastral"];
                        FeatureLayer highlightLayer = (FeatureLayer)highlightOverlay.Layers["Cadastral"];

                        highlightLayer.Open();
                        Collection<Feature> selectedFeatures = highlightLayer.QueryTools.GetFeaturesContaining(position, new string[1] { "Plotno_1" });
                        highlightLayer.Close();

                        if (selectedFeatures.Count > 0)
                        {
                            string p = null;
                            foreach (var v in selectedFeatures)
                            {
                                p = v.ColumnValues["Plotno_1"].ToString();
                            }
                            var uri = Android.Net.Uri.Parse("http://40.68.99.44/GeoManagerField/pages/valuationform.aspx?lrno=" + p);
                            var intent = new Intent(Intent.ActionView, uri);
                            StartActivity(intent);
                        }
                    }
                    catch (Exception ex)
                    {
                        // do nothing 
                    }
                });
            inputDialog.SetNegativeButton("Get Building No.", (afk, kfa) => {
                try
                {
                    LayerOverlay highlightOverlay = (LayerOverlay)androidMap.Overlays["building"];
                    FeatureLayer highlightLayer = (FeatureLayer)highlightOverlay.Layers["Building"];
                    highlightLayer.Open();
                    Collection<Feature> selectedFeatures = highlightLayer.QueryTools.GetFeaturesContaining(position, new string[1] { "BLD_NO" });
                    highlightLayer.Close();
                    if (selectedFeatures.Count > 0)
                    {
                        string p = null;
                        foreach (var v in selectedFeatures)
                        {
                            p = v.ColumnValues["BLD_NO"].ToString();
                        }
                        var uri = Android.Net.Uri.Parse("http://40.68.99.44/GeoManagerField/pages/buildingform.aspx?bldno=" + p);
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                    }
                }
                catch (Exception ex)
                {
                    //do nothing 
                }
            });
            inputDialog.Show();
        }
        void PlotSearch()
        {
            if (locationInitialised)
            {

                //use our web converter /pages/utmconverter.aspx?lat=-1.303&lon=36.90909
                string[] realpoint = null;
                var request = WebRequest.Create("http://40.68.99.44/GeoManagerField/pages/utmconverter.aspx?lat=" + _currentLocation.Latitude + "&lon=" + _currentLocation.Longitude) as HttpWebRequest;
                // request.Method = "GET";
                request.Method = "POST";
                request.ContentLength = 0;
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response;
                string fullresponse = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    fullresponse = readStream.ReadToEnd();
                    if (!fullresponse.StartsWith("0")) // getting 0 from response
                    {
                        string points = fullresponse.Substring(0, fullresponse.IndexOf("<")).Trim();
                        realpoint = points.Split(',');

                        double lat = 0;
                        double lon = 0;
                        if (double.TryParse(realpoint[0].ToString(), out lat) && double.TryParse(realpoint[1].ToString(), out lon))
                        {
                            pointLayer.InternalFeatures.Clear();
                            Feature gmp = new Feature(new PointShape( lat -94.280  ,  lon  + 278.182));
                            pointLayer.InternalFeatures.Add(gmp);
                            androidMap.Refresh();

                            //  PointShape position = new PointShape(lat, lon);
                            //  LayerOverlay highlightOverlay = (LayerOverlay)androidMap.Overlays["cadastral"];
                            //  FeatureLayer highlightLayer = (FeatureLayer)highlightOverlay.Layers["Cadastral"];

                            //  highlightLayer.Open();
                            //  Collection<Feature> selectedFeatures = highlightLayer.QueryTools.GetFeaturesContaining(position, new string[1] { "Plotno_1" });
                            // highlightLayer.Close();

                            // if (selectedFeatures.Count > 0)
                            // {
                            //     androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();
                            //     RectangleShape featureextent = selectedFeatures[0].GetShape().GetBoundingBox();
                            //     androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Add(selectedFeatures[0].Id, selectedFeatures[0]);
                            //    androidMap.CurrentExtent = featureextent;
                            //    androidMap.Refresh();
                            //turn on gps after 
                            //   locationInitialised = true;
                            //  }
                            // else
                            // {
                            //     var msgDialog = new AlertDialog.Builder(this);
                            //     msgDialog.SetTitle("GeoManager Alert");
                            //    msgDialog.SetMessage("GPS Position: " + lat + "," + lon + " not found in map sheet or position not in a plot . . . please verify.");
                            //     msgDialog.SetPositiveButton("Ok", (s, ev) =>
                            //    {

                            //   });
                            //    msgDialog.Show();

                            // }
                        }
                        else
                        {
                            Toast.MakeText(this, "Incorrect GPS data conversion - string passed as decimal.", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Incorrect GPS data conversion - response 0", ToastLength.Short).Show();
                    }

                }
                catch (Exception)
                {
                    Toast.MakeText(this, "No internet connectivity for GPS Data Conversion . . verify.", ToastLength.Short).Show();

                }


            }
            else
            {

                var inputDialog = new AlertDialog.Builder(this);
                EditText userInput = new EditText(this);

                string selectedInput = string.Empty;

                userInput.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationNormal;
                inputDialog.SetTitle("Input LR No?");
                inputDialog.SetView(userInput);
                inputDialog.SetPositiveButton(
                    "Spatial Search",
                    (see, ess) =>
                    {
                        if (userInput.Text != string.Empty && userInput.Text != "")
                        {
                            LayerOverlay highlightOverlay = (LayerOverlay)androidMap.Overlays["cadastral"];
                            FeatureLayer highlightLayer = (FeatureLayer)highlightOverlay.Layers["Cadastral"];

                            highlightLayer.Open();
                            Collection<Feature> selectedFeatures = highlightLayer.QueryTools.GetFeaturesByColumnValue("Plotno_1", userInput.Text.Trim());
                            highlightLayer.Close();

                            if (selectedFeatures.Count > 0)
                            {
                                androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Clear();
                                RectangleShape featureextent = selectedFeatures[0].GetShape().GetBoundingBox();
                                androidMap.TrackOverlay.TrackShapeLayer.InternalFeatures.Add(selectedFeatures[0].Id, selectedFeatures[0]);
                                androidMap.CurrentExtent = featureextent;
                                androidMap.Refresh();
                            }
                            else
                            {
                                var msgDialog = new AlertDialog.Builder(this);
                                msgDialog.SetTitle("GeoManager Alert");
                                msgDialog.SetMessage("LR No was not found in this map sheet . . . verify LR No");
                                msgDialog.SetPositiveButton("Ok", (s, ev) =>
                                {

                                });
                                msgDialog.Show();

                            }
                        }
                        HideKeyboard(userInput);
                    });
                inputDialog.SetNegativeButton("CLOSE", (afk, kfa) => { HideKeyboard(userInput); });
                inputDialog.Show();
                ShowKeyboard(userInput);
            }
        }
        string[] GetShapefiles() {
            string []  filelist = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).List();
            string[] filenames =null ;
            int shpcount  = 0;
            foreach (var item in filelist) //loop to first get array size for array initialization below 
            {
                if (item.ToString().Contains(".shp"))
                {                     
                    shpcount ++;
                }

            }
            filenames = new string[shpcount ];
            int i = 0;
            foreach ( string  item in filelist)//loop to set only shp file names 
            { 
                if (item.Contains(".shp"))
                {
                    filenames[i] = item;
                    i++;
                }

            }
  
            return filenames;           
        }
        string[] GetShapefiles2(string value)
        {
            string[] filelist = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).List();
            string[] filenames = null;
            int shpcount = 0;
            foreach (var item in filelist) //loop to first get array size for array initialization below 
            {
                if (item.ToString().Contains(".shp"))
                {
                    shpcount++;
                }

            }
            filenames = new string[shpcount];
            int i = 0;
            foreach (string item in filelist)//loop to set only shp file names 
            {
                if (item.Contains(".shp"))
                {
                    filenames[i] = item;
                    i++;
                }

            }
            Array.Resize(ref filenames, filenames.Length + 1);
            filenames[filenames.Length - 1] = value;
            return filenames;
        }
        void AddLayers(string value)
        {
            string[] fnames = GetShapefiles2(value);
            
            AlertDialog.Builder builder = new AlertDialog.Builder(this );
            builder.SetTitle("Select Shape File?");
            builder.SetItems( fnames , (sender, args) =>
            {

                if (fnames[args.Which] != null)
                {
                    //  Toast.MakeText( this , "Choose existing Service picked" +  fnames [args.Which], ToastLength.Short).Show();
                    string layername = fnames[args.Which];
                    ShapeFileFeatureLayer nLayer = new ShapeFileFeatureLayer(System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString(), layername ), ShapeFileReadWriteMode.ReadOnly);                   
                    nLayer.RequireIndex = false;
                    nLayer.Open();
                   
                    if (nLayer.GetShapeFileType() == ShapeFileType.Point)
                    {
                        nLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.CreateSimplePointStyle(PointSymbolType.Circle, GeoColor.StandardColors.Red, GeoColor.StandardColors.Orange, 2);               
                        nLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
                    }
                    else if (nLayer.GetShapeFileType() == ShapeFileType.Polyline)
                    {
                        nLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.StandardColors.LightPink,2, GeoColor.StandardColors.Black,0, false );
                        nLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
                    }
                    else if (nLayer.GetShapeFileType() == ShapeFileType.Polygon)
                    
                    {
                        nLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(
                         GeoColor.FromArgb(100, GeoColor.StandardColors.LightYellow), GeoColor.StandardColors.Blue);
                    }
                    var  v   = nLayer.QueryTools.GetColumns() ;
                    string[] fields = new string[ v.Count ];
                    //for (int i = 0; i<v.Count ; ++i)
                    int i = 0;
                    foreach (FeatureSourceColumn c in v)
                    {                                               
                        fields[i] = c.ColumnName;
                        i++;
                    } 
                    AlertDialog.Builder bld = new AlertDialog.Builder(this);
                    bld .SetTitle("Select labelling field?");
                    bld .SetItems(  fields  , (senders, argss) =>
                    {
                        if (fields[argss.Which] != null)
                        {
                            string d = fields[argss.Which].ToString();
                            if (nLayer.GetShapeFileType() == ShapeFileType.Polyline)
                            {
                                nLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.LocalRoad1(fields[argss.Which]);
                            } else
                            {                            
                            nLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle(fields[argss.Which], new GeoFont("Arail", 9, DrawingFontStyles.Bold), new GeoSolidBrush(GeoColor.SimpleColors.Black));                    
                            }
                            nLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
                            nLayer.Close();
                            nLayer.FeatureSource.Projection = proj4;
                            LayerOverlay lOverlay = new LayerOverlay();
                            lOverlay.Layers.Add(layername, nLayer);
                            androidMap.Overlays.Add(layername, lOverlay);
                            androidMap.Refresh();                              
                        }
                    });
                    bld.Create().Show();
                }
                else if (fnames[args.Which] ==null )
                {
                    Toast.MakeText( this , "Cancel picked", ToastLength.Short).Show();
                    //How to close dialog Here 
                }
            });
            builder.Create() .Show();
        }
        public static T[] ToGenericArray<T>(Collection<T> collection)
        {
            if (collection == null)
            {
                return new T[] { };
            }
            return new List<T>(collection).ToArray();
        }
        void GetGVBFromUser ()
        {
            var inputDialog = new AlertDialog.Builder(this );
            EditText userInput = new EditText(this );

            string selectedInput = string.Empty;
            
            userInput.InputType = Android.Text.InputTypes.ClassText  | Android.Text.InputTypes.TextVariationNormal ;
            inputDialog.SetTitle("What is the GVB?");
            inputDialog.SetView(userInput);
            inputDialog.SetPositiveButton(
                "SAVE BOTH",
                (see, ess) =>
                {
                    if (userInput.Text != string.Empty && userInput.Text != "")
                    {
                        postLayerChange(userInput .Text );

                    }
                    else
                    {
                         //empty
                    }
                    HideKeyboard(userInput);
                });
            inputDialog.SetNegativeButton("CLOSE", (afk, kfa) => { HideKeyboard(userInput); });
            inputDialog.Show();
            ShowKeyboard(userInput);
        }
        private void ShowKeyboard(EditText userInput)
        {
            userInput.RequestFocus();
            InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            imm.ToggleSoftInput(ShowFlags.Forced, 0);
        }
        private void HideKeyboard(EditText userInput)
                {
                    InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(userInput.WindowToken, 0);
                }
        public void InitializeInstruction(params View[] contentViews)
        {      
            contentHeight = 0;
            ViewGroup containerView = FindViewById<RelativeLayout>(Resource.Id.MainLayout);

            LayoutInflater inflater = LayoutInflater.From(this);
            View instructionLayoutView = inflater.Inflate(Resource.Layout.Instruction, containerView);

            TextView instructionTextView = instructionLayoutView.FindViewById<TextView>(Resource.Id.instructionTextView);
            TextView descriptionTextView = instructionLayoutView.FindViewById<TextView>(Resource.Id.descriptionTextView);
            descriptionTextView.Text = "Draw and Edit Shapes.";

            LinearLayout instructionLayout = instructionLayoutView.FindViewById<LinearLayout>(Resource.Id.instructionLinearLayout);
            LinearLayout contentLayout = instructionLayoutView.FindViewById<LinearLayout>(Resource.Id.contentLinearLayout);

            RelativeLayout headerRelativeLayout = instructionLayoutView.FindViewById<RelativeLayout>(Resource.Id.headerRelativeLayout);

            if (contentViews != null)
            {
                foreach (View view in contentViews)
                {
                    contentLayout.AddView(view);
                }
            }

            headerRelativeLayout.Click += (sender, e) =>
            {
                contentHeight = contentHeight == 0 ? instructionLayout.Height - instructionTextView.Height : -contentHeight;
                instructionLayout.Layout(instructionLayout.Left, instructionLayout.Top + contentHeight, instructionLayout.Right, instructionLayout.Bottom);
            };
        }
        public void OnLocationChanged(Location location)
        {
            if (locationInitialised)
            {
                _currentLocation = location;
                if (_currentLocation == null)
                {
                    //_locationText.Text = "Unable to determine your location.";
                    Toast.MakeText(this, "Unable to determine your location.", ToastLength.Short).Show();
                }
                else
                {       //turn off gps for this process to complete **process moved to search
                   
                    Toast.MakeText(this, "GeoManager GPS Position: " + _currentLocation .Latitude +" , " +_currentLocation.Longitude , ToastLength.Long ).Show();

                    ////               PointShape position = ExtentHelper.ToWorldCoordinate(androidMap.CurrentExtent, location.X,
                    ////location.Y, androidMap.Width, androidMap.Height);


                    //         ManagedProj4Projection gpsproj4 = new  ManagedProj4Projection();
                    //               string z = location.Altitude.ToString ();
                    //               gpsproj4.InternalProjectionParametersString = @"+proj=longlat +a=6378137.0000 +rf=298.2572221010000 +units=m +nadgrids=@null +no_defs";
                    //               gpsproj4.ExternalProjectionParametersString = @"+proj=utm +zone=37 +south +ellps=clrk80 +towgs84=-160,-6,-302,0,0,0,0 +units=m +no_defs";//ManagedProj4Projection.GetEpsgParametersString(21037);
                    //               gpsproj4.Open();
                    //               Vertex gpspoint  = gpsproj4 .ConvertToExternalProjection   (location .Latitude , location .Longitude );
                    //               gpsproj4.Close();
                }
            }
         
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (locationInitialised )
            {
                 _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            }
        }
        protected override void OnPause()
        {
            base.OnPause();

            if (locationInitialised )
            {
                 _locationManager.RemoveUpdates(this);
            }  
        }
        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }
        public void OnProviderEnabled(string provider)
        {
          throw new NotImplementedException();
        }
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Count >0)
            {
                _locationProvider = acceptableLocationProviders[0];               
              _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
                // Toast.MakeText(this, " Location provider  " + acceptableLocationProviders[0], ToastLength.Short).Show();
            }
            else
            {
                _locationProvider = String.Empty;
            }
        }
    }

}

