using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class IceBerryBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ice Berries");
			Description.SetDefault("You are immune to being on fire");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}
