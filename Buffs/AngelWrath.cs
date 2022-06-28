using Microsoft.Xna.Framework;
using SpiritMod.NPCs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class AngelWrath : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angel's Wrath");
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if (info.angelWrathStacks < 5)
			{
				info.angelWrathStacks++;
			}
			return true;
		}
		int lightnum;
		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();

			int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.Clentaminator_Purple);
			Main.dust[dust].velocity *= -1f;
			Main.dust[dust].scale *= .35f * info.angelWrathStacks;
			Main.dust[dust].noGravity = true;
			Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
			vector2_1.Normalize();
			Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
			Main.dust[dust].velocity = vector2_2;
			vector2_2.Normalize();
			Vector2 vector2_3 = vector2_2 * 34f;
			Main.dust[dust].position = npc.Center - vector2_3;

			if (info.angelWrathStacks <= 1)
				lightnum = 1;
			if (info.angelWrathStacks <= 2 || info.angelWrathStacks <= 3)
				lightnum = 2;
			if (info.angelWrathStacks >= 4)
				lightnum = 3;

			if (npc.buffTime[buffIndex] == 1)
			{
				SoundEngine.PlaySound(SoundID.Item9, npc.Center);
				for (int k = 0; k < lightnum; k++)
				{
					float num12 = Main.rand.Next(-10, 10);
					float num14 = (float)Math.Sqrt(num12 * num12 + 100 * 100);
					float num15 = 10 / num14;
					float num16 = num12 * num15;
					float num17 = 100 * num15;
					float SpeedX = num16 + Main.rand.Next(-40, 41) * 0.15f;  //this defines the projectile X position speed and randomnes
					float SpeedY = num17 + Main.rand.Next(-40, 41) * 0.15f;  //this defines the projectile Y position speed and randomnes
					Projectile.NewProjectile(npc.GetSource_Buff(buffIndex), npc.position.X, npc.position.Y + Main.rand.Next(-400, -380), SpeedX, SpeedY, ModContent.ProjectileType<AngelLightStar>(), 65, 0, Main.myPlayer, 0.0f, 1);
					info.angelWrathStacks = 0;
				}
			}
		}
	}
}
