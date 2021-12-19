using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Memoriae.Helpers.Swagger
{
    public static class SwaggerExtension
    {
        public static void SetDoc(this SwaggerGenOptions swaggerGenOptions, OpenApiInfo swaggerInfo)
        {
            var assembly = Assembly.GetEntryAssembly();
            // ReSharper disable once PossibleNullReferenceException
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            swaggerInfo.Description = string.IsNullOrEmpty(swaggerInfo.Description)
                                          ? $"Version: {fileVersion.FileVersion}"
                                          : $"{swaggerInfo.Description}{Environment.NewLine}Version: {fileVersion.FileVersion}";
            swaggerGenOptions.SwaggerDoc(swaggerInfo.Version, swaggerInfo);
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swaggerGenOptions.IncludeXmlComments(xmlPath);
        }

       
    }
}
