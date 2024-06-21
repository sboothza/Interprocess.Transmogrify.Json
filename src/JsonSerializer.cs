using System.Collections;
namespace Interprocess.Transmogrify.Json
{
    public static class JsonSerializer
    {
        public static string Serialize(object source, JsonSerializerOptions? options = null)
        {
            var writer = new StringWriter();
            Serialize(source, writer, options);
            return writer.ToString();
        }

        private static void Serialize(object? source, TextWriter writer, JsonSerializerOptions? options)
        {
            if (source is null)
            {
                writer.Write("null");
                return;
            }

            options ??= JsonSerializerOptions.Empty;

            if (source.Flatten(out var result))
            {
                writer.Write(result);
                return;
            }

            //complex
            if (source is IEnumerable sourceEnumerable)
            {
                if (sourceEnumerable is IDictionary dictionarySource)
                {
                    //handle dictionary
                    writer.Write('{');

                    var dictionaryEntries = dictionarySource.ToList();

                    dictionaryEntries.ProcessList(entry =>
                    {
                        writer.Write($"\"{entry.Key}\" : ");
                        Serialize(entry.Value, writer, options);

                        writer.Write(',');
                    }, entry =>
                    {
                        writer.Write($"\"{entry.Key}\" : ");
                        Serialize(entry.Value, writer, options);
                    });

                    writer.Write('}');
                    return;
                }

                //handle list
                writer.Write('[');

                var listEntries = sourceEnumerable.Cast<object>();

                listEntries.ProcessList(o =>
                {
                    Serialize(o, writer, options);
                    writer.Write(',');
                }, o => Serialize(o, writer, options));

                writer.Write(']');
                return;
            }

            //single object
            writer.Write('{');

            var entries = new List<PropertyTuple>();
            var props = source.GetType()
                              .GetProperties()
                              .Select(p => new
                              {
                                  prop = p,
                                  name = ((JsonPropertyAttribute)p.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                                                  .FirstOrDefault()!)?.Name ?? p.Name,
                                  ignore = ((JsonPropertyAttribute)p.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                                                    .FirstOrDefault()!)?.Ignore ?? false,
                                  renamed = !string.IsNullOrEmpty(((JsonPropertyAttribute)p
                                      .GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                      .FirstOrDefault()!)?.Name)
                              })
                              .Where(pn => options.IgnorePropertyAttributes || !pn.ignore);

            foreach (var prop in props)
            {
                if (!options.DontSerializeNulls || source.GetFieldOrPropertyValue(prop.prop.Name) is not null)
                {
                    var name = options.IgnorePropertyAttributes ? prop.prop.Name : prop.name;
                    if (!prop.renamed && !options.IgnorePropertyAttributes)
                        name = name.ConvertName(options.Naming);
                    var entry = PropertyTuple.Create(options, source, prop.prop.Name, name);
                    if (entry is not null)
                        entries.Add(entry);
                }
            }

            entries.ProcessList(tuple =>
            {
                writer.Write($"\"{tuple.OutputName}\" : ");
                Serialize(tuple.Value, writer, options);
                writer.Write(',');
            }, tuple =>
            {
                writer.Write($"\"{tuple.OutputName}\" : ");
                Serialize(tuple.Value, writer, options);
            });

            writer.Write('}');
        }

        public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
        {
            return (T?)Deserialize(json, typeof(T), options);
        }

        public static object? Deserialize(string json, Type type, JsonSerializerOptions? options)
        {
            var jsonObject = JsonParser.ParseObject(json);
            if (jsonObject is null)
                return null;
            options ??= JsonSerializerOptions.Empty;
            return Deserialize(jsonObject, type, options);
        }

        private static object? Deserialize(JsonObject? obj, Type type, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(type);

            switch (obj)
            {
                case null: return null;
                case JsonObjectValue jsonPrimitive:
                {
                    if (jsonPrimitive.Value is string strValue)
                    {
                        if (strValue == "null")
                            return null;
                        if (strValue.StartsWith("/Date("))
                        {
                            long unix = long.Parse(strValue.Replace("/Date(", "")
                                                           .Replace(")/", ""));
                            var value = DateTimeOffset.FromUnixTimeMilliseconds(unix);
                            return value.DateTime;
                        }
                    }

                    return Convert.ChangeType(jsonPrimitive.Value, type);
                }
                case JsonObjectComplex jsonObjectComplex:
                {
                    object? result = Activator.CreateInstance(type);
                    if (result is null)
                        throw new NullReferenceException("Result cannot be null");

                    var props = type.GetProperties()
                                    .Select(p => new
                                    {
                                        prop = p,
                                        name = ((JsonPropertyAttribute)p.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                                                        .FirstOrDefault()!)?.Name ?? p.Name,
                                        ignore = ((JsonPropertyAttribute)p.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                                                          .FirstOrDefault()!)?.Ignore ?? false,
                                        renamed = !string.IsNullOrEmpty(((JsonPropertyAttribute)p
                                            .GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                            .FirstOrDefault()!)?.Name)
                                    })
                                    .Where(pn => options.IgnorePropertyAttributes || !pn.ignore);

                    foreach (var prop in props)
                    {
                        if (prop.prop.CanWrite)
                        {
                            var name = prop.name;
                            if (options.IgnorePropertyAttributes)
                                name = prop.prop.Name;

                            JsonObject? jsonValue = null;
                            if (options.RemapFields.TryGetValue(name, out string? mappedName))
                                name = mappedName;
                            else
                                name = name.ConvertName(options.Naming);

                            if (jsonObjectComplex.Complex.TryGetValue(name, out var complexValue))
                                jsonValue = complexValue!;

                            var propertyType = GetBaseType(prop.prop.PropertyType);
                            var value = Deserialize(jsonValue, propertyType, options);
                            prop.prop.SetValue(result, value);
                        }
                    }

                    return result;
                }
                case JsonObjectList jsonList:
                {
                    var elementType = type.GetGenericArguments()[0];
                    var list = (IList?)Activator.CreateInstance(type);
                    if (list is null)
                        throw new NullReferenceException("list shouldn't be null");

                    foreach (var item in jsonList.Array)
                        list.Add(Deserialize(item, elementType, options));

                    return list;
                }
                default: throw new IndexOutOfRangeException("Could not deserialize");
            }
        }

        private static Type GetBaseType(Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return propertyType.GetGenericArguments()[0];
            return propertyType;
        }
    }
}