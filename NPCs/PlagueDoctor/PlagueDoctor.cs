using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.PlagueDoctor;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.PlagueDoctor
{
	public class PlagueDoctor : ModNPC
	{
		bool attack = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Alchemist");
			Main.npcFrameCount[NPC.type] = 12;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 48;
			NPC.damage = 29;
			NPC.defense = 16;
			NPC.lifeMax = 140;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 220f;
			NPC.knockBackResist = .35f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.DarkAlchemistBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Obsidian, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/PDoctor1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/PDoctor2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/PDoctor3").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.GoldenKey, 153);
			npcLoot.AddCommon(ItemID.Nazar, 75);
			npcLoot.AddCommon(ItemID.TallyCounter, 100);
			npcLoot.AddCommon(ItemID.BoneWand, 250);
			npcLoot.AddCommon<Items.Weapon.Thrown.PlagueVial>(1, 26, 42);
			npcLoot.AddOneFromOptions(ModContent.ItemType<PlagueDoctorCowl>(), ModContent.ItemType<PlagueDoctorRobe>(), ModContent.ItemType<PlagueDoctorLegs>());
		}


		int frame = 0;
		int timer = 0;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];

			float distance = NPC.DistanceSQ(target.Center);

			if (distance < 200 * 200)
				attack = true;

			if (distance > 250 * 250)
				attack = false;

			if (attack)
			{
				NPC.velocity.X = .008f * NPC.direction;

				if (frame == 3 && timer == 0)
				{
					SoundEngine.PlaySound(SoundID.Item106, NPC.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 7f;
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (NPC.direction * 12), NPC.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<ToxicFlaskHostile>(), 13, 1, Main.myPlayer, 0, 0);
						for (int k = 0; k < 11; k++)
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, (float)direction.X + A, (float)direction.Y + B, 0, default, .61f);
						Main.projectile[p].hostile = true;
					}
					timer++;
				}
				timer++;

				if (timer >= 12)
				{
					frame++;
					timer = 0;
				}

				if (frame >= 5)
					frame = 0;

				if (target.position.X > NPC.position.X)
					NPC.direction = 1;
				else
					NPC.direction = -1;
			}
			else
			{
				NPC.aiStyle = 3;
				AIType = NPCID.Skeleton;
				timer++;
				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}

				if (frame >= 11)
					frame = 5;

				if (frame < 5)
					frame = 5;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedPlantBoss)
				return spawnInfo.Player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<PlagueDoctor>()) < 1 ? 0.0015f : 0f;
			return spawnInfo.Player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<PlagueDoctor>()) < 1 ? 0.05f : 0f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/PlagueDoctor/PlagueDoctor_Glow").Value);
		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;
	}
}