using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace TestApiVersioning2
{
    public class ApiRangeAttribute : Attribute, IApiVersionProvider
    {
        private static readonly DateTime[] StableVersions = new[]
        {
            new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)
        };

        private static readonly DateTime[] PreviewVersions = new[]
        {
            new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), new DateTime(2022, 1, 1)
        };

        private IReadOnlyList<ApiVersion> versions;

        ApiVersionProviderOptions options = ApiVersionProviderOptions.None;

        public ApiRangeAttribute(string startVersion, string endVersion) 
        {
            string[] splitStart = startVersion.Split("-");
            string[] endSplit = endVersion.Split("-");
            DateTime startDate = new DateTime(int.Parse(splitStart[0]), int.Parse(splitStart[1]), int.Parse(splitStart[2]));
            DateTime endDate = new DateTime(int.Parse(endSplit[0]), int.Parse(endSplit[1]), int.Parse(endSplit[2]));
            bool startPreview = splitStart.Length > 3 && splitStart[3] == "preview";
            bool endPreview = endSplit.Length > 3 && endSplit[3] == "preview";
            versions = ComputeApiVersions(startDate, startPreview, endDate, endPreview);
        }

        public ApiRangeAttribute(string startVersion)
        {
            string[] splitStart = startVersion.Split("-");
            DateTime startDate = new DateTime(int.Parse(splitStart[0]), int.Parse(splitStart[1]), int.Parse(splitStart[2]));
            bool startPreview = splitStart.Length > 3 && splitStart[3] == "preview";
            versions = ComputeApiVersions(startDate, startPreview, new DateTime(9999, 12, 30), true);
        }

        public ApiVersionProviderOptions Options => options;

        public IReadOnlyList<ApiVersion> Versions => versions;

        private IReadOnlyList<ApiVersion> ComputeApiVersions(DateTime startVersion, bool startPreview, DateTime endVersion, bool endPreview)
        {
            List<ApiVersion> versions = new List<ApiVersion>();
            foreach (DateTime d in StableVersions)
            {
                if (d.CompareTo(startVersion) >= 0 && d.CompareTo(endVersion) < 0)
                {
                    versions.Add(new ApiVersion(d));
                }
            }

            foreach (DateTime d in PreviewVersions)
            {
                if (d.CompareTo(startVersion) > 0 && d.CompareTo(endVersion) < 0)
                {
                    versions.Add(new ApiVersion(d, "preview"));
                }

                if (d.CompareTo(endVersion) == 0 && !endPreview)
                {
                    versions.Add(new ApiVersion(d, "preview"));
                }

                if (d.CompareTo(startVersion) == 0 && startPreview)
                {
                    versions.Add(new ApiVersion(d, "preview"));
                }
            }

            return versions;
        }
    }
}
