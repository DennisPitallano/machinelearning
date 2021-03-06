﻿using System.IO;
using Microsoft.ML.Data;

namespace Microsoft.ML.Samples.Dynamic
{
    public static class LoadImages
    {
        // Loads the images of the imagesFolder into an IDataView. 
        public static void Example()
        {
            var mlContext = new MLContext();

            // Downloading a few images, and an images.tsv file, which contains a list of the files from the dotnet/machinelearning/test/data/images/.
            // If you inspect the fileSystem, after running this line, an "images" folder will be created, containing 4 images, and a .tsv file
            // enumerating the images. 
            var imagesDataFile = SamplesUtils.DatasetUtils.DownloadImages();

            // Preview of the content of the images.tsv file
            //
            // imagePath    imageType
            // tomato.bmp   tomato
            // banana.jpg   banana
            // hotdog.jpg   hotdog
            // tomato.jpg   tomato

            var data = mlContext.Data.CreateTextLoader(new TextLoader.Options()
            {
                Columns = new[]
                {
                        new TextLoader.Column("ImagePath", DataKind.String, 0),
                        new TextLoader.Column("Name", DataKind.String, 1),
                    }
            }).Load(imagesDataFile);

            var imagesFolder = Path.GetDirectoryName(imagesDataFile);
            // Image loading pipeline. 
            var pipeline = mlContext.Transforms.LoadImages(imagesFolder, ("ImageReal", "ImagePath"));
            var transformedData = pipeline.Fit(data).Transform(data);

            // The transformedData IDataView contains the loaded images now
            //Preview of the transformedData
            var transformedDataPreview = transformedData.Preview();

            // Preview of the content of the images.tsv file
            // The actual images, in the ImageReal column are of type System.Drawing.Bitmap.
            //
            // ImagePath    Name        ImageReal
            // tomato.bmp   tomato      {System.Drawing.Bitmap}
            // banana.jpg   banana      {System.Drawing.Bitmap}
            // hotdog.jpg   hotdog      {System.Drawing.Bitmap}
            // tomato.jpg   tomato      {System.Drawing.Bitmap}

        }
    }
}
