using System;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	class BabyMothronBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Baby Mothron");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("BabyMothron"), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
