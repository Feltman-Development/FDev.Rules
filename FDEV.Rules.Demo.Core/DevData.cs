namespace FDEV.Rules.Demo.Core
{
    public static class DevData
    {
        public static class LogData
        {
            public static string DefaultDevLogFolder => "C:/SRC/LOGGING/";

            public static  bool AllowLogfilesToCrossDays => false;

            /// <inheritdoc />
            public static  int MaxNumberOfLinesPrLogFile => 10000;
        }

		/// <summary>
		/// String prefixes for command delegation
		/// </summary>
		public static string ExecutePrefix => "Execute_";
		public static string CanExecutePrefix => "CanExecute_";

		public static string ConnectionString_FDRDEV = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FDR_DEV;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    }

	//TODO: Consolidate the lists
    public enum ExcludedOperator
	{
		Equal = 1 << 0,
		NotEqual = 1 << 1,
		Less = 1 << 2,
		LessOrEqual = 1 << 3,
		Greater = 1 << 4,
		GreaterOrEqual = 1 << 5,
		Contains = 1 << 6,
		DoesNotContain = 1 << 7,
		EndsWith = 1 << 8,
		DoesNotEndWith = 1 << 9,
		StartsWith = 1 << 10,
		DoesNotStartWith = 1 << 11
	}

    public enum OperatorType
    {
	    String = 0,
	    Numeric = 1,
	    Date = 2,
	    Time = 3,
	    Bool = 4,
	    Enum = 6,
	    None = 8
    }

    public enum ParameterType
    {
	    Source = 0,
	    Input = 1,
	    Constant = 2,
	    None = 4
    }
}
