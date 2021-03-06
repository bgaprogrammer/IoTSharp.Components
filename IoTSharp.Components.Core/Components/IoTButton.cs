﻿using System;
using System.Diagnostics;

namespace IoTSharp.Components
{
	public  class IoTButton : IoTComponent, IIoTButton
	{
		static readonly ITracer tracer = Tracer.Get<IoTButton> ();

		public event Action ButtonDown;
		public event Action ButtonUp;
		public event Action Clicked;

		public bool IsPressed { get; private set; }
		readonly IoTPin pin;

		public IoTButton (Connectors gpio)
		{
			pin = new IoTPin (gpio);
			pin.SetDirection (IoTPinDirection.DirectionIn);
			pin.SetActiveType (IoTActiveType.ActiveLow);
            IsPressed = false;
			tracer.Verbose ("Initial value: " + IsPressed);
		}

		public override void Update ()
		{
			var value = !pin.Value ;
			if (IsPressed == value)
				return;
			IsPressed = value;
			if (IsPressed) {
				tracer.Verbose ("Buton Down");
				ButtonDown?.Invoke ();
			} else {
                tracer.Verbose ("Buton Up");
				ButtonUp?.Invoke ();
				Clicked?.Invoke ();
			}
		}

		public override void Dispose ()
		{
			pin.Close ();
		}
	}
}
