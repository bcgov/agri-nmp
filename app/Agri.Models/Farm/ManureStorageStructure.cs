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
        public int? RectangularLength { get; set; }
        public int? RectangularWidth { get; set; }
        public int? RectangularHeight { get; set; }
        public int? CircularDiameter { get; set; }
        public int? CircularHeight { get; set; }
        public int? SlopedWallTopLength { get; set; }
        public int? SlopedWallTopWidth { get; set; }
        public int? SlopedWallHeight { get; set; }
        public int? SlopedWallSlopeOfWall { get; set; }
    }
}
