using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	public class TideMountBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Greenfin Treader Mount");
			Description.SetDefault("You ride a beast from the deep\nHow's it breathing?");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("TideMount"), player, false);
			player.buffTime[buffIndex] = 10;
		}
	}
}
