using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.JinxprobeWand
{
	public class JinxprobeBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tiny Jinxprobe");
			Description.SetDefault("The jinxprobe will fight for you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("Jinxprobe")] > 0)
			{
				modPlayer.Jinxprobe = true;
			}
			if (!modPlayer.Jinxprobe)
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