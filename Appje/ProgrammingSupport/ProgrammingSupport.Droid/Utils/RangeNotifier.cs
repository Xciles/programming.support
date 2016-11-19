﻿using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;

namespace ProgrammingSupport.Droid
{
	public class RangeEventArgs : EventArgs
	{
		public Region Region { get; set; }
		public ICollection<Beacon> Beacons { get; set; }
	}

	public class RangeNotifier : Java.Lang.Object, IRangeNotifier
	{
		public event EventHandler<RangeEventArgs> DidRangeBeaconsInRegionComplete;

		public void DidRangeBeaconsInRegion(ICollection<Beacon> beacons, Region region)
		{
			OnDidRangeBeaconsInRegion(beacons, region);
		}

		private void OnDidRangeBeaconsInRegion(ICollection<Beacon> beacons, Region region)
		{
			if (DidRangeBeaconsInRegionComplete != null)
			{
				DidRangeBeaconsInRegionComplete(this, new RangeEventArgs { Beacons = beacons, Region = region });
			}
		}
	}
}
