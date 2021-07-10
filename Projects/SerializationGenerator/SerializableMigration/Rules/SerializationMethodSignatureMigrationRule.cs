/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2021 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: SerializationMethodSignatureMigrationRule.cs                    *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using SerializationGenerator;

namespace SerializableMigration
{
    public class SerializationMethodSignatureMigrationRule : ISerializableMigrationRule
    {
        public string RuleName => nameof(SerializationMethodSignatureMigrationRule);

        public bool GenerateRuleState(
            Compilation compilation,
            ISymbol symbol,
            ImmutableArray<AttributeData> attributes,
            ImmutableArray<INamedTypeSymbol> serializableTypes,
            ISymbol? parentSymbol,
            out string[] ruleArguments
        )
        {
            if ((symbol as ITypeSymbol)?.HasPublicSerializeMethod(compilation, serializableTypes) != true)
            {
                ruleArguments = null;
                return false;
            }

            if (symbol is not INamedTypeSymbol namedTypeSymbol ||
                !namedTypeSymbol.HasGenericReaderCtor(compilation, parentSymbol, out var requiresParent))
            {
                ruleArguments = null;
                return false;
            }

            ruleArguments = requiresParent ? new[] { "DeserializationRequiresParent" } : Array.Empty<string>();
            return true;
        }

        public void GenerateDeserializationMethod(StringBuilder source, string indent, SerializableProperty property)
        {
            const string expectedRule = nameof(SerializationMethodSignatureMigrationRule);
            var ruleName = property.Rule;
            if (expectedRule != ruleName)
            {
                throw new ArgumentException($"Invalid rule applied to property {ruleName}. Expecting {expectedRule}, but received {ruleName}.");
            }

            var propertyName = property.Name;
            var argument = property.RuleArguments.Length >= 1 &&
                           property.RuleArguments[0] == "DeserializationRequiresParent" ? ", this" : "";

            source.AppendLine($"{indent}{propertyName} = new {property.Type}(reader{argument});");
        }

        public void GenerateSerializationMethod(StringBuilder source, string indent, SerializableProperty property)
        {
            const string expectedRule = nameof(SerializationMethodSignatureMigrationRule);
            var ruleName = property.Rule;
            if (expectedRule != ruleName)
            {
                throw new ArgumentException($"Invalid rule applied to property {ruleName}. Expecting {expectedRule}, but received {ruleName}.");
            }

            var propertyName = property.Name;
            source.AppendLine($"{indent}{propertyName}.Serialize(writer);");
        }
    }
}
