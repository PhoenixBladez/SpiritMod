using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
    public class StarPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Burn");
			Description.SetDefault("Critical hits may cause stars to fall from the sky\nIncreases ranged damage and critical strike chance by 4% when moving");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.starBuff = true;

			if (player.velocity.X != 0)
			{
				player.rangedDamage += 0.04f;
				player.rangedCrit += 4;
			}
			else if (player.velocity.Y != 0)
			{
				player.rangedDamage += 0.04f;
				player.rangedCrit += 4;
			}
		}
	}
}
