using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mimic
{
	public class GoldCrateMimic : ModNPC
	{
		bool jump = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Crate Mimic");
			Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 46;
			NPC.height = 40;
			NPC.damage = 22;
			NPC.defense = 12;
			NPC.lifeMax = 110;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 460f;
			NPC.knockBackResist = .15f;
			NPC.aiStyle = 41;
			AIType = NPCID.Herpling;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.GoldCrateMimicBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("While you may not enjoy their aggressive nature, you do respect their eye for fashion...at least, until you take it."),
			});
		}

		int frame = 2;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;

			if (NPC.collideY && jump && NPC.velocity.Y > 0)
			{
				if (Main.rand.NextBool(2))
				{
					jump = false;
					for (int i = 0; i < 20; i++)
					{
						int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.SpookyWood, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
						Main.dust[dust].noGravity = true;
					}
				}
			}

			if (!NPC.collideY)
				jump = true;
		}

		public override void FindFrame(int frameHeight)
		{
			Player target = Main.player[NPC.target];
			if (NPC.DistanceSQ(target.Center) < 720 * 720)
			{
				NPC.frameCounter++;
				if (NPC.frameCounter == 5)
				{
					frame++;
					NPC.frameCounter = 0;
				}

				if (frame >= 4)
					frame = 1;
			}
			else
			{
				frame = 0;
				NPC.velocity = Vector2.Zero;
			}

			NPC.frame.Y = frameHeight * frame;
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));

			if (distance > 720)
			{
				NPC.dontTakeDamage = true;
				if (!projectile.minion)
				{
					projectile.hostile = true;
					projectile.friendly = false;
					projectile.penetrate = 2;
					projectile.velocity.X *= -1f;
				}
				NPC.life = 100;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
			if (distance < 720)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 6, 220);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 6, 221);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 6, 222);
			}
			if (NPC.life <= 0 || NPC.life >= 0)
			{
				for (int k = 0; k < 6; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, .77f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(ItemID.GoldenCrate));
	}
}