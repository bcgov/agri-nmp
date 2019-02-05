using System;
using Agri.Models;
using Agri.Models.Configuration;

namespace Agri.Interfaces
{
    public interface IStorageVolumeCalculator
    {
        int GetSurfaceAreaOfRectangle(decimal length, decimal width, decimal height);
        int GetSurfaceAreaOfCircle(decimal diameter);
        int GetSurfaceAreaOfSlopedWall(decimal topLength, decimal topWidth);
    }
}