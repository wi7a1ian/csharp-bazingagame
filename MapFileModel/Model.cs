using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapFileModel
{
    public enum MapSpriteTile
    {
        PlatformLeftEdge,
        PlatformMiddle,
        PlatformRightEdge,
        GroundMiddle,
        GroundLeftEdge,
        GroundRightEdge,
        GroundLeftEnd,
        GroundRightEnd,
        GroundLeftInnerCorner,
        GroundRightInnerCorner,
        GroundLeft,
        GroundRight,
        GroundBottom,
        Ground,
        GroundLeftoOuterCorner,
        GroundRightOuterCorner,
        None
    }

    [Serializable]
    public struct MapTileFileInfo
    {
        public MapSpriteTile TileSprite;
    }
}
