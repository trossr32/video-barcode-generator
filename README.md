# Video Barcode Generator 
 Video Barcode Generator is a WPF application that creates coloured barcode images from frames taken from a supplied video file.

 Two types of barcode can be created:

 ## Single colour (standard)

 Frames are taken from the video file and converted to images. These images are then scaled to 70% to focus on the core of the frame, shrunk to a pixel and this aggregated colour then used to create a 1 pixel width line which is then added to the barcode image. Here's an example created from the 2009 film Avatar:

 ## 1 pixel compressed

 This follows the same basic process as creating a single colour barcode, but frame images are compressed to a pixel width and added to the barcode image rather than calculating an aggrgated frame colour. Using Avatar as an example again:

# Getting Started

Download the latest release from the release page, unzip the archive and double-click the executable. No installer is provided.

Alternatively clone the repository and open the solution in Visual Studio 2019. .NET Framework version 4.8 is required.

# Using the app

Import a video file by clicking the import button or dragging and dropping a file over the button. Video files must be imported singly at present, multiple file import is not supported.

By default the created image will be 600 pixels in height and the width will be 1 pixel for every second of the video. These settings can be amended using the supplied fields or quick setting buttons.

A standard single colour barcode image will always be created, but you can choose whether to also create the 1 pixel compressed barcode.

You can set the output directory root and each video file run will be saved to its own directory within this root with a name of {video name}_yyyyMMdd_HHmmss. In this directory the following will be created:

* The barcode images
* A videocollection.json configuration file containing the settings used for this run, including a collection that points to all created frame images and all colour codes used to generate the standard barcode.
* A frame images directory containing all frame images created during processing.
* If a 1 pixel barcode is being created, a frame images directory containg all the 1 pixel width images used to create the final barcode.

You can choose for the frame images to be archived to a zip at the end of a run to reclaim some disk space. Note that frame images can consume a lot of disk space and you may want to clean up the run directory once the task has completed (for example, the Avatar file used in the above example created 1.11Gb of frame images). 

Video files are processed in background tasks and any number of taks can be added. Up to 3 tasks can be run concurrently, any further tasks will be queued.

# Considerations

Created frame image sizes are directly linked to the resolution of the video being processed.

Processing time will also be directly linked to the size and resolution of the video file, as well as the number of frames that need to be processed (i.e. the width of the barcode being created).

# Examples

Here are some example barcodes created using this app:

### Deadpool (2016)

[![Deadpool (2016) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Deadpool%20(2016).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Deadpool%20(2016).jpg)

[![Deadpool (2016) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Deadpool%20(2016)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Deadpool%20(2016)_1px.jpg)

### The Matrix (1999)

[![The Matrix (1999) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/The%20Matrix%20(1999).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/The%20Matrix%20(1999).jpg)

[![The Matrix (1999) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/The%20Matrix%20(1999)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/The%20Matrix%20(1999)_1px.jpg)

### Star Wars Episode V - The Empire Strikes Back (1981)

[![Star Wars Episode V - The Empire Strikes Back (1981) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Star%20Wars%20Episode%20V%20-%20The%20Empire%20Strikes%20Back%20(1981).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Star%20Wars%20Episode%20V%20-%20The%20Empire%20Strikes%20Back%20(1981).jpg)

[![Star Wars Episode V - The Empire Strikes Back (1981) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Star%20Wars%20Episode%20V%20-%20The%20Empire%20Strikes%20Back%20(1981)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Star%20Wars%20Episode%20V%20-%20The%20Empire%20Strikes%20Back%20(1981)_1px.jpg)

### Finding Nemo (2003)

[![Finding Nemo (2003) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Finding%20Nemo%20(2003).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Finding%20Nemo%20(2003).jpg)

[![Finding Nemo (2003) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Finding%20Nemo%20(2003)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Finding%20Nemo%20(2003)_1px.jpg)

### Interstellar (2014)

[![Interstellar (2014) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Interstellar%20(2014).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Interstellar%20(2014).jpg)

[![Interstellar (2014) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Interstellar%20(2014)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Interstellar%20(2014)_1px.jpg)

### The Fifth Element (1997)

[![The Fifth Element (1997) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/The%20Fifth%20Element%20(1997).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/The%20Fifth%20Element%20(1997).jpg)

[![The Fifth Element (1997) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/The%20Fifth%20Element%20(1997)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/The%20Fifth%20Element%20(1997)_1px.jpg)

### Transformers (2007)

[![Transformers (2007) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Transformers%20(2007).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Transformers%20(2007).jpg)

[![Transformers (2007) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Transformers%20(2007)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Transformers%20(2007)_1px.jpg)

### Total Recall (1990)

[![Total Recall (1990) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Total%20Recall%20(1990).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Total%20Recall%20(1990).jpg)

[![Total Recall (1990) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Total%20Recall%20(1990)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Total%20Recall%20(1990)_1px.jpg)

### Spiderman (2002)

[![Spiderman (2002) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Spiderman%201%20(2002).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Spiderman%201%20(2002).jpg)

[![Spiderman (2002) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Spiderman%201%20(2002)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Spiderman%201%20(2002)_1px.jpg)

### Shrek (2001)

[![Shrek (2001) single colour](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Shrek%20(2001).jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Shrek%20(2001).jpg)

[![Shrek (2001) 1px](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Shrek%20(2001)_1px.jpg)](https://github.com/trossr32/video-barcode-generator/blob/master/Example%20Barcodes/Full/Shrek%20(2001)_1px.jpg)

# Contribute
If you can make this extension better I'm happy for the help! Create a pull request and get in touch. Alternatively feel free to raise an issue if you've found a bug.
