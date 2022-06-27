using SpiritMod.Projectiles.Summon.MoonjellySummon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class MoonjellySummonBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Preserver");
			Description.SetDefault("This moonlight preserver summons tiny Lunazoa to fight for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<MoonjellySummon>()] > 0) {
				modPlayer.lunazoa = true;
			}

			if (!modPlayer.lunazoa) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
