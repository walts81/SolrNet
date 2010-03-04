#region license

// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Xml;
using MbUnit.Framework;
using SolrNet.Mapping;
using SolrNet.Mapping.Validation;
using SolrNet.Mapping.Validation.Rules;
using SolrNet.Schema;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests {
    [TestFixture]
    public class MappingValidationManagerTests {
        [Test]
        public void FindValidationRulesInCollectionOfTypes() {
            var mappingManager = new MappingManager();
            var solrSchemaParser = new SolrSchemaParser();
            var mappingValidationManager = new MappingValidationManager(mappingManager, solrSchemaParser);

            var types = new[] { typeof(DummyValidationRuleError), typeof(String), typeof(DummyValidationRuleWarning) };

            var typesImplementingIValidationRule = mappingValidationManager.GetValidationRules(types);

            Assert.AreEqual(2, typesImplementingIValidationRule.Count);
        }

        [Test]
        public void ValidatingRuleSetReturnsValidationResults() {
            var mappingManager = new MappingManager();
            var solrSchemaParser = new SolrSchemaParser();
            var mappingValidationManager = new MappingValidationManager(mappingManager, solrSchemaParser);

            var types = new[] { typeof(DummyValidationRuleError)};
            var validationResults = mappingValidationManager.Validate<SchemaMappingTestDocument>(new XmlDocument(), types);
            
            Assert.AreEqual(1, validationResults.Count);
        }
    }

    public class SchemaMappingTestDocument
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public string FieldNotSolrSchema { get; set; }
    }

    public class DummyValidationRuleError : IValidationRule
    {
        public IEnumerable<MappingValidationItem> Validate<T>(SolrSchema solrSchema, IReadOnlyMappingManager mappingManager)
        {
            return new MappingValidationItem[] {new MappingValidationError("Dummy error validation rule")};
        }
    }

    public class DummyValidationRuleWarning : IValidationRule
    {
        public IEnumerable<MappingValidationItem> Validate<T>(SolrSchema solrSchema, IReadOnlyMappingManager mappingManager)
        {
            return new MappingValidationItem[] { new MappingValidationError("Dummy warning validation rule") };
        }
    }
}