using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Devices.Properties;

namespace Devices
{
    //..............................................................................................

    public class CCDMultiImageAnalysis
    {
        public double standardDevX = 0;       //Contains the standard deviation of the x movements of the spot 
        public double standardDevY = 0;       //Contains the standard deviation of the y movements of the spot
        public bool isMoving = false;         //Contains the boolean value determining whether the spot is moving.

        CCDImageAnalyzer[] ccdAnalysers;

        /// <summary>
        /// calculatedSpotMovement: Caclulates if a series of spots are moving, and return a "True" for movement, "False" for non movement
        /// </summary>
        /// <param name="imageAnalysers"></param>
        /// <returns>Returns a Bool Value (false) no significant spot movement; (true) significant spot movement</returns>
        public bool calculateSpotMovement(CCDImageAnalyzer[] imageAnalysers)
        {
            ccdAnalysers = imageAnalysers;
            getSpotMovementX();
            getSpotMovementY();
            if (standardDevX > 150 || standardDevY > 150 || standardDevX + standardDevY > 200)
            {
                isMoving = true;
                return true;
            }
            else
            {
                isMoving = false;
                return false;
            }
        }
        private double getSpotMovementX()
        {
            double stdev = 0;
            double sum = 0;
            double sumSquare = 0;
            long count = 0;
            long index = 0;

            for (index = 0; index < ccdAnalysers.Count(); index++)
            {
                double valueX = ccdAnalysers[index].spotCenterX;

                if (true)
                {
                    count++;
                    sum += valueX;
                    sumSquare += (valueX * valueX);
                }

            }

            try
            {
                if (count > 1)
                    stdev = Math.Sqrt((sumSquare - (sum * sum / count)) / (count - 1));
                else
                    stdev = 0;
            }
            catch
            {
                stdev = 0;
            }

            standardDevX = stdev;
            return stdev;

        } //Calcualtes the standard deviation for the X movements of the spot
        private double getSpotMovementY()
        {
            double stdev = 0;
            double sum = 0;
            double sumSquare = 0;
            long count = 0;
            long index = 0;

            for (index = 0; index < ccdAnalysers.Count(); index++)
            {
                double valueX = ccdAnalysers[index].spotCenterY;

                if (true)
                {
                    count++;
                    sum += valueX;
                    sumSquare += (valueX * valueX);
                }

            }

            try
            {
                if (count > 1)
                    stdev = Math.Sqrt((sumSquare - (sum * sum / count)) / (count - 1));
                else
                    stdev = 0;
            }
            catch
            {
                stdev = 0;
            }
            standardDevY = stdev;
            return stdev;
        } //Calculates the standard deviation for the Y movements of the spot  
    }

    //..............................................................................................

    /// <summary>
    /// Displays, Calculates, and Analyzes information about the spot.
    /// This class inherits down from Picturebox, So all referenses to "this." May include all variables and functions from Picturebox as well
    /// </summary>
    public class CCDImageAnalyzer : PictureBox
    {
        //Constant Variables          
        private const int DIAMETER = 2032;          //Contains a valuse used in the conversion process for the pixel-To-Micron Conversion

        //Public Variables 
        public Bitmap thumbNail = null;             //Contains the bitmap representation of the Thumnail portion of the Image Reduced about 30%
        public Bitmap RIOPicture = null;            //Contains a Bitmap respresentaion of the ROI portion of the image   
        public Point spotCenter = Point.Empty;      //Contains the literal point for the spot Ceneter
        public double spotOffSet = 0;               //Contains the offset valuse from the Center of the anode to the spot Center
        public double spotTotalCounts = 0;          //Contains the Total counts of the image as it relates to the spot
        public double anodeTotalCounts = 0;         //Contains the total counts of the image as it relates to the Anode Center.
        public double spotAngle = 0;                //Contains the angle of the of the spot from the anode Center
        public bool isSpotCenterOfROI = false;      //Contains the boolean value of which circle is to be drawn around the spot
        public bool isPass = true;
        public double spotCenterX = 0;              //Contains the XCoord of the xRay Spot;
        public double spotCenterY = 0;              //Contains the YCoord of the XRay Spot;
        public double[,] anodeCounts;
        public double[,] spotCounts;
        public double[,] anodeChartData;
        public double[,] spotChartData;
        public int anode70Flux;
        public int spot70Flux;
        public double StDevX;
        public double StDevY;
        public double pixelToMicron = 0;            //Contains the Conversion factor, that allows a single pixel to be represened as a single micron.
        public double counts1mm = 0;                //Contains the total Counts of the first 1.0 mm radius circle from the
        public double counts1_5mm = 0;              //Contains the Total Counts of the first 1.5 mm from the center of the anode 
        public double minDim = 0;
        public double maxDim = 0;
        public double xRange = 0;
        public double yRange = 0;
        public double fluxPercent = 70;


        //Private Variables
        private static List<Point> circlePoints = new List<Point>();    //Contains the three points required to generate the Circle for the ROI             
        private List<Point> circlePointsSpot = new List<Point>();       //NOT SURE WHAT THIS DOES. We wil see if it comes to use.        
        private bool mouseDown = false;             //Contains a Boolean that indicates whether the mouse click possision is currently down. 
        private int radius = 0;                     //Contains the radius from center point to circlePoints[] (set by generateROI() or Operator); 
        private int spotMaxCounts;                  //Conatins the highest number of xRays Counted in the Xray image. 
        private int imageNum;                       //Contains the Image number for xRayCounts. (passed in)
        private int[,] ROI;                         //Contains the Xray Counts for each pixel in the x,y coordinate for the Region of Interest (passed In)
        private int[, ,] xRayCounts;                 //Contains the Xray Counts for each pixel in the x,y coordinate for the Xray Image (passed In)

        //----------------------------PUBLIC FUNCTIONS----------------------------------

        /// <summary>
        /// Constructor: Sets up the event handellers for the mouse events.
        /// </summary>
        public CCDImageAnalyzer()
        {
            base.MouseDown += CCDImageBox_MouseDown;
            this.MouseMove += CCDImageBox_MouseMove;
            this.MouseUp += CCDImageBox_MouseUp;
            this.Paint += CCDImageBox_Paint;
        }

        /// <summary>
        /// addImage: adds the image to the CCD Anyliser;
        /// </summary>
        /// <param name="image">Requres an image from the CCDImage class</param>
        public void addImage(Image image, int imagNum)
        {
            base.Image = image;             //provides the image and stores the image to be anaylised the Base Class.
            if (imagNum == 4)                //Only Auto generates on Image[4] or the fifth image (all others are to week to survive) 
                generateROI();                  //Auto Generates the Region Of Interest (Work in progress)

        }
    
    public int Spot70Flux {  get { return spot70Flux; } set { spot70Flux = value; } }

    public int Anode70Flux {  get { return anode70Flux; } set { anode70Flux = value; } }

    public double AnodeTotalCounts {  get { return anodeTotalCounts; } set { anodeTotalCounts = value; } }

    public double MinDim {  get { return minDim; }set { anodeTotalCounts = value; } }

    public double MaxDim { get { return maxDim; }set { maxDim = value; } }


        public void clearROIPoints()
        {
            circlePoints.Clear();
        }

        /// <summary>
        /// StartImageAnalysis: Starts the Analysis of the CCD Image: Requires the execution of CCDImage.start() function in CCDImage class;
        /// </summary>
        /// <param name="xRayCounts">Requires the array of 'xRayCounts' from Class CCDImage</param>
        /// <param name="spotMaxCounts">Requires the the int value spotMaxCounts from CCDImage</param>
        /// <param name="imageNum">Requires the int value of the current image being analysed for xRayCounts</param>
        public void startImageAnalysis(int[, ,] xRayCounts, int spotMaxCounts, int imageNum)
        {
            this.imageNum = imageNum;
            this.xRayCounts = xRayCounts;
            this.spotMaxCounts = spotMaxCounts;

            createROIThumnail();                                                     //Creates a thumbnail of the image for printing purposes
            calculateSpotOffset();                                                   //Calculates the spot offset
            calculateSpotCenter();                                                   //Calculates the Center of the Spot
            anodeCounts = calcCountsFromCenter(ROI, "Anode");                        //Cacluates the total counts as it relates to the Anode Center
            spotCounts = calcCountsFromCenter(getSpotCenteredROI(radius), "Spot");   //Cacluates the total counts as it relates the spot location
            anode70Flux = getFluxAndChartData(anodeCounts, ref anodeChartData, anodeTotalCounts);
            spot70Flux = getFluxAndChartData(spotCounts, ref spotChartData, spotTotalCounts);
            SpotSize(ROI);                                                           //Populates the MinDim and MaxDim variables.
            calculateAngleToSpot();
        }


        //----------------------------------PRIVATE FUNCTIONS-----------------------------------

        /// <summary>
        /// generateROI: Generates three points and calls the this.Refresh() command to draw a circle
        /// </summary>
        /// NOTES: This function uses the bitmap to find the location of the halo, It starts at the top, left, and botom centers and moves
        /// towards the center in incraments of ten, checking the average color fluxuations in the image, in order to identify where the edge of the spot is
        private void generateROI()
        {
            ////THIS was and atttempt to auto find the point. You are welcome to try and make it work. I could not.


            //Bitmap b = new Bitmap(this.Image);                          //Convertes the CCD image to a bitmap
            //Graphics g = this.CreateGraphics();                         //Creates a Graphics interface
            //Point pointTop = Point.Empty;                               //Holds the Top Point for the ROI Circle
            //Point pointBottom = Point.Empty;                            //holds the bottom point for the ROI Circle
            //Point pointLeft = Point.Empty;                              //holds the left point for the ROI Circle
            //Color color;

            //double rollingAverge = 0;          //Holds the current average of the the Red Color Value for each itereation            
            //int incramentPixel = 0;

            ////Finds Top of the Halo (not working perfectly): by looking at the red in the image. White being 255,255,255. In order for the blue to show up Red needs to be removed
            ////So this algarithm looks for a sharp decrese in red, which may be the edge of the halo
            //while (pointTop.IsEmpty && incramentPixel < 450)         //450 is an arbitrary value that is used to stop the algarithym from looking when that pixel has reachchd 450
            //{                                                        //because it is impossible for the top of the spot to be at Locacation.Y = 450 (no reason to look farther)
            //    double sum = 0;                                      //Contains the sum of the average

            //    for (int X = 0; X < 10; X++)                         //Graps ten pixels in a Y line and adds up the red color values in order to genereate an average, this reduses mistaken halo edges 
            //    {                                                    //in the noise (works ok, there is probably a better way)
            //        color = b.GetPixel(256, incramentPixel);         //gets the pixel color
            //        sum += color.R;                                  //adds it to the total sum of the desired ten pixels
            //        incramentPixel++;                                //moves to the the next pixel

            //    }
            //    rollingAverge = Math.Ceiling(sum / 10);              //Finds the average red value for pixels X Through (X+10)

            //    if (rollingAverge < 220)                             //checks to see if the average has fallen out of the normal bounderies for the noise on the outer edge of the spot
            //        pointTop = new Point(256, incramentPixel - 10);  //saves the location of the top point IF, it appears to be the edge of the halo
            //}

            //rollingAverge = 0;                                       //resets rolling average back to zero           
            //incramentPixel = 0;                                      //resets the the current pixel to zero

            //g.DrawLine(Pens.Red, pointTop.X - 2, pointTop.Y, pointTop.X + 2, pointTop.Y);   //draws the top point
            //g.DrawLine(Pens.Red, pointTop.X, pointTop.Y - 2, pointTop.X, pointTop.Y + 2);
            //circlePoints.Add(pointTop);                                                     //adds the top point to the to the list of RIO Point indecators. 

            ////Finds the left: Uses same functionallity as the Finding top Algarithm, please refer to it for functionality. No documentation will be mentioned for Left and Bottom.
            //while (pointLeft.IsEmpty && incramentPixel < 450)
            //{
            //    double sum = 0;

            //    for (int X = 0; X < 10; X++)
            //    {
            //        color = b.GetPixel(incramentPixel, 256);
            //        sum += color.R;
            //        incramentPixel++;

            //    }
            //    rollingAverge = Math.Ceiling(sum / 10);
            //    if (rollingAverge < 245)
            //        pointLeft = new Point(incramentPixel - 10, 256);

            //}


            //g.DrawLine(Pens.Red, pointLeft.X - 2, pointLeft.Y, pointLeft.X + 2, pointLeft.Y);
            //g.DrawLine(Pens.Red, pointLeft.X, pointLeft.Y - 2, pointLeft.X, pointLeft.Y + 2);
            //circlePoints.Add(pointLeft);

            //rollingAverge = 0;
            //incramentPixel = 490;

            ////Finds the bottom
            //while (pointBottom.IsEmpty && incramentPixel > 10)
            //{
            //    double sum = 0;

            //    for (int X = 10; X > 0; X--)
            //    {
            //        color = b.GetPixel(256, incramentPixel);
            //        sum += color.R;
            //        incramentPixel--;
            //    }


            //    rollingAverge = Math.Ceiling(sum / 10);
            //    if (rollingAverge < 220)
            //    {
            //        pointBottom = new Point(256, incramentPixel + 10);
            //    }

            //}

            //g.DrawLine(Pens.Red, pointBottom.X - 2, pointBottom.Y, pointBottom.X + 2, pointBottom.Y);
            //g.DrawLine(Pens.Red, pointBottom.X, pointBottom.Y - 2, pointBottom.X, pointBottom.Y + 2);
            //circlePoints.Add(pointBottom);

            //if (circlePoints.Count == 3)
            //    drawROICircle(g);

            //this.Refresh();

        }

        /// <summary> getROI: Finds dimensions (center, radius, diamter) for the RIO and produces a Bitmap image[130px, 130px] for viewing and printing 
        /// </summary>
        /// Function Notes: This section first find the center point of the ROI and uses it to Calculate radius from center to outer Points; It then creates a 
        /// rectagle over the RIO and clones the image into it and stores it as a bitmap called thumbnail. Which is used for printing and viewing
        private void createROIThumnail()
        {
            Bitmap tempImage = new Bitmap(this.Image); //Contains a temperatry copy the CCD Image
            Point center; //Contains the Center Point of the ROI              
            int diameter; //Contains the diameter of the ROI. 

            center = findCenterOfROI();  //Locates the center of the ROI

            radius = Convert.ToInt32(Math.Floor(Math.Sqrt(Math.Pow(center.X - circlePoints[0].X, 2) + Math.Pow(center.Y - circlePoints[0].Y, 2))));  //uses (A^2 + B^2 = C^2) to find the radius of circlePoint[0] A 
            radius += Convert.ToInt32(Math.Floor(Math.Sqrt(Math.Pow(center.X - circlePoints[1].X, 2) + Math.Pow(center.Y - circlePoints[1].Y, 2)))); //uses (A^2 + B^2 = C^2) to find the radius of circlePoint[1]
            radius += Convert.ToInt32(Math.Floor(Math.Sqrt(Math.Pow(center.X - circlePoints[2].X, 2) + Math.Pow(center.Y - circlePoints[2].Y, 2)))); //uses (A^2 + B^2 = C^2) to find the radius of circlePoint[2]
            radius /= 3;             // finds the average radius
            diameter = radius * 2;   //calculates the diamer         

            Rectangle cropArea = new Rectangle(center.X - (radius + 5), center.Y - (radius + 5), radius * 2, radius * 2); //creates a rectangle for cropping all but out the ROI           
            this.RIOPicture = new Bitmap(tempImage.Clone(cropArea, tempImage.PixelFormat)); //Clones the image and stores it in RIOPicture (for printing)
            this.thumbNail = new Bitmap(tempImage.Clone(cropArea, tempImage.PixelFormat), 130, 130);                          //creates the thumbnail images of the cropped area

            ROI = new int[diameter + 10, diameter + 10];

            int x = 0;
            int y = 0;
            for (int X = (center.X - (radius + 5)); X < center.X + (radius + 5); X++)
            {
                y = 0;
                for (int Y = (center.Y - (radius + 5)); Y < center.Y + (radius + 5); Y++)
                {
                    ROI[x, y] = xRayCounts[imageNum, X, Y];
                    y++;
                }
                x++;
            }
            this.Refresh();
        }



        /// <summary>
        /// drawRIOCirlce
        /// </summary>
        /// <param name="g"></param>
        private void drawROICircle(Graphics g)
        {
            try
            {
                Point center = new Point();
                double radius = new double();
                Pen crossPen = new Pen(Color.Red, 1);
                crossPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                center = findCenterOfROI();
                radius = Math.Floor(Math.Sqrt((center.X - circlePoints[0].X) ^ 2 + (center.Y - circlePoints[0].Y) ^ 2));
                radius += Math.Floor(Math.Sqrt((center.X - circlePoints[1].X) ^ 2 + (center.Y - circlePoints[1].Y) ^ 2));
                radius += Math.Floor(Math.Sqrt((center.X - circlePoints[2].X) ^ 2 + (center.Y - circlePoints[2].Y) ^ 2));
                radius = radius / 3;
                g.DrawEllipse(Pens.Red, center.X - (int)radius, center.Y - (int)radius, 2 * (int)radius, 2 * (int)radius);
                g.DrawLine(crossPen, center.X - (int)radius, center.Y, center.X + (int)radius, center.Y);
                g.DrawLine(crossPen, center.X, center.Y - (int)radius, center.X, center.Y + (int)radius);
            }
            catch
            {

            }
            this.Refresh();
        }

        private double calculateSpotOffset()
        {
            double minX = 0;
            double minY = 0;
            double maxX = 0;
            double maxY = 0;
            
            double SSDT = 0.5;
            double pCount = 0;
            double rowSum = 0;
            double colSum = 0;
            double compVal = 0;
            double targetRadius = 0;
            double windowMid = (double)ROI.GetLength(0) / 2;
            targetRadius = windowMid - 1;
            pixelToMicron = DIAMETER / (2 * targetRadius);

            for (int y = 0; y <= ROI.GetLength(1) - 1; y++)
            {
                for (int x = 0; x <= ROI.GetLength(0) - 1; x++)
                {
                    compVal = (double)(ROI[x, y]) / spotMaxCounts;
                    if (compVal > SSDT)
                    {
                        pCount += compVal;
                        rowSum = rowSum + y * compVal;
                        colSum = colSum + x * compVal;
                    }
                }
            }
            if (pCount == 0)
            {
                return 0;
            }

            this.spotCenterX = Math.Round(colSum / pCount - windowMid) * pixelToMicron;
            this.spotCenterY = Math.Round(rowSum / pCount - windowMid) * pixelToMicron;

            if (minX > this.spotCenterX)
                minX = this.spotCenterX;
            if (minY > this.spotCenterY)
                minY = this.spotCenterY;
            if (maxX < this.spotCenterX)
                maxX = this.spotCenterX;
            if (maxY < this.spotCenterY)
                maxY = this.spotCenterY;

            xRange = (float)(maxX - minX);
            yRange = (float)(maxY - minY);

            spotOffSet = Math.Round(Math.Sqrt(Math.Pow(spotCenterX, 2) + Math.Pow(spotCenterY, 2)));

            this.Refresh();

            return (spotOffSet / 1000);   //Posted to the blackboard for later printing.
        }

        private void calculateSpotCenter()
        {
            Point centerXY = findCenterOfROI();
            try
            {
                spotCenter = new Point((int)(double)(centerXY.X + (this.spotCenterX / this.pixelToMicron)), (int)(double)(centerXY.Y + (this.spotCenterY / this.pixelToMicron)));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to calculate spot Center\n" + ex);
            }

            this.Refresh();
        }

        private void calculateAngleToSpot()
        {
            double angle = 0;
            if (spotCenterX == 0)
            {
                if (spotCenterY > 0)
                    angle = 270;
                else
                    angle = 90;
            }
            else
            {
                angle = Math.Atan(-spotCenterY / spotCenterX) * (180 / Math.PI);
                if (spotCenterY > 0)
                {
                    if (spotCenterX > 0)
                        angle += 360;
                    else
                        angle += 180;
                }
                else if (spotCenterY == 0)
                {
                    if (spotCenterX > 0)
                        angle = 0;
                    else
                        angle = 180;
                }
                else
                {
                    if (spotCenterX < 0)
                        angle += 180;
                }
            }

            this.spotAngle = angle;
        }


        private int[,] getSpotCenteredROI(int ROIRadius)
        {
            int RadiusX = 0;
            int RadiusY = 0;

            int[,] vROI = new int[2 * ROIRadius, 2 * ROIRadius];
            for (int X = Math.Max(spotCenter.X - ROIRadius, 0); X < Math.Min(spotCenter.X + ROIRadius, 511); X++)
            {
                for (int Y = Math.Max(spotCenter.Y - ROIRadius, 0); Y < Math.Min(spotCenter.Y + ROIRadius, 511); Y++)
                {
                    if (xRayCounts[imageNum, X, Y] < 0)
                        vROI[RadiusX, RadiusY] = 0;
                    else
                    {
                        int o = xRayCounts[imageNum, X, Y];
                        vROI[RadiusX, RadiusY] = o;
                    }
                    RadiusY += 1;
                }
                RadiusY = 0;
                RadiusX += 1;
            }

            return vROI;
        }

        private double[,] calcCountsFromCenter(int[,] roi, string type)
        {
            //Variables
            bool counts1mmSet = false;
            bool counts1_5mmSet = false;
            double spotSizeDetectionThreshold = 0;
            double pixelCount = 0;
            double radius = 0;
            double binSize = 2;
            double windowMid = 0;
            double[,] IRCounts;
            int bin = 0;
            int bins = 0;
            int spotPixelCount = 0;
            int targetCounts;
            int[] BinPixelCount;
            int[] iCounts;
            int[] rCounts;
            double innerRadius = 0;
            double outRadius = 0;
            double radiusBinCenter = 0;

            //TotalCounts Setup
            spotSizeDetectionThreshold = 0.5 * this.spotMaxCounts;
            windowMid = roi.GetLength(1) / 2;
            bins = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(windowMid / binSize)));
            iCounts = new int[bins + 1];
            rCounts = new int[bins + 2];
            BinPixelCount = new int[bins + 1];
            IRCounts = new double[bins + 2, 2];

      for (int y = 0; y <= roi.GetLength(1) - 1; y++)// for (int y = 0; y <= this.xRayCounts.GetLength(2) - 1; y++)
      {
        for (int x = 0; x <= roi.GetLength(0) - 1; x++)// for (int x = 0; x <= this.xRayCounts.GetLength(1) - 1; x++)
        {
          radius = Math.Sqrt(Math.Pow((x - windowMid), 2) + Math.Pow((y - windowMid), 2));
          pixelCount = roi[x, y];//pixelCount = this.xRayCounts[imageNum, x, y];
          if (radius <= windowMid)
          {
            bin = Convert.ToInt32(Math.Floor(radius / binSize) + 1);
            if (pixelCount > 3000)//Settings.Default.Settings.minimumCountsAccepted)
            {
              if ((rCounts[bin] + pixelCount) <= int.MaxValue)
              {
                rCounts[bin] = Convert.ToInt32(rCounts[bin] + pixelCount);
              }
            }
            if (pixelCount >= spotSizeDetectionThreshold)
            {
              spotPixelCount++;
            }
          }
        }
      }

            targetCounts = rCounts[0];
            iCounts[0] = targetCounts;

            for (bin = 1; bin <= bins; bin++)
            {
                targetCounts = targetCounts + rCounts[bin];
                iCounts[bin] = targetCounts;
            }

            for (bin = 1; bin <= bins - 1; bin++)
            {
                outRadius = bin * binSize * pixelToMicron;
                innerRadius = (bin - 1) * binSize * pixelToMicron;
                radiusBinCenter = (innerRadius + outRadius) / 2;

                IRCounts[bin, 0] = radiusBinCenter;
                IRCounts[bin, 1] = iCounts[bin];
            }


            for (int i = 0; i < IRCounts.GetLength(0); i++)
            {
                if (IRCounts[i, 0] > 500 && !counts1mmSet)
                {
                    this.counts1mm = IRCounts[i, 1] / 1000000;
                    counts1mmSet = true;
                }

                if (IRCounts[i, 0] > 750 && !counts1_5mmSet)
                {
                    this.counts1_5mm = IRCounts[i, 1] / 1000000;
                    counts1_5mmSet = true;
                }
            }

            for (int i = IRCounts.GetLength(0) - 1; i > 0; i--)
            {
                if (IRCounts[i, 1] > 0)
                {
                    if (type == "Spot")
                    {
                        spotTotalCounts = IRCounts[i, 1];
                        break;
                    }
                    else
                    {
                        anodeTotalCounts = IRCounts[i, 1];
                        break;
                    }

                }

            }

            return IRCounts;

        }


        //-----------------------------------SERVICE FUNCTIONS-------------------------------

        private Point findCenterOfROI()
        {
            Point point = default(Point);
            Point tP;
            float vM1 = 0;
            float vM2;
            float vX;
            float vY;

            try
            {
                if (circlePoints[0].X == circlePoints[1].X || circlePoints[0].Y == circlePoints[1].Y)
                    swapPoints(1, 2);
                if (circlePoints[1].X == circlePoints[2].X)
                    swapPoints(0, 1);

                vM1 = ((float)circlePoints[1].Y - (float)circlePoints[0].Y) / (float)(circlePoints[1].X - (float)circlePoints[0].X);
                vM2 = ((float)circlePoints[2].Y - (float)circlePoints[1].Y) / (float)(circlePoints[2].X - (float)circlePoints[1].X);

                if (vM1 == 0 || vM2 == 0)
                {
                    circlePoints.Clear();
                    MessageBox.Show("The selected points do not produce a circle.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return Point.Empty;
                }

                vX = (vM1 * vM2 * (circlePoints[0].Y - circlePoints[2].Y) + vM2 * (circlePoints[0].X + circlePoints[1].X) - vM1 * (circlePoints[1].X + circlePoints[2].X)) / (2 * (vM2 - vM1));
                if (vM1 != 0)
                    vY = -(vX - (circlePoints[0].X + circlePoints[1].X) / 2) / vM1 + (circlePoints[0].Y + circlePoints[1].Y) / 2;
                else
                    vY = -(vX - (circlePoints[1].X + circlePoints[2].X) / 2) / vM2 + (circlePoints[1].Y + circlePoints[2].Y) / 2;

                point.X = (int)Math.Round(vX);
                point.Y = (int)Math.Round(vY);

                circlePointsSpot.Clear();


                for (int i = 0; i < 3; i++)
                {
                    tP = circlePoints[i];
                    circlePointsSpot.Add(tP);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to find center of Image\n" + ex);

            }

            return point;
        }

        private void swapPoints(int first, int second)
        {
            Point temp;
            temp = circlePoints[first];
            circlePoints[first] = circlePoints[second];
            circlePoints[second] = temp;
        }

        // -----------------------------------EVENT HANDLERS----------------------------------------------

        private void CCDImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();

            if (mouseDown && circlePoints.Count < 2)
            {
                this.Refresh();
                g.DrawLine(Pens.Red, e.X - 2, e.Y, e.X + 2, e.Y);
                g.DrawLine(Pens.Red, e.X, e.Y - 2, e.X, e.Y + 2);
            }

            if (mouseDown && circlePoints.Count == 2)
            {
                Point point = new Point();
                int x = 0;
                this.Refresh();
                g.DrawLine(Pens.Red, e.X - 2, e.Y, e.X + 2, e.Y);
                g.DrawLine(Pens.Red, e.X, e.Y - 2, e.X, e.Y + 2);
                point.X = e.X;
                point.Y = e.Y;

                if (!(point.Y == circlePoints[0].Y || point.Y == circlePoints[1].Y))
                    circlePoints.Add(point);

                this.Refresh();

                while (x < circlePoints.Count)
                {
                    if (Math.Abs(circlePoints[x].X - e.X) < 1 && Math.Abs(circlePoints[x].Y - e.Y) < 1)
                    {
                        circlePoints.RemoveAt(x);
                    }
                    x++;
                }
            }
        }

        public void CCDImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            int x = 0;
            this.mouseDown = true;
            while (x < circlePoints.Count)
            {
                if (Math.Abs(circlePoints[x].X - e.X) < 10 && Math.Abs(circlePoints[x].Y - e.Y) < 10)
                    circlePoints.RemoveAt(x);
                else
                    x++;
            }

        }

        private void CCDImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Point point = new Point();
            mouseDown = false;

            if (circlePoints.Count == 3)
                return;

            if (e.Button == MouseButtons.Left)
            {
                point.X = e.X;
                point.Y = e.Y;
                g.DrawLine(Pens.Red, point.X - 2, point.Y, point.X + 2, point.Y);
                g.DrawLine(Pens.Red, point.X, point.Y - 2, point.X, point.Y + 2);

                circlePoints.Add(point);

                if (circlePoints.Count == 3)
                    drawROICircle(g);

            }

        }

        private void CCDImageBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point center = new Point();
            int vRadius = new int();
            Pen pCenter = new Pen(Color.Black, 1);
            Pen crossPen = new Pen(Color.Red, 1);
            Pen crossPenSpot = new Pen(Color.Blue, 1);
            crossPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            crossPenSpot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            const int XLEN = 6;

            for (int vCount = 0; vCount < circlePoints.Count; vCount++)
            {
                g.DrawLine(Pens.Red, circlePoints[vCount].X - 2, circlePoints[vCount].Y, circlePoints[vCount].X + 2, circlePoints[vCount].Y);
                g.DrawLine(Pens.Red, circlePoints[vCount].X, circlePoints[vCount].Y - 2, circlePoints[vCount].X, circlePoints[vCount].Y + 2);
            }

            if (spotCenter != Point.Empty)
            {
                if (spotCenter.X > 0 & spotCenter.Y > 0)
                {
                    g.DrawLine(pCenter, spotCenter.X - XLEN, spotCenter.Y, spotCenter.X + XLEN, spotCenter.Y);
                    g.DrawLine(pCenter, spotCenter.X, spotCenter.Y - XLEN, spotCenter.X, spotCenter.Y + XLEN);
                }
            }


            if (circlePoints.Count == 3)
            {
                try
                {
                    center = findCenterOfROI();
                    vRadius = (int)Math.Floor(Math.Sqrt(Math.Pow((center.X - circlePoints[0].X), 2) + Math.Pow((center.Y - circlePoints[0].Y), 2)));
                    vRadius += (int)Math.Floor(Math.Sqrt(Math.Pow((center.X - circlePoints[1].X), 2) + Math.Pow((center.Y - circlePoints[1].Y), 2)));
                    vRadius += (int)Math.Floor(Math.Sqrt(Math.Pow((center.X - circlePoints[2].X), 2) + Math.Pow((center.Y - circlePoints[2].Y), 2)));
                    vRadius = vRadius / 3;

                    if (isSpotCenterOfROI)
                    {
                        float gRadius = 2000 / (2 * vRadius);
                        float rAnodeLimit = (Convert.ToSingle(25) / 2) / (gRadius);
                        g.DrawEllipse(Pens.Green, Convert.ToSingle(center.X - rAnodeLimit), Convert.ToSingle(center.Y - rAnodeLimit), rAnodeLimit * 2, rAnodeLimit * 2);
                    }

                    g.DrawEllipse(Pens.Red, center.X - vRadius, center.Y - vRadius, 2 * vRadius, 2 * vRadius);
                    g.DrawLine(crossPen, center.X - vRadius, center.Y, center.X + vRadius, center.Y);
                    g.DrawLine(crossPen, center.X, center.Y - vRadius, center.X, center.Y + vRadius);
                }
                catch
                {
                }
            }
        }



        //double[,] anodeCenter;


        private int getFluxAndChartData(double[,] ROICounts, ref double[,] chartData, double totalCounts)
        {
            double centerX = 0;
            double centerY = 0;
            bool stillBelow = true;
            int theFlux = 0;
            int lastIndex;

            chartData = new double[ROICounts.GetLength(0), 2];

            for (lastIndex = ROICounts.GetLength(0) - 1; lastIndex > 0; lastIndex--)
            {
                if (ROICounts[lastIndex, 0] > 0)
                    break;
            }

            for (int X = 0; X < lastIndex; X++)
            {
                centerX = ROICounts[X, 0] * 2;
                centerY = 100 * ROICounts[X, 1] / totalCounts;

                //Not the cleanest... but this was a good place to insert the data for the 70%anode and 70%spot so that it can 
                //be displayed in chart form in CCDForm.cs
                chartData[X, 0] = Math.Floor(centerX);
                chartData[X, 1] = Math.Floor(centerY); // 'Format(cX, "0.0")


                if (centerY >= fluxPercent && stillBelow)
                {
                    theFlux = Get70x(X, totalCounts, ROICounts, pixelToMicron);
                    stillBelow = false;
                }
            }
            return theFlux;
        }

        private int Get70x(int g70, double totalCounts, double[,] ROICounts, double pixelToMicron)
        {
            double rAnodeLimit;
            double rSpotLimit;
            int fluxSpot;

            if (g70 == 0)
                return 0;

            double y1, y2;
            double x1, x2;
            y1 = 100 * ROICounts[g70 - 1, 1] / totalCounts;
            y2 = 100 * ROICounts[g70, 1] / totalCounts;
            x1 = ROICounts[g70 - 1, 0] * 2;
            x2 = ROICounts[g70, 0] * 2;
            double get70x = (x1 + (x2 - x1) * ((fluxPercent - y1) / (y2 - y1)));

            rAnodeLimit = (get70x / pixelToMicron) / 2; //'(2032 / 298)) / 2 ' microns/(microns per pixel) /2 = radius

            rSpotLimit = (get70x / pixelToMicron) / 2;


            fluxSpot = Convert.ToInt32(get70x);
            return fluxSpot;

        }

        //KIRK B's Code converted from VB to C# (It was more complicated that i wanted to deal with to summerize and simplify, So somehow this spits out the MinDim and MaxDim values
        //which may or may not be needed)
        public object SpotSize(int[,] roi)
        {
            int[,] SmoothROI = roi;
            int NumRows = roi.GetLength(0) - 1;
            int NumCols = roi.GetLength(1) - 1;
            int NumSmooths = 2;
            int row = 0;
            int col = 0;
            double DiagSum = 0;
            double CrossSum = 0;
            double RowSum = 0;
            double ColSum = 0;
            double Weights = 0;
            double[,] ROId = new double[NumRows + 1, NumCols + 1];
            double CutOff = 0.33;
            int c = 0;


            for (c = 0; c <= NumSmooths; c++)
            {
                //Smoothing of interior points
                Weights = 4 + (4 / Math.Sqrt(2));
                for (row = 1; row <= NumRows - 1; row++)
                {
                    for (col = 1; col <= NumCols - 1; col++)
                    {
                        DiagSum = roi[row - 1, col - 1] + roi[row - 1, col + 1] + roi[row + 1, col - 1] + roi[row + 1, col + 1];
                        CrossSum = roi[row, col - 1] + roi[row, col + 1] + roi[row - 1, col] + roi[row + 1, col];
                        SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);
                    }
                }

                //Smoothing of Top
                Weights = 3 + 2 / Math.Sqrt(2);
                row = 0;
                for (col = 1; col <= NumCols - 1; col++)
                {
                    DiagSum = roi[row + 1, col - 1] + roi[row + 1, col + 1];
                    CrossSum = roi[row, col - 1] + roi[row, col + 1] + roi[row + 1, col];
                    SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);
                }

                //Smoothing of Bottom
                row = NumRows;
                for (col = 1; col <= NumCols - 1; col++)
                {
                    DiagSum = roi[row - 1, col - 1] + roi[row - 1, col + 1];
                    CrossSum = roi[row, col - 1] + roi[row, col + 1] + roi[row - 1, col];
                    SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);
                }

                //Smoothing of Left Side
                col = 0;
                for (row = 1; row <= NumRows - 1; row++)
                {
                    DiagSum = roi[row - 1, col + 1] + roi[row + 1, col + 1];
                    CrossSum = roi[row, col + 1] + roi[row - 1, col] + roi[row + 1, col];
                    SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);
                }

                //Smoothing of Right Side
                col = NumCols;
                for (row = 1; row <= NumRows - 1; row++)
                {
                    DiagSum = roi[row - 1, col - 1] + roi[row + 1, col - 1];
                    CrossSum = roi[row, col - 1] + roi[row - 1, col] + roi[row + 1, col];
                    SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);
                }

                //Smoothing of Corners
                Weights = 2 + 1 / Math.Sqrt(2);
                row = 0;
                col = 0;
                DiagSum = roi[row + 1, col + 1];
                CrossSum = roi[row, col + 1] + roi[row + 1, col];
                SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);

                row = 0;
                col = NumCols;
                DiagSum = roi[row + 1, col - 1];
                CrossSum = roi[row, col - 1] + roi[row + 1, col];
                SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);

                row = NumRows;
                col = 0;
                DiagSum = roi[row - 1, col + 1];
                CrossSum = roi[row, col + 1] + roi[row - 1, col];
                SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);

                row = NumRows;
                col = NumCols;
                DiagSum = roi[row - 1, col - 1];
                CrossSum = roi[row - 1, col] + roi[row, col - 1];
                SmoothROI[row, col] = Convert.ToInt32((roi[row, col] + (DiagSum / Math.Sqrt(2) + CrossSum) / Weights) / 2);

                roi = SmoothROI;
            }
            int spotMax = 0;
            for (row = 0; row <= NumRows; row++)
            {
                for (col = 0; col <= NumCols; col++)
                {
                    if (roi[row, col] > spotMax)
                        spotMax = roi[row, col];
                }
            }

            RowSum = 0;
            ColSum = 0;

            int HotCount = 0;
            for (row = 0; row <= NumRows; row++)
            {
                for (col = 0; col <= NumCols; col++)
                {
                    double tempval = Convert.ToDouble(roi[row, col]) / spotMax;
                    ROId[row, col] = tempval;
                    if (tempval > CutOff)
                    {
                        RowSum = RowSum + row;
                        ColSum = ColSum + col;
                        HotCount = HotCount + 1;
                    }
                }
            }

            int RowCenter = Convert.ToInt32(RowSum / HotCount);
            int ColCenter = Convert.ToInt32(ColSum / HotCount);

            int ThetaInc = 2;
            int minDim = spotMax;
            int maxDim = 0;
            int Theta = 0;
            double r = 0;
            double r1 = 0;
            double r2 = 0;
            double thetar1 = 0;
            double thetar2 = 0;
            double costr = 0;
            double sintr = 0;
            bool FoundHot = false;

            for (Theta = 0; Theta <= (180 - ThetaInc); Theta = Theta + 2)
            {
                r1 = 0;
                r2 = 0;
                thetar1 = 2 * Math.PI * Theta / 360;
                costr = Math.Cos(thetar1);
                sintr = Math.Sin(thetar1);
                FoundHot = false;
                r = Math.Round(Math.Sqrt(2) * NumRows / 2);
                while (!FoundHot)
                {
                    r = r - 1;
                    if (r < 0)
                    {
                        FoundHot = true;
                        r = NumRows / 2;
                    }
                    row = Convert.ToInt32(Math.Round(RowCenter + r * sintr));
                    col = Convert.ToInt32(Math.Round(ColCenter + r * costr));
                    if (row > 0 & row < NumRows & col > 0 & col < NumCols)
                    {
                        if (ROId[row, col] > CutOff)
                        {
                            r1 = r;
                            FoundHot = true;
                        }
                    }
                }

                thetar2 = 2 * Math.PI * (Theta + 180) / 360;
                costr = Math.Cos(thetar2);
                sintr = Math.Sin(thetar2);
                FoundHot = false;
                r = Convert.ToInt32(Math.Round(Math.Sqrt(2) * NumRows / 2));
                while (!FoundHot)
                {
                    r = r - 1;
                    if (r < 0)
                    {
                        FoundHot = true;
                        r = NumRows / 2;
                    }
                    row = Convert.ToInt32(Math.Round(RowCenter + r * sintr));
                    col = Convert.ToInt32(Math.Round(ColCenter + r * costr));
                    if (row > 0 & row < NumRows & col > 0 & col < NumCols)
                    {
                        if (ROId[row, col] > CutOff)
                        {
                            r2 = r;
                            FoundHot = true;
                        }
                    }
                }
                if (r1 > 0 & r2 > 0)
                {
                    if (r1 + r2 > maxDim)
                        maxDim = Convert.ToInt32(r1 + r2);
                    if (r1 + r2 < minDim)
                        minDim = Convert.ToInt32(r1 + r2);
                }
            }

            this.maxDim = Math.Round(Convert.ToDouble(maxDim * DIAMETER / NumRows)) / 1000;
            //2032
            this.minDim = Math.Round(Convert.ToDouble(minDim * DIAMETER / NumRows)) / 1000;
            //2032          
            return roi;
        }




        
    }
}



 //private string Get70x(int g70, double totalCounts, double[,] ROICounts, double pixelToMicron)
 //       {
 //           double rAnodeLimit;
 //           double rSpotLimit;
 //           int fluxSpot;

 //           if (g70 == 0)
 //               return string.Empty;

 //           double y1, y2;
 //           double x1, x2;
 //           y1 = 100 * ROICounts[g70 - 1, 1] / totalCounts;
 //           y2 = 100 * ROICounts[g70, 1] / totalCounts;
 //           x1 = ROICounts[g70 - 1, 0] * 2;
 //           x2 = ROICounts[g70, 0] * 2;
 //           double get70x = (x1 + (x2 - x1) * ((Settings.Default.Settings.fluxPercent - y1) / (y2 - y1)));

 //           rAnodeLimit = (get70x / pixelToMicron) / 2; //'(2032 / 298)) / 2 ' microns/(microns per pixel) /2 = radius

 //           rSpotLimit = (get70x / pixelToMicron) / 2;


 //           fluxSpot = Convert.ToInt32(get70x);
 //           return fluxSpot.ToString();

 //       }

 //       private string displaySeventyPercentChart(double[,] ROICounts, double totalCounts, int chartSeriesNum, double pixelToMicron)
 //       {
 //           double centerX = 0;
 //           double centerY = 0;
 //           bool stillBelow = true;
 //           string theFlux = string.Empty;

 //           charFluxContained.Series[chartSeriesNum].Points.Clear();

 //           charFluxContained.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = Settings.Default.Settings.fluxPercent;

 //           int lastIndex;
 //           for (lastIndex = ROICounts.GetLength(0) - 1; lastIndex > 0; lastIndex--)
 //           {
 //               if (ROICounts[lastIndex, 0] > 0)
 //                   break;
 //           }

 //           for (int X = 0; X < lastIndex; X++)
 //           {
 //               centerX = ROICounts[X, 0] * 2;
 //               centerY = 100 * ROICounts[X, 1] / totalCounts;
 //               charFluxContained.Series[chartSeriesNum].Points.AddXY(Convert.ToInt32(centerX), centerY); // 'Format(cX, "0.0")
 //               if (centerY >= Settings.Default.Settings.fluxPercent && stillBelow)
 //               {
 //                   theFlux = Get70x(X, totalCounts, ROICounts, pixelToMicron);
 //                   stillBelow = false;
 //               }
 //           }



 //           return theFlux;
 //       }
