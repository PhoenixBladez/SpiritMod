using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	public class DrakinBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Drakin Mount");
			Description.SetDefault("A wild ride indeed");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("DrakinMount"), player, false);
			player.buffTime[buffIndex] = 10;

			player.minionDamage += 0.05f;
			player.meleeDamage += 0.05f;
			player.thrownDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.magicDamage += 0.08f;

			player.statDefense += 10;

			player.GetModPlayer<MyPlayer>(mod).drakinMount = true;
		}
	}
}
