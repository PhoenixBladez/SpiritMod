using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class FrenzyPlant : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Nature's Fury");
			Description.SetDefault("Increases damage and movement speed by 8%");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += 0.05f;
			player.allDamage += 0.05f;
		}
	}
}
