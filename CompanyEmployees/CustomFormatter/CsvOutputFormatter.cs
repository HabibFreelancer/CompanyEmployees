using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
using System.Text;

namespace CompanyEmployees.CustomFormatter
{
    /*its a custom outputFormatter to render csv format in the http response
     * (Http Header => Accept => text/csv*/
    public class CsvOutputFormatter : TextOutputFormatter
    {

        /*In the constructor, we define which media type this formatter should
        parse as well as encodings*/
        public CsvOutputFormatter()
        {
            /* apply the csv custom format when we have text/csv int http Header */
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        /*The CanWriteType method is overridden, and it indicates whether
        or not the CompanyDto type can be written by this serializer*/
        protected override bool CanWriteType(Type? type)
        {
            if (typeof(CompanyDto).IsAssignableFrom(type) ||
            typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, 
            Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<CompanyDto>)
            {
                foreach (var company in (IEnumerable<CompanyDto>)context.Object)
                {
                    FormatCsv(buffer, company);
                }
            }
            else
            {
                FormatCsv(buffer, (CompanyDto)context.Object);
            }
            await response.WriteAsync(buffer.ToString());
        }
        /*FormatCsv method that formats a response
        the way we want it*/
        private static void FormatCsv(StringBuilder buffer, CompanyDto company)
        {
            buffer.AppendLine($"{company.Id},\"{company.Name},\"{company.FullAddress}\"");
        }
    }
}
