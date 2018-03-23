﻿using System.Collections.Generic;

namespace Common
{
    public class ProductVersionInfo
    {
        public ProductVersionInfo()
        {
            this.ApplicationVersions = new List<ApplicationVersionInfo>();
            this.DatabaseVersions = new List<DatabaseVersionInfo>();
        }

        public List<ApplicationVersionInfo> ApplicationVersions { get; set; }
        public List<DatabaseVersionInfo> DatabaseVersions { get; set; }
    }
}
