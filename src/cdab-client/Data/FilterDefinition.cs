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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Terradue.OpenSearch.Result;

namespace cdabtesttools.Data
{
    public class FilterDefinition
    {
        private readonly string key;
        private string fullName;
        private string value;
        private string label;
        private readonly System.Func<IOpenSearchResultCollection, bool> resultsValidator;
        private System.Func<IOpenSearchResultItem, bool> validator1;


        public string Key => key;

        public string FullName => fullName;

        public string Value
        {
            get
            {
                return value;
            }

            private set
            {
                this.value = value;
                if (value.StartsWith("[NOW"))
                {
                    if (value.EndsWith("M]"))
                    {
                        int months = int.Parse(value[4] + value.Substring(5, value.Length - 7));
                        this.value = DateTime.UtcNow.AddMonths(months).ToString("s");
                    }
                    if (value.EndsWith("D]"))
                    {
                        int months = int.Parse(value[4] + value.Substring(5, value.Length - 7));
                        this.value = DateTime.UtcNow.AddDays(months).ToString("s");
                    }
                }
            }
        }

        [JsonIgnore]
        public System.Func<IOpenSearchResultCollection, bool> ResultsValidator
        {
            get
            {
                return resultsValidator;
            }
        }

        [JsonIgnore]
        public System.Func<IOpenSearchResultItem, bool> ItemValidator
        {
            get
            {
                return validator1;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
        }

        public FilterDefinition(string key, string fullName, string value, string label, System.Func<IOpenSearchResultItem, bool> validator1, System.Func<IOpenSearchResultCollection, bool> resultsValidator)
        {
            if(string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(fullName);
            this.key = key;
            this.fullName = fullName;
            this.Value = value;
            this.label = label;
            this.validator1 = validator1;
            this.resultsValidator = resultsValidator;
        }
    }
}
