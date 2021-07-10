/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2021 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: EnumMigrationRule.cs                                            *
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
using System.Text;
using Microsoft.CodeAnalysis;
using SerializationGenerator;

namespace SerializableMigration
{
    public class EnumMigrationRule : ISerializableMigrationRule
    {
        public string RuleName => nameof(EnumMigrationRule);

        public bool GenerateRuleState(
            Compilation compilation,
            ISymbol symbol,
            ImmutableArray<AttributeData> attributes,
            ImmutableArray<INamedTypeSymbol> serializableTypes,
            ISymbol? parentSymbol,
            out string[] ruleArguments
        )
        {
            if (symbol is not ITypeSymbol typeSymbol || !typeSymbol.IsEnum())
            {
                ruleArguments = null;
                return false;
            }

            ruleArguments = Array.Empty<string>();
            return true;
        }

        public void GenerateDeserializationMethod(StringBuilder source, string indent, SerializableProperty property)
        {
            const string expectedRule = nameof(EnumMigrationRule);
            var ruleName = property.Rule;
            if (expectedRule != ruleName)
            {
                throw new ArgumentException($"Invalid rule applied to property {ruleName}. Expecting {expectedRule}, but received {ruleName}.");
            }

            source.AppendLine($"{indent}{property.Name} = reader.ReadEnum<{property.Type}>();");
        }

        public void GenerateSerializationMethod(StringBuilder source, string indent, SerializableProperty property)
        {
            const string expectedRule = nameof(EnumMigrationRule);
            var ruleName = property.Rule;
            if (expectedRule != ruleName)
            {
                throw new ArgumentException($"Invalid rule applied to property {ruleName}. Expecting {expectedRule}, but received {ruleName}.");
            }

            source.AppendLine($"{indent}writer.WriteEnum<{property.Type}>({property.Name});");
        }
    }
}
