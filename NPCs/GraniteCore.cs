using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class GraniteCore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crackling Core");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 32;
			npc.damage = 30;
			npc.defense = 4;
			npc.noGravity = true;
			npc.lifeMax = 18;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 360f;
			npc.knockBackResist = .3f;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 226;
			for(int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.87f);
			}
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			npc.localAI[0] += 1f;
			if(npc.localAI[0] == 12f) {
				npc.localAI[0] = 0f;
				for(int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(npc.Center, 0, 0, 226, 0f, 0f, 160, default(Color), 1f);
					Main.dust[num8].scale = 0.9f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = npc.Center + vector2;
					Main.dust[num8].velocity = npc.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
		}
		public override bool CheckDead()
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 110));
			for(int i = 0; i < 20; i++) {
				int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .25f;
				if(Main.dust[num].position != npc.Center)
					Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
			}
			for(int i = 0; i < 4; i++) {
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y,
					velocity.X, velocity.Y, mod.ProjectileType("GraniteShard1"), 13, 1, Main.myPlayer, 0, 0);
				Main.projectile[proj].friendly = false;
				Main.projectile[proj].hostile = true;
				Main.projectile[proj].velocity *= 4f;
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GraniteChunk>(), 1);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			{
				target.AddBuff(BuffID.Confused, 160);
			}
		}
	}
}
