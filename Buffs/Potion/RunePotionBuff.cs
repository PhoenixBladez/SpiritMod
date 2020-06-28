using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
	public class RunePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Runescribe");
			Description.SetDefault("Magic attacks may cause enemies to erupt into runes\n5% increased magic damage");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.runeBuff = true;
			player.magicDamage += 0.05f;
		}
	}
}
