namespace SqlMeta.Tests
{
    using System.ComponentModel;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;
    using Data.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    [TestClass]
    public class MetaRepositoryTests
    {
        [TestMethod]
        public void TestMetaRespository()
        {
            string connectionString = @"";

            var repo = new MetaRepository(connectionString: connectionString);

            var tables = repo.GetTableInfo();

            var sprocs = repo.GetStoredProcedureInfo().Where(s=>!s.Name.Contains("diagram"));

            dynamic display = new ExpandoObject();
            display.Tables = tables;
            display.Sprocs = sprocs;

            var jsonString = JsonConvert.SerializeObject(display, Formatting.Indented);

            File.WriteAllText("Shabang.txt", jsonString);
        }
    }
}
