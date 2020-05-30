using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Buffs
{
    public class DrainLife : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Drain Life");
            Description.SetDefault("Your life energy becomes theirs.");
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}
		int counter = 0;
		public override void Update(NPC npc, ref int buffIndex)
		{

			npc.lifeRegen -= 2;
			Player player = Main.player[npc.target];
			counter++;
			if (counter % 15 == 0)
			{
				player.HealEffect(1);
				player.statLife += 1;
			}
            if (Main.rand.NextBool(2))
            {
                Vector2 center = npc.Center;
                center.X += Main.rand.Next(-100, 100) * 0.05F;
                center.Y += Main.rand.Next(-100, 100) * 0.05F;
                center += npc.velocity;

                int dust = Dust.NewDust(center, 1, 1, mod.DustType("BloodSiphonDust"));
                Main.dust[dust].velocity *= 0.0f;
                Main.dust[dust].scale = Main.rand.Next(70, 85) * 0.01f;
                Main.dust[dust].fadeIn = 1;
            }
        }
	}
}
