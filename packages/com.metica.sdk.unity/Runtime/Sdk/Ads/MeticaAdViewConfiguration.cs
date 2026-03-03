namespace Metica.Ads
{
    /// <summary>
    /// Position for banner and MREC ad views.
    ///
    /// WARNING: This enum MUST be kept in sync with Android's AdViewPosition and iOS's Position enum!
    /// Android enum location: ads/src/main/kotlin/com/metica/unity_bridge/internal/UnityBannersAndMrec.kt
    /// Any changes to enum names or values MUST be mirrored in both files and platforms.
    /// This enum name is passed as a string via JNI, and Android parses it back to AdViewPosition.
    /// Mismatched enums will cause runtime crashes on Android via error() in AdViewPosition.fromString().
    /// </summary>
    public enum MeticaAdViewPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        Centered,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    public class MeticaAdViewConfiguration {
        public MeticaAdViewPosition? Position { get; }
        public double? XCoordinate { get; }
        public double? YCoordinate { get; }

        public MeticaAdViewConfiguration(MeticaAdViewPosition position)
        {
            Position = position;
            XCoordinate = null;
            YCoordinate = null;
        }

        public MeticaAdViewConfiguration(double xCoordinate, double yCoordinate)
        {
            Position = null;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }
    }
}
