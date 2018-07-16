using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace PM.Domain.Test.Attributes
{
    public class JsonFileDataAttribute : DataAttribute
    {
        private readonly string _filePath;
        private readonly string _propertyName;

        public JsonFileDataAttribute(string filePath)
            : this(filePath, null) { }

        public JsonFileDataAttribute(string filePath, string propertyName)
        {
            _filePath = filePath;
            _propertyName = propertyName;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            var fileData = File.ReadAllText(_filePath);

            MethodBase method = testMethod.DeclaringType.GetMethod(testMethod.Name);
            ParameterInfo[] parameters = method.GetParameters();

            if (string.IsNullOrEmpty(_propertyName))
            {
                var jsonData = JsonConvert.DeserializeObject<List<object[]>>(fileData);
                return ConvertParamTypes(jsonData, parameters);
            }
            else
            {
                var jsonData = JObject.Parse(fileData)[_propertyName];
                return ConvertParamTypes(jsonData.ToObject<List<object[]>>(), parameters);
            }

        }

        private IEnumerable<object[]> ConvertParamTypes(List<object[]> jsonData, ParameterInfo[] parameters)
        {
            var result = new List<object[]>();

            foreach (var paramsTuple in jsonData)
            {
                var paramValues = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramValues[i] = ((JObject)paramsTuple[i]).ToObject(parameters[i].ParameterType);
                }

                result.Add(paramValues);
            }

            return result;
        }
    }
}
