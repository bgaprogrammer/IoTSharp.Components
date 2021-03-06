﻿using System;
using System.Diagnostics;

namespace IoTSharp.Components
{
	public class IoTSensor : IoTComponent, IIoTSensor
	{
		static readonly ITracer tracer = Tracer.Get<IoTSensor> ();
		public event Action<bool> PresenceStatusChanged;
		readonly IoTPin pin;

		public bool HasPresence {
			get; private set;
		}

		public IoTSensor (Connectors gpio)
		{
			pin = new IoTPin (gpio);
			pin.SetDirection (IoTPinDirection.DirectionIn);
			pin.SetActiveType (IoTActiveType.ActiveLow);
			HasPresence = pin.Value;
			tracer.Verbose ("Initial value: " + HasPresence);
		}

		public override void Update ()
		{
			var presence = pin.Value;
			if (presence == HasPresence)
				return;
			HasPresence = presence;
			tracer.Verbose ("Detected presence: " + HasPresence);
			PresenceStatusChanged?.Invoke (presence);
		}

		public override void Dispose ()
		{
			pin.Close ();
		}
	}
}
