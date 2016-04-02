namespace SqlMeta
{
	using System;

	public static class Inputs
	{
		// Primary Inputs
		private const string ServerName = @"(localdb)\ProjectsV12";	   
		private const string DatabaseName = "LocalDatabase";
		private const string Solution = "YourSolutionName";
		private const string Project = "YourProjectName";

		#region Secondary Inputs
		private const string ContextFolder = "Contexts"; 
		private const string EntityFolder = "Entities";
		#endregion

		#region Publically Used Properties
		public static string Connection = BuildConnectionString();
		public static string Database = DatabaseName;
		public static string DatabaseNamespace = BuildNamespace(ContextFolder);
		public static string EntityNamespace = BuildNamespace(EntityFolder);
		#endregion

		#region Helpers
		private static string BuildNamespace(string folder, string solution = Solution, string project = Project)
		{
			var result = String.Format("{0}.{1}.{2}", solution, project, folder);
			return result;
		}

		private static string BuildConnectionString(string database = DatabaseName, string server = ServerName)
		{
			var result = String.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;Pooling=False;", server, database);
			return result;
		}
		#endregion
	}
}
