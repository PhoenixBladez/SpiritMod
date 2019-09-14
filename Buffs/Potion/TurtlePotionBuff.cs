using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Potion
{
    public class TurtlePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Steadfast");
			Description.SetDefault("Increases defense as health wanes\nReduces damage taken by 5%");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            if (player.statLife < 100)
            {
                player.statDefense += 17;
            }
            else if (player.statLife < 200)
            {
                player.statDefense += 12;
            }
            else if (player.statLife < 300)
            {
                player.statDefense += 7;
            }
            else if (player.statLife < 500)
			{
				player.statDefense += 3;
			}

			player.endurance += 0.05f;
		}
	}
}
