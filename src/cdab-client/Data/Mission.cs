/*
cdab-client is part of the software suite used to run Test Scenarios 
for bechmarking various Copernicus Data Provider targets.
    
    Copyright (C) 2020 Terradue Ltd, www.terradue.com
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published
    by the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using cdabtesttools.Config;
using cdabtesttools.Target;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Terradue.GeoJson.Geometry;
using Terradue.Metadata.EarthObservation.OpenSearch.Extensions;
using Terradue.OpenSearch.Result;

namespace cdabtesttools.Data
{

    public class Mission
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Random rnd = new Random(DateTime.Now.Millisecond);
        private static WKTWriter wktWriter = new WKTWriter();
        private static WKTReader wktreader = new WKTReader();
        private readonly LabelString psn;
        private static Regex dateRegex = new Regex(@"_(?'yyyy'\d{4})(?'MM'\d{2})(?'dd'\d{2})T(?'HH'\d{2})(?'mm'\d{2})(?'ss'\d{2})_");

        public string Name { get; set; }

        [ComplexChoice]
        public TimeRange Lifetime { get; set; }


        [SimpleChoice]
        public StringListChoice PlatformIdentifiers { get; set; }

        [SimpleChoice]
        public StringListChoice ProductTypes { get; set; }

        [SimpleChoice]
        public StringListChoice Polarisations { get; set; }

        [SimpleChoice]
        public StringListChoice SensorModes { get; set; }

        [SimpleChoice]
        public StringListChoice StorageStatus { get; set; }

        [SimpleChoice]
        public StringListChoice Instruments { get; set; }

        [SimpleChoice]
        public StringListChoice Timeliness { get; set; }

        [SimpleChoice]
        public StringListChoice ProductLevels { get; set; }

        [SimpleChoice]
        public ItemNumberRange RelativeOrbit { get; set; }

        [SimpleChoice]
        public ItemNumberRange CloudCover { get; set; }

        [ComplexChoice]
        [JsonIgnore]
        public GeometryFilterCollection Geometries { get; set; }

        public StringListChoice ArchivingStatus { get; set; }

        public LabelString MissionName => psn;

        [JsonIgnore]
        [SimpleChoice]
        public ItemNumberRange Count { get; set; }

        public Mission(string name, LabelString psn)
        {
            this.Name = name;
            this.psn = psn;
        }

        public static IEnumerable<Mission> GenerateExistingDataDictionary(TargetSiteWrapper target)
        {
            List<Mission> missions = new List<Mission>();
            IEnumerable<Feature> features = ShapeFileLoader.Load(Configuration.Current.Global.CountryShapefilePath);

            // Sentinel1
            Mission s1Mission = new Mission("Sentinel-1", new LabelString("Sentinel-1", "Sentinel-1", GetIdentifierValidator(new Regex(@"^S1.*"))));
            s1Mission.Lifetime = new TimeRange("{http://a9.com/-/opensearch/extensions/time/1.0/}start", "{http://a9.com/-/opensearch/extensions/time/1.0/}end", new DateTime(2014, 04, 03), DateTime.UtcNow);
            if (target != null && target.Type == TargetType.ASF)
            {
                s1Mission.PlatformIdentifiers = new StringListChoice("platformSerialIdentifier", "{http://a9.com/-/opensearch/extensions/eo/1.0/}platformSerialIdentifier",
                    new LabelString[] {
                        new LabelString("A", "A", GetIdentifierValidator(new Regex(@"^S1A.*"))),
                        new LabelString("B", "B", GetIdentifierValidator(new Regex(@"^S1B.*")))
                    });
            }
            else
            {
                if (target.Type != TargetType.THIRDPARTY)
                {
                    s1Mission.PlatformIdentifiers = new StringListChoice("platformSerialIdentifier", "{http://a9.com/-/opensearch/extensions/eo/1.0/}platformSerialIdentifier",
                        new LabelString[] {
                            new LabelString("2014-016A", "A", GetIdentifierValidator(new Regex(@"^S1A.*"))),
                            new LabelString("2016-025A", "B", GetIdentifierValidator(new Regex(@"^S1B.*")))
                        });
                }
            }
            s1Mission.ProductTypes = new StringListChoice("productType", "{http://a9.com/-/opensearch/extensions/eo/1.0/}productType",
                new LabelString[] {
                    new LabelString("RAW", "Level-0 SAR raw data (RAW)", GetIdentifierValidator(new Regex(@"^S1.*_RAW_.*"))),
                    new LabelString("SLC", "Level-1 Single Look Complex (SLC)", GetIdentifierValidator(new Regex(@"^S1.*_SLC_.*"))),
                    new LabelString("GRD", "Level-1 Ground Range Detected (GRD)", GetIdentifierValidator(new Regex(@"^S1.*_GRD._.*"))),
                    new LabelString("OCN", "Level-2 Ocean (OCN)", GetIdentifierValidator(new Regex(@"^S1.*_OCN_.*")))
                });
            s1Mission.Polarisations = new StringListChoice("polarizationChannels", "{http://a9.com/-/opensearch/extensions/eo/1.0/}polarizationChannels",
                new LabelString[] {
                    new LabelString("HH", "HH", GetIdentifierValidator(new Regex(@"^S1.*SH_.*"))),
                    new LabelString("VV", "VV", GetIdentifierValidator(new Regex(@"^S1.*SV_.*"))),
                    new LabelString("HV", "HV", GetIdentifierValidator(new Regex(@"^S1.*(HV_|DH_).*"))),
                    new LabelString("VH", "VH", GetIdentifierValidator(new Regex(@"^S1.*(VH_|DV_).*"))),
                    new LabelString("HH+HV", "HH+HV", GetIdentifierValidator(new Regex(@"^S1.*DH_.*"))),
                    new LabelString("VV+VH", "VV+VH", GetIdentifierValidator(new Regex(@"^S1.*DV_.*")))
                });
            s1Mission.SensorModes = new StringListChoice("sensorMode", "{http://a9.com/-/opensearch/extensions/eo/1.0/}sensorMode",
                new LabelString[] {
                    new LabelString("SM", "Stripmap", GetIdentifierValidator(new Regex(@"^S1.*_S._.*"))),
                    new LabelString("IW", "Interferometric Wide swath", GetIdentifierValidator(new Regex(@"^S1.*_IW_.*"))),
                    new LabelString("EW", "Extra Wide swath", GetIdentifierValidator(new Regex(@"^S1.*_EW_.*"))),
                    new LabelString("WV", "Wave", GetIdentifierValidator(new Regex(@"^S1.*_WV_.*"))),
                });
            if (!(target.Wrapper is Terradue.OpenSearch.DataHub.Dias.WekeoDiasWrapper)) {
                // Disabled for WEkEO because it leads to query timeouts
                s1Mission.RelativeOrbit = new ItemNumberRange("track", "{http://a9.com/-/opensearch/extensions/eo/1.0/}track",
                    1, 175, 1, "[{0},{1}]", new Regex(@"\[([0-9]+(\\.[0-9]+)?),([0-9]+(\\.[0-9]+)?)\]"), "Track", GetTrackValidator, null
                );
            }

            // s1Mission.Timeliness = new StringListChoice("timeliness", "{http://a9.com/-/opensearch/extensions/eo/1.0/}timeliness",
            //     new LabelString[] {
            //         new LabelString("Fast-1h", "Fast 1h", null, GetMultiFiltersConditioner("productType", new string[]{"SLC", "GRD"})),
            //         new LabelString("Fast-1.5h", "Fast 1.5h", null, GetMultiFiltersConditioner("productType", new string[]{"OCN"})),
            //         new LabelString("Fast-24h", "Fast 24h", null, GetMultiFiltersConditioner("productType", new string[]{"SLC", "GRD"})),
            //     });

            if (!(target.Wrapper is Terradue.OpenSearch.DataHub.Dias.WekeoDiasWrapper)) {
                s1Mission.ArchivingStatus = new StringListChoice("archiveStatus", "{http://a9.com/-/opensearch/extensions/eo/1.0/}statusSubType",
                    new LabelString[] {
                        new LabelString("Online", "Online", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.ONLINE), null),
                        new LabelString("Offline", "Offline", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.OFFLINE), null),
                    });
            }

            s1Mission.Count = new ItemNumberRange("count", "{http://a9.com/-/spec/opensearch/1.1/}count", 1, 50, 1, "{0}",
                new Regex(@"([0-9]+(\\.[0-9]+)?)"), "Count", null, GetCountValidator);

            s1Mission.Geometries = new GeometryFilterCollection("geom", "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry", features);

            if (target.Type != TargetType.USGS) missions.Add(s1Mission);

            // Sentinel2
            Mission s2Mission;

            if (target.Type == TargetType.USGS) {
                s2Mission = new Mission("Sentinel-2", new LabelString("Sentinel-2", "Sentinel-2", GetIdentifierValidator(new Regex(@"^L1C_.*"))));
                s2Mission.ProductTypes = new StringListChoice("productType", "{http://a9.com/-/opensearch/extensions/eo/1.0/}productType",
                    new LabelString[] {
                        new LabelString("sentinel_2a", "Level-1C", GetIdentifierValidator(new Regex(@"^L1C_.*"))),
                    });
                s2Mission.Lifetime = new TimeRange("{http://a9.com/-/opensearch/extensions/time/1.0/}start", "{http://a9.com/-/opensearch/extensions/time/1.0/}end", new DateTime(2020, 01, 01), DateTime.UtcNow);
                s2Mission.Geometries = new GeometryFilterCollection("geom", "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry", features);
                s2Mission.Count = new ItemNumberRange("count", "{http://a9.com/-/spec/opensearch/1.1/}count", 1, 10, 1, "{0}",
                    new Regex(@"([0-9]+(\\.[0-9]+)?)"), "Count", null, GetCountValidator);

            } else {
                s2Mission = new Mission("Sentinel-2", new LabelString("Sentinel-2", "Sentinel-2", GetIdentifierValidator(new Regex(@"^S2.*"))));
                s2Mission.Lifetime = new TimeRange("{http://a9.com/-/opensearch/extensions/time/1.0/}start", "{http://a9.com/-/opensearch/extensions/time/1.0/}end", new DateTime(2015, 07, 01), DateTime.UtcNow);
                if (target.Type != TargetType.THIRDPARTY) {
                    s2Mission.PlatformIdentifiers = new StringListChoice("platformSerialIdentifier", "{http://a9.com/-/opensearch/extensions/eo/1.0/}platformSerialIdentifier",
                        new LabelString[] {
                            new LabelString("2015-028A", "A", GetIdentifierValidator(new Regex(@"^S2A.*"))),
                            new LabelString("2017-013A", "B", GetIdentifierValidator(new Regex(@"^S2B.*")))
                        });
                }
                s2Mission.ProductTypes = new StringListChoice("productType", "{http://a9.com/-/opensearch/extensions/eo/1.0/}productType",
                    new LabelString[] {
                        new LabelString("S2MSI1C", "Level-1C", GetIdentifierValidator(new Regex(@"^S2.*_(MSI)?L1C_.*"))),
                        new LabelString("S2MSI2A", "Level-2A", GetIdentifierValidator(new Regex(@"^S2.*_(MSI)?L2A_.*"))),
                    });
                if (target.Type != TargetType.THIRDPARTY && !(target.Wrapper is Terradue.OpenSearch.DataHub.Dias.WekeoDiasWrapper)) {
                    s2Mission.RelativeOrbit = new ItemNumberRange("track", "{http://a9.com/-/opensearch/extensions/eo/1.0/}track", 1, 143, 1, "[{0},{1}]",
                        new Regex(@"\[([0-9]+(\\.[0-9]+)?),([0-9]+(\\.[0-9]+)?)\]"), "Track", GetTrackValidator, null);
                }

                s2Mission.Count = new ItemNumberRange("count", "{http://a9.com/-/spec/opensearch/1.1/}count", 1, 50, 1, "{0}",
                    new Regex(@"([0-9]+(\\.[0-9]+)?)"), "Count", null, GetCountValidator);

                // s2Mission.Timeliness = new StringListChoice("timeliness", "{http://a9.com/-/opensearch/extensions/eo/1.0/}timeliness",
                //     new LabelString[] {
                //         new LabelString("Fast-1.5h", "Fast 1.5h", null, GetMultiFiltersConditioner("productType", new string[]{"S2MSI1C"})),
                //     });

                if (target.Type != TargetType.THIRDPARTY) {
                    s2Mission.ArchivingStatus = new StringListChoice("archiveStatus", "{http://a9.com/-/opensearch/extensions/eo/1.0/}statusSubType",
                        new LabelString[] {
                            new LabelString("Online", "Online", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.ONLINE), null),
                            new LabelString("Offline", "Offline", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.OFFLINE), null),
                        });
                }

                s2Mission.Geometries = new GeometryFilterCollection("geom", "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry", features);
            }

            missions.Add(s2Mission);

            // Sentinel3
            Mission s3Mission = new Mission("Sentinel-3", new LabelString("Sentinel-3", "Sentinel-3", GetIdentifierValidator(new Regex(@"^S3.*"))));
            s3Mission.Lifetime = new TimeRange("{http://a9.com/-/opensearch/extensions/time/1.0/}start", "{http://a9.com/-/opensearch/extensions/time/1.0/}end", new DateTime(2016, 03, 01), DateTime.UtcNow);
            if (target.Type != TargetType.THIRDPARTY)
            {
                s3Mission.PlatformIdentifiers = new StringListChoice("platformSerialIdentifier", "{http://a9.com/-/opensearch/extensions/eo/1.0/}platformSerialIdentifier",
                    new LabelString[] {
                        new LabelString("2016-011A", "A", GetIdentifierValidator(new Regex(@"^S3A.*"))),
                        new LabelString("2018-039A", "B", GetIdentifierValidator(new Regex(@"^S3B.*")))
                    });
            }
            s3Mission.ProductTypes = new StringListChoice("productType", "{http://a9.com/-/opensearch/extensions/eo/1.0/}productType",
                new LabelString[] {
                    new LabelString("OL_1_EFR___", "OLCI Level-1B EO FR", GetIdentifierValidator(new Regex(@"^S3.*_OL_1_EFR___.*"))),
                    new LabelString("OL_1_ERR___", "OLCI Level-1B EO RR", GetIdentifierValidator(new Regex(@"^S3.*_OL_1_ERR___.*"))),
                    new LabelString("OL_2_LRR___", "OLCI Level-2 Land RR", GetIdentifierValidator(new Regex(@"^S3.*OL_2_LRR___.*"))),
                    new LabelString("OL_2_LFR___", "OLCI Level-2 Land FR", GetIdentifierValidator(new Regex(@"^S3.*OL_2_LFR___.*"))),
                    new LabelString("SL_1_RBT___", "SLSTR Level-1B RBT", GetIdentifierValidator(new Regex(@"^S3.*SL_1_RBT___.*"))),
                    new LabelString("SL_2_LST___", "SLSTR Level-2 Land Surface Temp", GetIdentifierValidator(new Regex(@"^S3.*SL_2_LST___.*"))),
                    new LabelString("SR_1_SRA___", "Altimetry Level-1 SRA", GetIdentifierValidator(new Regex(@"^S3.*SR_1_SRA___.*"))),
                    new LabelString("SR_1_SRA_A_", "Altimetry Level-1 SRA_A", GetIdentifierValidator(new Regex(@"^S3.*SR_1_SRA_A_.*"))),
                    new LabelString("SR_1_SRA_BS", "Altimetry Level-1 SRA_BS", GetIdentifierValidator(new Regex(@"^S3.*SR_1_SRA_BS.*"))),
                    new LabelString("SR_2_LAN___", "Altimetry Level-2 Land", GetIdentifierValidator(new Regex(@"^S3.*SR_2_LAN___.*"))),
                    // new LabelString("SR_2_WAT___", "Altimetry Level-2 Water", GetIdentifierValidator(new Regex(@"^S3.*SR_2_WAT___.*"))),
                    new LabelString("SY_2_SYN___", "Synergy Level-2 Surface Reflectance", GetIdentifierValidator(new Regex(@"^S3.*SY_2_SYN___.*"))),
                    // new LabelString("SY_2_VGP___", "Synergy Level-2 Vegetation",  GetIdentifierValidator(new Regex(@"^S3.*SY_2_VGP___.*"))),
                });
            if (!(target.Wrapper is Terradue.OpenSearch.DataHub.Dias.WekeoDiasWrapper)) {
                s3Mission.RelativeOrbit = new ItemNumberRange("track", "{http://a9.com/-/opensearch/extensions/eo/1.0/}track", 1, 385, 1, "[{0},{1}]",
                    new Regex(@"\[([0-9]+(\\.[0-9]+)?),([0-9]+(\\.[0-9]+)?)\]"), "Track", GetTrackValidator, null);
            }

            s3Mission.Count = new ItemNumberRange("count", "{http://a9.com/-/spec/opensearch/1.1/}count", 1, 50, 1, "{0}",
                new Regex(@"([0-9]+(\\.[0-9]+)?)"), "Count", null, GetCountValidator);

            s3Mission.Geometries = new GeometryFilterCollection("geom", "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry", features);

            s3Mission.Timeliness = new StringListChoice("timeliness", "{http://a9.com/-/opensearch/extensions/eo/1.0/}timeliness",
                new LabelString[] {
                    new LabelString("NRT", "Near Real Time", GetTimelinessValidator("NRT")),
                    new LabelString("STC", "Short Time Critical", GetTimelinessValidator("STC")),
                    new LabelString("NTC", "Non Time Critical", GetTimelinessValidator("NTC")),
                    new LabelString("Fast-1.5h", "Fast 1.5h", null, GetMultiFiltersConditioner("productType",
                    new string[]{"OL_1_EFR___", "OL_1_ERR___", "OL_2_LRR___", "OL_2_LFR___",
                                 "SL_1_RBT___", "SL_2_LST___",
                                 "SR_1_SRA___", "SR_1_SRA_A_", "SR_1_SRA_BS",
                                 "SR_2_LAN___", "SR_2_WAT___"
                                 })),
                });

            s3Mission.ArchivingStatus = new StringListChoice("archiveStatus", "{http://a9.com/-/opensearch/extensions/eo/1.0/}statusSubType",
                new LabelString[] {
                    new LabelString("Online", "Online", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.ONLINE), null),
                    new LabelString("Offline", "Offline", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.OFFLINE), null),
                });

            if (target.Type != TargetType.USGS) missions.Add(s3Mission);

            Mission s5pMission = new Mission("Sentinel-5P", new LabelString("Sentinel-5 Precursor", "Sentinel-5 Precursor", GetIdentifierValidator(new Regex(@"^S5P.*"))));
            s5pMission.Lifetime = new TimeRange("{http://a9.com/-/opensearch/extensions/time/1.0/}start", "{http://a9.com/-/opensearch/extensions/time/1.0/}end", new DateTime(2015, 07, 01), DateTime.UtcNow);
            s5pMission.ProductLevels = new StringListChoice("productType", "{http://a9.com/-/opensearch/extensions/eo/1.0/}processingLevel",
                new LabelString[] {
                    new LabelString("L1B", "Level-1B", GetIdentifierValidator(new Regex(@"^S5P.*_L1B_.*"))),
                    new LabelString("L2", "Level-2", GetIdentifierValidator(new Regex(@"^S5P.*_L2__.*"))),
                });
            s5pMission.ArchivingStatus = new StringListChoice("archiveStatus", "{http://a9.com/-/opensearch/extensions/eo/1.0/}statusSubType",
                new LabelString[] {
                    new LabelString("Online", "Online", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.ONLINE), null),
                    new LabelString("Offline", "Offline", GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.OFFLINE), null),
                });

            if (true) {
                s3Mission.Timeliness = new StringListChoice("timeliness", "{http://a9.com/-/opensearch/extensions/eo/1.0/}timeliness",
                    new LabelString[] {
                        new LabelString("Near Real Time", "Near Real Time", GetTimelinessValidator("NRT")),
                        new LabelString("Offline", "Offline", GetTimelinessValidator("OFFL")),
                });
            }

            s5pMission.Geometries = new GeometryFilterCollection("geom", "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry", features);

            if (target.Type != TargetType.USGS) missions.Add(s5pMission);

            return missions;
        }

        internal Func<IOpenSearchResultItem, bool> GetItemValidator(OpenSearchParameter parameter)
        {
            IEnumerable<System.Reflection.PropertyInfo> props = this.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(SimpleChoiceAttribute)) || Attribute.IsDefined(prop, typeof(ComplexChoiceAttribute)));

            foreach (var prop in props)
            {
                try
                {
                    IMissionFilter filter = prop.GetValue(this) as IMissionFilter;
                    if (filter.FullName != parameter.FullName) continue;
                    return filter.GetItemValidator(parameter);
                }
                catch
                {
                    continue;
                }
            }

            return null;
        }

        private static Func<NameValueCollection, bool> GetMultiFiltersConditioner(string key, string[] values)
        {
            return (NameValueCollection nvc)
                     =>
             {
                 return values.Any(v => v == nvc[key]);
             };
        }

        private static Func<IOpenSearchResultItem, bool> GetTrackValidator(double[] arg)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                double value = 0;
                double.TryParse(item.FindTrack(), out value);
                return value >= arg[0] && value <= arg[1];
            };
        }

        private static Func<IOpenSearchResultItem, bool> GetCloudCoverValidator(double[] arg)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                double value = item.FindCloudCoverPercentage();
                return value >= arg[0] && value <= arg[1];
            };
        }

        public static Func<IOpenSearchResultItem, bool> GetGeometryValidator(Feature feature)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                var geom = item.FindGeometry();
                if (geom == null)
                {
                    log.WarnFormat("No geometry found for item {0}", item.Identifier);
                    return false;
                }
                Geometry geom2 = wktreader.Read(geom.ToWkt());

                // Invert coordinates if necessary
                if (AdjustGeometry(geom2))
                {
                    log.DebugFormat("Coordinates adjusted for item {0}", item.Identifier);
                }
                
                return feature.Geometry.Buffer(0.5).Intersects(geom2);
            };
        }


        public static Func<IOpenSearchResultItem, bool> GetGeometryValidator(Geometry feature)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                var geom = item.FindGeometry();
                if (geom == null)
                {
                    log.WarnFormat("No geometry found for item {0}", item.Identifier);
                    return false;
                }
                return feature.Buffer(0.1).Intersects(wktreader.Read(geom.ToWkt()));
            };
        }

        public static Func<IOpenSearchResultItem, bool> GetIngestionDateValidator(DateTime date)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                return item.PublishDate == DateTime.MinValue || item.PublishDate > date;   // ??? publish date corresponds to CreationDate (>= IngestionDate)
            };
        }

        public static Func<IOpenSearchResultItem, bool> GetRangeValidator(ItemNumberRange range)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                double value = 0;
                double.TryParse(item.FindTrack(), out value);
                return value >= range.Min && value <= range.Min;
            };
        }

        public static Func<IOpenSearchResultItem, bool> GetTimelinessValidator(string tl)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                string timeliness = "NTC";
                try
                {
                    timeliness = item.GetEarthObservationProfile().result.Eop21EarthObservationResult.product[0].ProductInformation.timeliness.Text;
                }
                catch
                {
                    return tl == "NTC";
                }
                return timeliness.StartsWith(tl);
            };
        }

        public static Func<IOpenSearchResultItem, bool> GetArchivingStatusValidator(Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType st)
        {
            return (IOpenSearchResultItem item)
                    =>
            {
                try
                {
                    Terradue.ServiceModel.Ogc.Eop21.EarthObservationType eop = (Terradue.ServiceModel.Ogc.Eop21.EarthObservationType)item.GetEarthObservationProfile();
                    if (eop.EopMetaDataProperty.EarthObservationMetaData.statusSubTypeSpecified)
                        return eop.EopMetaDataProperty.EarthObservationMetaData.statusSubType == st;
                }
                catch
                {
                }
                return st == Terradue.ServiceModel.Ogc.Eop21.StatusSubTypeValueEnumerationType.ONLINE;
            };
        }

        public static Func<IOpenSearchResultCollection, bool> GetCountValidator(double[] count)
        {
            return (IOpenSearchResultCollection results)
                    =>
            {
                if (results.TotalResults >= count[0] || results.TotalResults < 0)
                    return results.Count == count[0];
                else
                    return results.Count == results.TotalResults;
            };
        }

        public static Func<IOpenSearchResultItem, bool> GetIdentifierValidator(Regex regex)
        {
            return (IOpenSearchResultItem item) =>
            {
                return regex.Match(item.Identifier).Success;
            };
        }

        public static IEnumerable<FiltersDefinition> ShuffleSimpleRandomFiltersCombination(IEnumerable<Mission> missions, Dictionary<string, CatalogueSetConfiguration> catConfig, int num, Func<ItemNumberRange, string> rangeReformatter = null)
        {
            List<FiltersDefinition> _randomCombinations = new List<FiltersDefinition>();

            var baselines = catConfig.Select(cc => DataHelper.ResolveBaseline(cc.Key, cc.Value));

            for (int i = 0; i < num; i++)
            {
                // Shuffle in missions present in the baselines...
                var matchingMissions = missions.Where(m => baselines.Any(b =>
                        // and in the collections...
                        b.Collections.Any(c =>
                            // with the platform parameter equals to the mission name
                            c.Value.Parameters.Any(p => p.FullName == "{http://a9.com/-/opensearch/extensions/eo/1.0/}platform"
                                && p.Value == m.MissionName.Value))));

                var _mission = matchingMissions.ToArray()[rnd.Next(0, matchingMissions.Count())];
                var collections = baselines.SelectMany(b => b.Collections.Where(c =>
                            c.Value.Parameters.Any(p => p.FullName == "{http://a9.com/-/opensearch/extensions/eo/1.0/}platform"
                                && p.Value == _mission.MissionName.Value))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var fd = Mission.ShuffleSimpleRandomFiltersCombination(_mission, collections, string.Format("Simple-Random-{0}", i), rangeReformatter, Configuration.Current.Global.SimpleFilterLimit);
                fd.AddFilter("archiveStatus", "{http://a9.com/-/opensearch/extensions/eo/1.0/}statusSubType", "online", "Online", null, null);
                _randomCombinations.Add(fd);
            }

            return _randomCombinations;
        }

        internal static IEnumerable<FiltersDefinition> ShuffleComplexRandomFiltersCombination(IEnumerable<Mission> missions, Dictionary<string, CatalogueSetConfiguration> catConfig, int num)
        {
            List<FiltersDefinition> _randomCombinations = new List<FiltersDefinition>();

            var baselines = catConfig.Select(cc => DataHelper.ResolveBaseline(cc.Key, cc.Value));

            for (int i = 0; i < num; i++)
            {
                try
                {
                    // Shuffle in missions present in the baselines...
                    var matchingMissions = missions.Where(m => baselines.Any(b =>
                            // and in the collections...
                            b.Collections.Any(c =>
                                // with the platform parameter equals to the mission name
                                c.Value.Parameters.Any(p => p.FullName == "{http://a9.com/-/opensearch/extensions/eo/1.0/}platform"
                                    && p.Value == m.MissionName.Value))));
                    var _mission = matchingMissions.ToArray()[rnd.Next(0, matchingMissions.Count())];
                    var collections = baselines.SelectMany(b => b.Collections.Where(c =>
                               c.Value.Parameters.Any(p => p.FullName == "{http://a9.com/-/opensearch/extensions/eo/1.0/}platform"
                                   && p.Value == _mission.MissionName.Value))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    var fd = Mission.ShuffleComplexRandomFiltersCombination(_mission, collections, string.Format("Complex-Random-{0}", i), Configuration.Current.Global.ComplexFilterLimit);
                    fd.AddFilter("archiveStatus", "{http://a9.com/-/opensearch/extensions/eo/1.0/}statusSubType", "online", "Online", null, null);
                    _randomCombinations.Add(fd);

                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error shuffling filters: {0}", e.Message);
                    log.Debug(e.StackTrace);
                }
            }

            return _randomCombinations;
        }

        private static FiltersDefinition ShuffleSimpleRandomFiltersCombination(Mission mission, Dictionary<string, DataCollectionDefinition> collections, string FilterSetkey, Func<ItemNumberRange, string> rangeReformatter = null, int limit = 3)
        {
            FiltersDefinition _filtersDefinition = new FiltersDefinition(FilterSetkey);
            var collection = collections.ToArray()[rnd.Next(0, collections.Count())].Value;
            _filtersDefinition.AddFilters(collection.Parameters, mission);

            IEnumerable<System.Reflection.PropertyInfo> props = mission.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(SimpleChoiceAttribute)));

            foreach (var prop in props)
            {
                if (_filtersDefinition.GetFilters().Count() >= limit) continue;
                if (rnd.Next() % 2 == 0) continue;
                var choice = prop.GetValue(mission);
                if (choice == null) continue;
                if (_filtersDefinition.Filters.Any(fd => fd.FullName == ((IMissionFilter)choice).FullName)) continue;
                if (choice is StringListChoice)
                {
                    var sl = choice as StringListChoice;
                    var value = sl.LabelStrings.ToArray()[rnd.Next(0, sl.LabelStrings.Count())];
                    _filtersDefinition.AddFilter(sl.Key, sl.FullName, value.Value, value.Label, value.Validator, null);
                }
                if (choice is ItemNumberRange)
                {
                    var nb = choice as ItemNumberRange;
                    int randomMin = rnd.Next((int)(nb.Min / nb.Step), (int)(nb.Max / nb.Step));
                    decimal min = new decimal(randomMin * nb.Step);
                    int randomMax = rnd.Next((int)((int)min / nb.Step), (int)(nb.Max / nb.Step));
                    decimal max = new decimal(randomMax * nb.Step);
                    var value = new double[2] { (double)min, (double)max };
                    var formatter = nb.Formatter;
                    if (rangeReformatter != null)
                    {
                        formatter = rangeReformatter.Invoke(nb);
                    }
                    Func<IOpenSearchResultItem, bool> ivalidator = null;
                    if (nb.ItemValueValidator != null)
                        ivalidator = nb.ItemValueValidator.Invoke(value);
                    Func<IOpenSearchResultCollection, bool> cvalidator = null;
                    if (nb.ResultsValidator != null)
                        cvalidator = nb.ResultsValidator.Invoke(value);
                    _filtersDefinition.AddFilter(nb.Key, nb.FullName, string.Format(formatter, value.Cast<object>().ToArray()), string.Format("{0} {1}", nb.Label, string.Format(formatter, value.Cast<object>().ToArray())), ivalidator, cvalidator);
                }

            }

            return _filtersDefinition;

        }

        private static FiltersDefinition ShuffleComplexRandomFiltersCombination(Mission mission, Dictionary<string, DataCollectionDefinition> collections, string filterSetKey, int limit = 2)
        {
            FiltersDefinition _filtersDefinition = ShuffleSimpleRandomFiltersCombination(mission, collections, filterSetKey, null, limit);

            IEnumerable<System.Reflection.PropertyInfo> props = mission.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(ComplexChoiceAttribute)));

            foreach (var prop in props)
            {
                var choice = prop.GetValue(mission);
                if (choice == null) continue;
                if (choice is TimeRange && prop.Name == "Lifetime")
                {
                    var tr = choice as TimeRange;
                    DateTime start = tr.Start;
                    DateTime stop = tr.Stop >= DateTime.UtcNow ? DateTime.UtcNow : tr.Stop;
                    if (rnd.Next() % 2 == 0)
                    {
                        TimeSpan timeSpan = stop - tr.Start;
                        TimeSpan newSpan = new TimeSpan(rnd.Next(0, (int)timeSpan.TotalHours), 0, 0);
                        start = tr.Start + newSpan;
                        _filtersDefinition.AddFilter("sensingStart", tr.ParameterStartFullName, start.ToString("s"), string.Format("From {0}", start.ToString("D")),
                            (IOpenSearchResultItem item) =>
                            {
                                var date = Terradue.Metadata.EarthObservation.OpenSearch.Extensions.EarthObservationOpenSearchResultExtensions.FindStartDate(item);
                                return date > start;
                            },
                            null
                            );
                    }
                    if (rnd.Next() % 2 == 0)
                    {
                        TimeSpan timeSpan = stop - start;
                        TimeSpan newSpan = new TimeSpan(rnd.Next(0, (int)timeSpan.TotalHours), 0, 0);
                        stop = start + newSpan;
                        _filtersDefinition.AddFilter("sensingEnd", tr.ParameterEndFullName, stop.ToString("s"), string.Format("To {0}", stop.ToString("D")),
                            (IOpenSearchResultItem item) =>
                            {
                                var date = Terradue.Metadata.EarthObservation.OpenSearch.Extensions.EarthObservationOpenSearchResultExtensions.FindEndDate(item);
                                if (date.Year == 9999)
                                {
                                    Match dateMatch = dateRegex.Match(item.Identifier);
                                    if (dateMatch.Success)
                                    {
                                        date = new DateTime(
                                            Int32.Parse(dateMatch.Groups["yyyy"].Value),
                                            Int32.Parse(dateMatch.Groups["MM"].Value),
                                            Int32.Parse(dateMatch.Groups["dd"].Value),
                                            Int32.Parse(dateMatch.Groups["HH"].Value),
                                            Int32.Parse(dateMatch.Groups["mm"].Value),
                                            Int32.Parse(dateMatch.Groups["ss"].Value),
                                            DateTimeKind.Utc
                                        );
                                    }
                                    Console.WriteLine("ITEM DATE: {0} {1:O}", item.Identifier, date);
                                }
                                return date < stop.AddDays(1);
                            },
                            null
                            );
                    }
                }
                if (choice is GeometryFilterCollection)
                {
                    var gfc = choice as GeometryFilterCollection;

                    var geomFilter = _filtersDefinition.Filters.FirstOrDefault(fd => fd.FullName == "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry");

                    Feature[] features = null;
                    if (geomFilter == null)
                    {
                        features = gfc.Features.ToArray();
                    }
                    else if (Configuration.Current.GeometryFilters != null && Configuration.Current.GeometryFilters.Keys.Contains(geomFilter.Label))
                    {
                        List<string> shapeList = Configuration.Current.GeometryFilters[geomFilter.Label];
                        features = gfc.Features.Where(f => shapeList.Contains(f.Attributes["NAME"])).ToArray();
                    }
                    if (features != null)
                    {
                        var feature = features[rnd.Next(0, features.Length)];
                        _filtersDefinition.AddFilter(gfc.Key, "{http://a9.com/-/opensearch/extensions/geo/1.0/}geometry", wktWriter.Write(feature.Geometry), string.Format("intersecting {0}", feature.Attributes["NAME"]),
                            GetGeometryValidator(feature),
                            null
                        );
                    }

                }
            }
            return _filtersDefinition;
        }

        private static bool AdjustGeometry(Geometry geometry)
        {
            // Invert coordinates if Y values are intended as X values (< -90 or > 90)
            bool inverted = false;
            foreach (var c in geometry.Coordinates)
            {
                if (c.Y < -90 || c.Y > 90 && !inverted) {
                    inverted = true;
                    break;
                }
            }
            if (inverted)
            {
                foreach (var c in geometry.Coordinates)
                {
                    double x = c.X;
                    c.X = c.Y;
                    c.Y = x;
                }
            }
            return inverted;
        }


    }
}
