using System;
using System.Collections.Generic;

namespace Agri.Models.Configuration
{
    public class CropType
    {
        public CropType()
        {
            Crops = new List<Crop>();
            PrevCropTypes = new List<PreviousCropType>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CoverCrop { get; set; }
        public bool CrudeProteinRequired { get; set; }
        public bool CustomCrop { get; set; }
        public bool ModifyNitrogen { get; set; }

        public List<Crop> Crops { get; set; }
        public List<PreviousCropType> PrevCropTypes { get; set; }
    }
}