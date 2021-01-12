using System;
using System.Numerics;

namespace Raytracing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Camera position and direction
            var cameraPos = new Vector3(0, 0, -6);
            var cameraDir = new Vector3(0, 0, 1);
            
            // Projection plane position is at 1 unit away from camera
            var planeOrigin = cameraPos + Vector3.Normalize(cameraDir);
            float distanceFromPlaneToCamera = (cameraPos - planeOrigin).Length();

            // Field of view in degrees and converted to radians
            float fov = 60;
            var fovInRad = fov / 180f * Math.PI;

            // Screen size in pixels
            int screenWidth = 640, screenHeight = 480;
            bool[,] screenBuffer = new bool[screenWidth, screenHeight];

            // Max units visible from the center of the plane
            float realPlaneSize = (float)(distanceFromPlaneToCamera * Math.Tan(fovInRad));

            float realPlaneWidth = realPlaneSize * (screenWidth / (float) Math.Max(screenHeight,screenWidth));
            float realPlaneHeight = realPlaneSize * (screenHeight / (float) Math.Max(screenHeight, screenWidth));

            var sphereToDraw = new Sphere(new Vector3(0, 0, 5), 1);

            for (var x = 0; x < screenWidth; x++)
			{
                for (var y = 0; y < screenHeight; y++)
				{
                    // Normalized coordinates of pixel to [-1; 1] interval
                    var xNorm = (x - screenWidth / 2) / (float)screenWidth;
                    var yNorm = (y - screenHeight / 2) / (float)screenHeight;

                    var positionOnPlane =
                        planeOrigin + new Vector3(xNorm * realPlaneWidth / 2, yNorm * realPlaneHeight / 2, 0);

                    var rayDirection = positionOnPlane - cameraPos;
                    if (IsRayIntersectsSphere(positionOnPlane, rayDirection, sphereToDraw))
					{
                        screenBuffer[x, y] = true;
					}
				}
			}

            ScreenImageCreator.CreateAndSaveFromBoolBuffer(screenBuffer);
        }

        private static bool IsRayIntersectsSphere(Vector3 rayOrigin, Vector3 rayDirection, Sphere sphere)
		{
            Vector3 l = sphere.center - rayOrigin;
            float tca = Vector3.Dot(l, rayDirection);
            if (tca < 0)
			{
                return false;
			}

            float d2 = Vector3.Dot(l, l) - tca * tca;
            if (d2 > sphere.radius * sphere.radius)
			{
                return false;
			}

            return true;
		}
    }
}
