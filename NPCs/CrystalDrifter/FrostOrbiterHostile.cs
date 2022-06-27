﻿using Microsoft.Xna.Framework;
using SpiritMod.NPCs.CrystalDrifter;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.CrystalDrifter
{
	public class FrostOrbiterHostile : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Frost Spirit");

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
		}

		public override void AI()
		{
			int num1 = ModContent.NPCType<CrystalDrifter>();
			float num2 = 60f;
			float x = 0.2f;
			float y = 0.4f;
			bool flag2 = false;
			if ((double)Projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)Projectile.ai[1];
				if (Main.npc[index1].active && Main.npc[index1].type == num1) {
					if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
						Projectile.position = Projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
				}
				else {
					Projectile.ai[0] = num2;
					flag4 = false;
				}
				if (flag4 && !flag2) {
					Projectile.velocity = Projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
				}
			}
			for (int i = 0; i < 5; i++) {
				if (Projectile.width == 8) {
					float x1 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
					float y1 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
					int num = Dust.NewDust(new Vector2(x1, y1), 2, 2, DustID.Snow);
					Main.dust[num].velocity = Vector2.Zero;
					Main.dust[num].noGravity = true;
				}
			}

		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.Frostburn, 120);
		public override void Kill(int timeLeft)
		{
			const int DustType = 51;

			for (int k = 0; k < 6; k++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.White, 0.7f);
			}

			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.White, 0.7f);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.White, 0.7f);
			Projectile.velocity *= 0f;
			Projectile.width = 40;
			Projectile.knockBack = 0;
		}
	}
}
