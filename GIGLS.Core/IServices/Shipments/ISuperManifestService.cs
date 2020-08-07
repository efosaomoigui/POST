﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface ISuperManifestService : IServiceDependencyMarker
    {
        Task<string> GenerateSuperManifestCode();
    }
}