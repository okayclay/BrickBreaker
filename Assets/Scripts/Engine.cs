using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	class Engine : MonoBehaviour
	{
		public enum Mode
		{
			Play,
			Move,
			None, 
			Pause
		}

		private static Mode curMode;

		public static Mode CurrentMode { get { return curMode; } }

		void Start()
		{
			ChangeMode(Mode.Pause);
		}

		public static void ChangeMode(Mode newMode)
		{
			curMode = newMode;
		}
	}
}
