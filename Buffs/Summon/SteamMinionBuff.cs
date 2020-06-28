using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class SteamMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Starplate Minion");
			Description.SetDefault("Uses stars as a conduit");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(player.ownedProjectileCounts[ModContent.ProjectileType<SteamMinion>()] > 0) {
				modPlayer.steamMinion = true;
			}

			if(!modPlayer.steamMinion) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
