using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	class TikiInfestation : ModBuff
	{
		public static ModBuff _ref;

		public const int maxStacks = 10;
		public const int duration = 600;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tiki Infestation");
			Main.buffNoTimeDisplay[Type] = false;
		}

	}
}
