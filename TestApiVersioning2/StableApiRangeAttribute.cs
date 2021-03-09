using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace TestApiVersioning2
{
    public class StableApiRangeAttribute : Attribute, IApiVersionProvider
    {
        private IReadOnlyList<ApiVersion> versions;

        ApiVersionProviderOptions options = ApiVersionProviderOptions.None;

        public StableApiRangeAttribute(int startVersion, int endVersion) 
        {
            versions = ComputeApiVersions(startVersion, endVersion);
        }

        public StableApiRangeAttribute(int startVersion)
        {
            int endVersion = (int)Enum.GetValues(typeof(StableApiVersions)).Cast<StableApiVersions>().Max();
            versions = ComputeApiVersions(startVersion, endVersion);
        }

        public ApiVersionProviderOptions Options => options;

        public IReadOnlyList<ApiVersion> Versions => versions;

        private IReadOnlyList<ApiVersion> ComputeApiVersions(int startVersion, int endVersion)
        {
            List<ApiVersion> versions = new List<ApiVersion>();
            for (int i = startVersion; i <= endVersion; i++)
            {
                versions.Add(new ApiVersion(2, i));
            }

            return versions;
        }
    }
}
