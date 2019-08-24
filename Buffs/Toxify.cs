using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Toxify : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Toxify");
			Description.SetDefault("Reduced damage, life regen, and defense");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense = (npc.defDefense / 100) * 85;
			npc.damage = (npc.defDamage / 100) * 85;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			player.lifeRegen = 0;
			player.statDefense = (player.statDefense / 100) * 85;
			player.magicDamage *= 0.85f;
			player.meleeDamage *= 0.85f;
			player.rangedDamage *= 0.85f;
			player.minionDamage *= 0.85f;
			player.thrownDamage *= 0.85f;

			modPlayer.toxify = true;
		}
	}
}
