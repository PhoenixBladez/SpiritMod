using System;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	class BroomBuff : ModBuff
	{
		public static int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Enchanted Broom");
			Description.SetDefault("It's a kind of magic");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(Mounts.BroomMount._type, player);
			player.buffTime[buffIndex] = 10;

			player.minionDamage += 0.08f;
			player.magicDamage += 0.08f;
		}
	}
}
