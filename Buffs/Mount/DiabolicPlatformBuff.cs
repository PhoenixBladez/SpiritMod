using System;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	class DiabolicPlatformBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Diabolic Platform");
			Description.SetDefault("Command the Infernal");


			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("DiabolicPlatform"), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
