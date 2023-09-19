﻿namespace Agri.Models.Settings
{
    public class AppSettings
    {
        public string OtherCropId { get; set; }
        public string CommentLength { get; set; }
        public int NMPReleaseVersion { get; set; }
        public bool RefreshDatabase { get; set; }
        public bool LoadSeedData { get; set; }
        public int ExpectedSeedDataVersion { get; set; }
        public bool FLAG_BLUEBERRIES_WORKFLOW { get; set; }
    }
}