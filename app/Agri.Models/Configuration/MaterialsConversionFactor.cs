﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class MaterialsConversionFactor : Versionable
    {
        [Key]
        public int Id { get; set; }

        public AnnualAmountUnits InputUnit { get; set; }

        public string InputUnitName { get; set; }
    }
}