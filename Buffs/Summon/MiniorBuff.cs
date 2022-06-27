using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class MiniorBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Meteor");
			Description.SetDefault("A mini meteor fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
		MyPlayer modPlayer = player.GetSpiritPlayer();
		if (player.ownedProjectileCounts[ModContent.ProjectileType<Minior>()] > 0) {
			modPlayer.minior = true;
			player.buffTime[buffIndex] = 18000;
			}

		if (!modPlayer.minior) {
			player.DelBuff(buffIndex);
			buffIndex--;
			return;
			}
		}
	}
}
