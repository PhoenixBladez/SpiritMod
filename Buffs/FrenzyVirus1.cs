using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class FrenzyVirus1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Viral Wrath");
			Description.SetDefault("The virus has mutated within you...\nIncreases damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.allDamage += 0.05f;
		}
	}
}
