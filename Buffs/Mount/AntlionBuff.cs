using System;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	class AntlionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Antlion Charger");
			Description.SetDefault("Its sharp claws aid your digging underground");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("AntlionMount"), player);
			player.buffTime[buffIndex] = 10;

			if (player.ZoneRockLayerHeight)
			{
				player.pickSpeed -= 0.20f;
			}
		}
	}
}
