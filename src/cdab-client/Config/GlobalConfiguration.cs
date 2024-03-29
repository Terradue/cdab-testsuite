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

namespace cdabtesttools.Config
{
    /// <summary>
    /// Data object class for the <em>global</em> node in the configuration YAML file.
    /// </summary>
    public class GlobalConfiguration
    {
        public int QueryTryNumber { get; set; }

        public string CountryShapefilePath { get; set; }

        public bool TestMode { get; set; }

        public int SimpleFilterLimit { get; set; }

        public int ComplexFilterLimit { get; set; }

        public GlobalConfiguration()
        {
            QueryTryNumber = 3;
            CountryShapefilePath = "App_Data/TM_WORLD_BORDERS-0.3/TM_WORLD_BORDERS-0.3";
            TestMode = false;
            SimpleFilterLimit = 3;
            ComplexFilterLimit = 2;
        }
    }
}