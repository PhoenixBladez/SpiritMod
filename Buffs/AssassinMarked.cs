using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class AssassinMarked : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Assassin's Mark");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (Main.rand.Next(2) == 0) {
				for (int k = 0; k < 2; k++) {
					int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 130);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale *= .5f;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = npc.Center - vector2_3;
				}
			}
		}
	}
}
