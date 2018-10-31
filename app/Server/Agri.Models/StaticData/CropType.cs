using System;
using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class CropType
    {
        public CropType()
        {
            Crops = new List<Crop>();
            PrevCropTypes = new List<PrevCropType>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CoverCrop { get; set; }
        public bool CrudeProteinRequired { get; set; }
        public bool CustomCrop { get; set; }
        public bool ModifyNitrogen { get; set; }

        public List<Crop> Crops { get; set; }
        public List<PrevCropType> PrevCropTypes { get; set; }
    }
}