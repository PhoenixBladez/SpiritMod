using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class Righteous : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Righteous Virtue");
			Description.SetDefault("'Blessed by the divine'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.magicDamage += .1f;
		}
	}
}
