using Customer.Domain.Models;
using System.Text;
using System.Text.Json;

namespace Customer.Utils.Utils
{
    public static class CustomerCsvParser
    {
        public static List<CustomerViewModel> Parse(string[] content)
        {
            var customers = content
             .Select(line => line.Split('\t'))
             .Select(x => new CustomerViewModel
             {
                 FirstName = x[0],
                 LastName = x[1]
             }).ToList();

            return customers;
        }

        public static string ComposeLine(string content)
        {
            var data = JsonSerializer.Deserialize<CustomerViewModel>(content)!;

            // TODO: compose dynamically
            StringBuilder builder = new();             
            builder.Append(data.FirstName);
            builder.Append("\t");
            builder.Append(data.LastName);
            builder.Append("\r\n");
            return builder.ToString();
        }
    }
}
