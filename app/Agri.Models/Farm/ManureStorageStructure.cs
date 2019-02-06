using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Farm
{
    public class ManureStorageStructure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStructureCovered => !UncoveredAreaSquareFeet.HasValue;
        public int? UncoveredAreaSquareFeet { get; set; }
        public StorageShapes SelectedStorageStructureShape { get; set; }
        public decimal? RectangularLength { get; set; }
        public decimal? RectangularWidth { get; set; }
        public decimal? RectangularHeight { get; set; }
        public decimal? CircularDiameter { get; set; }
        public decimal? CircularHeight { get; set; }
        public decimal? SlopedWallTopLength { get; set; }
        public decimal? SlopedWallTopWidth { get; set; }
        public decimal? SlopedWallHeight { get; set; }
        public decimal? SlopedWallSlopeOfWall { get; set; }
        public int? surfaceArea { get; set; }
        public int? volumeUSGallons { get; set; }
        public string volumeOfStorageStructure { get; set; }
    }
}
