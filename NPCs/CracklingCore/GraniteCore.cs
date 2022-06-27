using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.GraniteSet;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.CracklingCore
{
	public class GraniteCore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crackling Core");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 32;
			NPC.damage = 30;
			NPC.defense = 4;
			NPC.noGravity = true;
			NPC.lifeMax = 18;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 360f;
			NPC.knockBackResist = .3f;
			NPC.aiStyle = 44;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;

			AIType = NPCID.FlyingAntlion;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.CracklingCoreBanner>();
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.87f);
			}
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			NPC.localAI[0] += 1f;
			if (NPC.localAI[0] == 12f)
			{
				NPC.localAI[0] = 0f;
				for (int j = 0; j < 12; j++)
				{
					Vector2 vector2 = Vector2.UnitX * -NPC.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (NPC.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(NPC.Center, 0, 0, DustID.Electric, 0f, 0f, 160, default, 1f);
					Main.dust[num8].scale = 0.9f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = NPC.Center + vector2;
					Main.dust[num8].velocity = NPC.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
		}

		public override bool CheckDead()
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 110));
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .25f;
				if (Main.dust[num].position != NPC.Center)
					Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 6f;
			}

			for (int i = 0; i < 4; i++)
			{
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y,
					velocity.X, velocity.Y, ModContent.ProjectileType<GraniteShard1>(), 13, 1, Main.myPlayer, 0, 0);
				Main.projectile[proj].friendly = false;
				Main.projectile[proj].hostile = true;
				Main.projectile[proj].velocity *= 4f;
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void OnKill() => Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<GraniteChunk>(), 1);
		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Confused, 160);
	}
}