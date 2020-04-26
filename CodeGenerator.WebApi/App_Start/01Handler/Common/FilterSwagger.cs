using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.WebApi
{ 
    /// <summary>
    /// Swagger 标签备注
    /// </summary>
    public class FilterSwagger : IDocumentFilter
    {
        /// <summary>
        ///  大标签备注
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new[] {
                new Tag { Name = "Login", Description = "登录" },
            };
        }



    }
}
