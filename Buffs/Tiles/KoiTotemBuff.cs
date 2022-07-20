using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Tiles
{
	public class KoiTotemBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of the Koi");
			Description.SetDefault("Increased fishing skill");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) => player.fishingSkill += 5;
	}
}
