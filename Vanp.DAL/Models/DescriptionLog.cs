using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanp.DAL.Models
{
    public class DescriptionLog
    {
        public int ProductId { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string ProductText { get; set; }
        public DescriptionLog()
        {
        }
        public static IEnumerable<DescriptionLog> GetListByPath(string logPath)
        {
            var serializer = new JsonSerializer();
            using (var re = File.OpenText(logPath))
            using (var reader = new JsonTextReader(re))
            {
                var entries = serializer.Deserialize<DescriptionLog[]>(reader);
                return entries;
            }
        }
        public bool Insert(string logPath)
        {
            bool isExisted = true;
            if (!File.Exists(logPath))
            {
                isExisted = false;
            }
            var data = new List<DescriptionLog>();
            if (isExisted && GetListByPath(logPath) != null)
            {
                data = GetListByPath(logPath).ToList();
            }
            data.Add(this);
            string json = JsonConvert.SerializeObject(data.ToArray());
            System.IO.File.WriteAllText(logPath, json);
            return true;
        }
    }
}
