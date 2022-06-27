﻿using Microsoft.Xna.Framework;
using SpiritMod.NPCs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class AngelLight : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angel's Light");
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if (info.angelLightStacks < 3)
			{
				info.angelLightStacks++;
			}
			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();

			int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.Clentaminator_Purple);
			Main.dust[dust].velocity *= -1f;
			Main.dust[dust].scale *= .5f * info.angelLightStacks;
			Main.dust[dust].noGravity = true;
			Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
			vector2_1.Normalize();
			Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
			Main.dust[dust].velocity = vector2_2;
			vector2_2.Normalize();
			Vector2 vector2_3 = vector2_2 * 34f;
			Main.dust[dust].position = npc.Center - vector2_3;

			if (info.angelLightStacks == 2)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 9);
				for (int k = 0; k < Main.rand.Next(1, 3); k++)
				{
					float num12 = Main.rand.Next(-10, 10);
					float num14 = (float)Math.Sqrt(num12 * num12 + 100 * 100);
					float num15 = 10 / num14;
					float num16 = num12 * num15;
					float num17 = 100 * num15;
					float SpeedX = num16 + Main.rand.Next(-40, 41) * 0.05f;  //this defines the projectile X position speed and randomnes
					float SpeedY = num17 + Main.rand.Next(-40, 41) * 0.05f;  //this defines the projectile Y position speed and randomnes
					Projectile.NewProjectile(npc.position.X, npc.position.Y + Main.rand.Next(-400, -380), SpeedX, SpeedY, ModContent.ProjectileType<AngelLightStar>(), 50, 0, Main.myPlayer, 0.0f, 1);
					info.angelLightStacks = 0;
				}
			}
		}
	}
}