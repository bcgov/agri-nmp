using Agri.Data;
using System;

namespace Agri.CalculateService
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

    public class StorageVolumeCalculator : IStorageVolumeCalculator
    {
        private IAgriConfigurationRepository _repository;

        public StorageVolumeCalculator(IAgriConfigurationRepository repository)
        {
            _repository = repository;
        }

        public int GetSurfaceAreaOfRectangle(decimal? length, decimal? width, decimal? height)
        {
            var surfaceArea = (int)Math.Round((length ?? 0) * (width ?? 0));
            return surfaceArea;
        }

        public int GetVolumeFT3OfRectangle(decimal? length, decimal? width, decimal? height)
        {
            var freeBoard = 1;
            var activeHeight = height - freeBoard;
            var volumeFT3 = (int)Math.Round((activeHeight ?? 0) * GetSurfaceAreaOfRectangle(length, width, height));
            return volumeFT3;
        }

        public int GetVolumeUSGallonsOfRectangle(decimal? length, decimal? width, decimal? height)
        {
            var volumeUSGallons = (int)Math.Round(7.48052 * GetVolumeFT3OfRectangle(length, width, height));
            return volumeUSGallons;
        }

        public int GetSurfaceAreaOfCircle(decimal? diameter)
        {
            var surfaceArea = (int)Math.Round((3.1428) * Math.Pow(Convert.ToDouble(diameter) / 2, 2));
            return surfaceArea;
        }

        public int GetVolumeFT3OfCircle(decimal? diameter, decimal? height)
        {
            var freeBoard = 1;
            var activeHeight = height - freeBoard;
            var volumeFT3 = (int)Math.Round(Convert.ToDouble(activeHeight) * ((22 / 7.0) * Math.Pow(Convert.ToDouble(diameter) / 2, 2)));
            return volumeFT3;
        }

        public int GetVolumeUSGallonsOfCircle(decimal? diameter, decimal? height)
        {
            var freeBoard = 1;
            var activeHeight = height - freeBoard;
            var volumeUSGallons = (int)Math.Round(7.48052 * (Convert.ToDouble(activeHeight) * ((22 / 7.0) * Math.Pow(Convert.ToDouble(diameter) / 2, 2))));
            return volumeUSGallons;
        }

        public int GetSurfaceAreaOfSlopedWall(decimal? topLength, decimal? topWidth)
        {
            var surfaceArea = (int)Math.Round((topLength ?? 0) * (topWidth ?? 0));
            return surfaceArea;
        }

        public int GetVolumeFT3OfSlopedWall(decimal? topLength, decimal? topWidth, decimal? height, decimal? slopeOfWall)
        {
            var freeBoard = 1;
            var activeHeight = height - freeBoard;
            var bottomLength = topLength - 2 * height / slopeOfWall;
            var bottomWidth = topWidth - 2 * height / slopeOfWall;
            var areaBottom = bottomLength * bottomWidth;

            var volumeFT3 = (int)Math.Round(Convert.ToDouble(activeHeight / 3) *
                                             (Convert.ToDouble(areaBottom) + Convert.ToDouble((topLength * topWidth)) +
                                              Math.Sqrt(Convert.ToDouble(areaBottom) * Convert.ToDouble((topLength * topWidth)))));
            return volumeFT3;
        }

        public int GetVolumeUSGallonsOfSlopedWall(decimal? topLength, decimal? topWidth, decimal? height, decimal? slopeOfWall)
        {
            var freeBoard = 1;
            var activeHeight = height - freeBoard;
            var bottomLength = topLength - 2 * height / slopeOfWall;
            var bottomWidth = topWidth - 2 * height / slopeOfWall;
            var areaBottom = bottomLength * bottomWidth;
            var volumeUSGallons = (int)Math.Round(7.48052 * (Convert.ToDouble(activeHeight / 3) *
                                                             (Convert.ToDouble(areaBottom) + Convert.ToDouble((topLength * topWidth)) +
                                                              Math.Sqrt(Convert.ToDouble(areaBottom) * Convert.ToDouble((topLength * topWidth))))));
            return volumeUSGallons;
        }
    }
}