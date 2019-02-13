using System;
using Agri.Models;
using Agri.Models.Configuration;

namespace Agri.Interfaces
{
    public interface IStorageVolumeCalculator
    {
        int GetSurfaceAreaOfRectangle(decimal? length, decimal? width, decimal? height);
        int GetVolumeFT3OfRectangle(decimal? length, decimal? width, decimal? height);
        int GetVolumeUSGallonsOfRectangle(decimal? length, decimal? width, decimal? height);
        int GetSurfaceAreaOfCircle(decimal? diameter);
        int GetVolumeFT3OfCircle(decimal? diameter, decimal? height);
        int GetVolumeUSGallonsOfCircle(decimal? diameter, decimal? height);
        int GetSurfaceAreaOfSlopedWall(decimal? topLength, decimal? topWidth);
        int GetVolumeFT3OfSlopedWall(decimal? topLength, decimal? topWidth, decimal? height, decimal? slopeOfWall);
        int GetVolumeUSGallonsOfSlopedWall(decimal? topLength, decimal? topWidth, decimal? height, decimal? slopeOfWall);
    }
}