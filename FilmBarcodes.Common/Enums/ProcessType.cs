namespace FilmBarcodes.Common.Enums
{
    public enum ProcessType
    {
        BuildColourList = 0,
        BuildOnePixelImage,
        ZipArchive,
        UnZipArchive,
        DeleteFrameImages,
        RenderImage,
        RenderImageCompressedToOnePixelWide,
        WriteVideoFile
    }
}