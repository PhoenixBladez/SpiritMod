using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 29;
			npc.defense = 16;
			npc.lifeMax = 140;
			npc.HitSound = SoundID.NPCHit2;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.CursedInferno] = true;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 220f;
			npc.knockBackResist = .35f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.DarkAlchemistBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Obsidian, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.CursedTorch, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor3"), 1f);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(153) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);

			if (Main.rand.Next(75) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar);

			if (Main.rand.Next(100) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);

			if (Main.rand.Next(250) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);

			string[] lootTable = { "PlagueDoctorCowl", "PlagueDoctorRobe", "PlagueDoctorLegs" };
			if (Main.rand.Next(6) == 0)
			{
				int loot = Main.rand.Next(lootTable.Length);
				npc.DropItem(mod.ItemType(lootTable[loot]));
			}

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapon.Thrown.PlagueVial>(), Main.rand.Next(26, 43));
		}


		int frame = 0;
		int timer = 0;

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];

			float distance = npc.DistanceSQ(target.Center);

			if (distance < 200 * 200)
				attack = true;

			if (distance > 250 * 250)
				attack = false;

			if (attack)
			{
				npc.velocity.X = .008f * npc.direction;

				if (frame == 3 && timer == 0)
				{
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 106);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 7f;
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<ToxicFlaskHostile>(), 13, 1, Main.myPlayer, 0, 0);
						for (int k = 0; k < 11; k++)
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.CursedTorch, (float)direction.X + A, (float)direction.Y + B, 0, default, .61f);
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

				if (target.position.X > npc.position.X)
					npc.direction = 1;
				else
					npc.direction = -1;
			}
			else
			{
				npc.aiStyle = 3;
				aiType = NPCID.Skeleton;
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
				return spawnInfo.player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<PlagueDoctor>()) < 1 ? 0.0015f : 0f;
			return spawnInfo.player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<PlagueDoctor>()) < 1 ? 0.05f : 0f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/PlagueDoctor/PlagueDoctor_Glow"));
		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
	}
}