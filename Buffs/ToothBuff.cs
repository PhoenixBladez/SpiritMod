using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class ToothBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Poison Bite");
			Description.SetDefault("Attacks poison foes");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.gremlinBuff = true;

			int dust = Dust.NewDust(player.position, player.width, player.height, 75);
			Main.dust[dust].scale = 0.5f;
			Main.dust[dust].noGravity = true;
		}
	}
}
