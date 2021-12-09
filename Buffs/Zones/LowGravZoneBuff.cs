using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Zones
{
	public class LowGravZoneBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Low Gravity Zone");
			Description.SetDefault("You feel light!");
			Main.pvpBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.gravity = .05f;
            if (player.velocity.Y != 0 && player.wings <= 0 && !player.mount.Active)
            {
                player.runAcceleration *= 1.45f;
                player.maxRunSpeed *= 1.2f;
			}

			if (Main.rand.NextBool(3))
			{
				int d = Dust.NewDust(player.position, player.width, player.height, DustID.FireworkFountain_Blue, Main.rand.NextFloat(-0.1f, 0.1f), 
					Main.rand.NextFloat(-0.1f, 0.1f), 150, default, Main.rand.NextFloat(0.5f, 0.7f));
				Main.dust[d].noGravity = true;
				Main.dust[d].fadeIn = 0.7f;
				Main.dust[d].shader = Terraria.Graphics.Shaders.GameShaders.Armor.GetSecondaryShader(87, Main.LocalPlayer);
			}
		}
	}
}
