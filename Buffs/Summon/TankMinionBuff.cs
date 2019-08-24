using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class TankMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tank Minion");
			Description.SetDefault("Powered by the thermal depths");

			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("TankMinion")] > 0)
			{
				modPlayer.tankMinion = true;
			}
			if (!modPlayer.tankMinion)
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