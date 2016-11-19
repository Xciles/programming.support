using System;
namespace ProgrammingSupport.Core
{
	public static class BeaconStats
	{
		public static EArea ClosestArea { get; set; }

		public static EProximity ProximityToClosestArea { get; set; }
	}

	public enum EArea
	{
		Unknown = 0,
		CSharp = 1,
		Java = 2
	}

	public enum EProximity
	{
		Unknown = 0,
		Far = 1,
		Medium = 2,
		Close = 3,
		OnTop = 4
	}
}
