using System;
using System.Collections.Generic;
using System.Text;
using Leadtools;
using Leadtools.Annotations;
using Leadtools.Codecs;
using Leadtools.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Leadtools.Drawing;

namespace Sempra.Confirm.InBound.ImageEdit
{
    enum ZoomSize { ZoomIn, ZoomOut, Normal }
    ;
    class ImageSizing
    {
        private RasterImageViewer viewer;
        public static readonly float _minimumScaleFactor = 0.05f;
        public static readonly float _maximumScaleFactor = 11f;
        private double imageScaleFactor;

        public ImageSizing(RasterImageViewer viewer)
        {
            this.viewer = viewer;
            imageScaleFactor = this.viewer.ScaleFactor; 
        }
        public void Zoom(ZoomSize size)
        {
            

            // zoom
            double scaleFactor = viewer.ScaleFactor;

            const float ratio = 1.2F;

            if (size == ZoomSize.ZoomIn)
            {
                scaleFactor *= ratio;
            }
            else if (size == ZoomSize.ZoomOut)
            {
                scaleFactor /= ratio;
            }
            else if (size == ZoomSize.Normal)
            {
                scaleFactor = 1;
            
            }
            imageScaleFactor = scaleFactor;
            SetScale(scaleFactor);
            
        }
        public double getScaleFactor()
        {
            return imageScaleFactor;
        }

        public void SetScale(double scaleFactor)
        {
            // get the current center in logical units

            Rectangle rc = Rectangle.Intersect(viewer.PhysicalViewRectangle, viewer.ClientRectangle); // get what you see in physical coordinates
            PointF center = new PointF(rc.Left + rc.Width / 2, rc.Top + rc.Height / 2); // get the center of what you see in physical coordinates
            Transformer t = new Transformer(viewer.Transform);
            center = t.PointToLogical(center);  // get the center of what you see in logical coordinates


            if (scaleFactor == 1)
            {
                if (viewer.SizeMode != RasterPaintSizeMode.Normal)
                    viewer.SizeMode = RasterPaintSizeMode.Normal;
            }

            scaleFactor = Math.Max(_minimumScaleFactor, Math.Min(_maximumScaleFactor, scaleFactor));

            if (viewer.ScaleFactor != scaleFactor)
            {
                viewer.ScaleFactor = scaleFactor;

                // bring the original center into the view center
                t = new Transformer(viewer.Transform);
                center = t.PointToPhysical(center); // get the center of what you saw before the zoom in physical coordinates
                viewer.CenterAtPoint(Point.Round(center)); // bring the old center into the center of the view
            }
        }
        public void RotateImageAndAnnotations(int angle, AnnContainer container)
        {
            
            // round the angle
            angle %= 360;

            if (angle == 0)
                return;  // nothing to do

            // when we rotate an image, this is what happens:
            //  1. the image is rotated around its center (width/2, height/2)
            //  2. the image is translated so that its top,left position is at 0,0

            // to calculate this translation, we:
            //  1. find the 4 end points of the container
            //  2. rotate these points
            //  3. find the minimum/maximum "out of range" point
            //  4. calculate the translation (amount to bring this "out of range" point back into view)
            PointF[] pts =
         {
            new PointF(0, 0),
            new PointF(viewer.Image.ImageWidth, 0),
            new PointF(viewer.Image.ImageWidth, viewer.Image.ImageHeight),
            new PointF(0, viewer.Image.ImageHeight)
         };

            PointF origin = new PointF(viewer.Image.ImageWidth / 2, viewer.Image.ImageHeight / 2);

            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, origin);
                m.TransformPoints(pts);
            }

            float xMin = pts[0].X;
            float yMin = pts[0].Y;

            for (int i = 1; i < pts.Length; i++)
            {
                if (pts[i].X < xMin)
                    xMin = pts[i].X;

                if (pts[i].Y < yMin)
                    yMin = pts[i].Y;
            }

            float xTranslate = -xMin;
            float yTranslate = -yMin;

            // now, rotate the image
            // Note, in this demo we will rotate only the view perspective, but you can use the RotateCommand to rotate
            // the annotations and the image around angle angle
            viewer.Image.RotateViewPerspective(angle);

            AnnPoint annOrigin = new AnnPoint(origin, AnnUnit.Pixel);

            // rotate and translate the annotations
            foreach (AnnObject obj in container.Objects)
            {
                obj.Rotate(angle, annOrigin);

                if (xTranslate != 0 || yTranslate != 0)
                    obj.Translate(xTranslate, yTranslate);
            }

            // re-set the container bounds
            container.Bounds = new AnnRectangle(0, 0, viewer.Image.ImageWidth, viewer.Image.ImageHeight);
        }


    }
}
