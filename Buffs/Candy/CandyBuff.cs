using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Candy
{
	public class CandyBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sugar Rush");
			Description.SetDefault("Increased stats");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.5f;
			player.statDefense += 2;
			player.GetCritChance(DamageClass.Generic) += 2;
			player.GetDamage(DamageClass.Generic) += 0.04f;
			player.lifeRegen += 1;
			player.jumpSpeedBoost += 0.4f;
		}
	}
}
