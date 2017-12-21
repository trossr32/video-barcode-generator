using NReco.VideoInfo;

namespace VideoProcessor
{
    public static class Helpers
    {
        public static MediaInfo GetVideoInfo(string file)
        {
            return new FFProbe().GetMediaInfo(file);
        }
    }
}