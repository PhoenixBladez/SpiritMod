using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
    public class LavaRockSummon : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Magmarock");
			Description.SetDefault("This glob of magma is sentient and surprisingly good at its job!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[mod.ProjectileType("LavaRockSummon")] > 0)
			{
				modPlayer.lavaRock = true;
			}

			if (!modPlayer.lavaRock)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}