using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.EventSystem
{
	public class EventPlayer : ModPlayer
	{
		public static event Action OnModifyScreenPosition;

		public override void ModifyScreenPosition()
		{
			OnModifyScreenPosition?.Invoke();
		}
	}
}
