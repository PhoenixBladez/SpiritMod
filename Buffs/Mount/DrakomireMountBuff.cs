using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	public class DrakomireMountBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Drakomire");
			Description.SetDefault("You have tamed a Celestial Invader!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("Drakomire"), player, false);
			player.buffTime[buffIndex] = 10;
		}
	}
}
