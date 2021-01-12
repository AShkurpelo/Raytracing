using System.Drawing;

namespace Raytracing
{
	public static class ScreenImageCreator
	{
		public static void CreateAndSaveFromBoolBuffer(bool[,] screenBuffer, Color? falseColor = null, Color? trueColor = null)
		{
			falseColor ??= Color.White;
			trueColor ??= Color.Black;

			var width = screenBuffer.GetLength(0);
			var height = screenBuffer.GetLength(1);
			var bitmap = new Bitmap(width, height);

			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					bitmap.SetPixel(x, y, screenBuffer[x, y] ? trueColor.Value : falseColor.Value);
				}
			}

			bitmap.Save("image.png");
		}
	}
}
