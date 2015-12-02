namespace Artportalen.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Artportalen.Model;

    public class AttributeCalculator
    {
        public int GetQuantity(string attribute)
        {
            if (string.IsNullOrEmpty(attribute))
            {
                return 0;
            }

            if (attribute.Contains(" "))
            {
                int q;
                if (int.TryParse(attribute.Substring(0, attribute.IndexOf(" ", StringComparison.Ordinal)), out q))
                {
                    return q;
                }
            }

            return 0;
        }

        public int? GetStageId(string attribute)
        {
            if (string.IsNullOrEmpty(attribute))
            {
                return null;
            }

            var attributeValues = this.GetAttributeValues<StageEnum>();
            for (int i = 0; i < attributeValues.Length; i++)
            {
                if (attribute.Contains(attributeValues[i].Item1))
                {
                    return (int)attributeValues[i].Item2;
                }
            }

            return null;
        }

        public int? GetGenderId(string attribute)
        {
            if (string.IsNullOrEmpty(attribute))
            {
                return null;
            }

            var attributeValues = this.GetAttributeValues<GenderEnum>();
            for (int i = 0; i < attributeValues.Length; i++)
            {
                if (attribute.Contains(attributeValues[i].Item1))
                {
                    return (int)attributeValues[i].Item2;
                }
            }

            return null;
        }

        public int? GetActivityId(string attribute)
        {
            if (string.IsNullOrEmpty(attribute))
            {
                return null;
            }
            
            var attributeValues = this.GetAttributeValues<ActivityEnum>();
            for (int i = 0; i < attributeValues.Length; i++)
            {
                if (attribute.Contains(attributeValues[i].Item1))
                {
                    return (int)attributeValues[i].Item2;
                }
            }

            return null;
        }

        public string GetAttribute(int quantity, int? stageId, int? genderId, int? activityId)
        {
            var attributes = new List<string>();
            var stage = ParseEnum<StageEnum>(stageId ?? 0);
            var gender = ParseEnum<GenderEnum>(genderId ?? 0);
            var activity = ParseEnum<ActivityEnum>(activityId ?? 0);

            attributes.Add(quantity == 0 ? "-" : quantity.ToString(CultureInfo.InvariantCulture));

            if (gender == GenderEnum.Undefined && stage == StageEnum.Undefined)
            {
                attributes.Add("ex");
            }
            else
            {
                if (stage != StageEnum.Undefined)
                {
                    attributes.Add(this.GetEnumDisplayShortName(stage));
                }

                if (gender != GenderEnum.Undefined)
                {
                    attributes.Add(this.GetEnumDisplayShortName(gender));
                }
            }

            if (activity != ActivityEnum.Undefined)
            {
                attributes.Add(this.GetEnumDisplayShortName(activity));
            }

            return string.Join(" ", attributes);
        }

        private TEnum ParseEnum<TEnum>(int value)
        {
            if (typeof (TEnum).GetTypeInfo().IsEnum)
            {
                if (typeof (TEnum).GetTypeInfo().IsEnumDefined(value))
                {
                    return (TEnum)Enum.Parse(typeof (TEnum), value.ToString());
                }

                var values = Enum.GetValues(typeof (TEnum));
                return (TEnum)values.GetValue(0);
            }

            throw new InvalidOperationException("Only enum types supported");
        }

        private string GetEnumDisplayShortName(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute),
                false);

            if (attributes.Length > 0)
            {
                return attributes[0].GetShortName();
            }

            return value.ToString();
        }

        private Tuple<string, T>[] GetAttributeValues<T>()
        {
            var values = new List<Tuple<string, T>>();
            var enumvalues = Enum.GetValues(typeof(T));

            foreach (T enumValue in enumvalues)
            {
                var shortName = this.GetEnumDisplayShortName(enumValue as Enum);
                if (!string.IsNullOrEmpty(shortName))
                {
                    values.Add(new Tuple<string, T>(string.Format(" {0}", shortName), enumValue));
                }
            }

            return values.OrderByDescending(x => x.Item1.Length).ToArray();
        }
    }
}
