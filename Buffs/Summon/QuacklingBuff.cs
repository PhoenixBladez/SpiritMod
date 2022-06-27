using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class QuacklingBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quackling Minion");
			Description.SetDefault("Born with a bandana!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<QuacklingMinion>()] > 0) {
				modPlayer.QuacklingMinion = true;
				player.buffTime[buffIndex] = 18000;
			}

			else if (!modPlayer.QuacklingMinion) {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
