﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class AnimalSubType : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(16,4)")]
        public decimal? LiquidPerGalPerAnimalPerDay { get; set; }

        [Column(TypeName = "decimal(16,4)")]
        public decimal? SolidPerGalPerAnimalPerDay { get; set; }

        [Column(TypeName = "decimal(16,4)")]
        public decimal? SolidPerPoundPerAnimalPerDay { get; set; }

        public decimal SolidLiquidSeparationPercentage { get; set; }
        public decimal WashWater { get; set; }
        public decimal MilkProduction { get; set; }
        public int AnimalId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Animal Animal { get; set; }

        public int SortOrder { get; set; }
    }
}