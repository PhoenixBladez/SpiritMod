using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.CrystalDrifter
{
	public class CrystalDrifter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Drifter");
			Main.npcFrameCount[npc.type] = 12;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 88;
			npc.damage = 27;
			npc.defense = 17;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCDeath15;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.value = 200f;
			npc.knockBackResist = 0f;
			npc.alpha = 100;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			npc.aiStyle = -1;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.CrystalDrifterBanner>();
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.08f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			npc.spriteDirection = -npc.direction;
			npc.spriteDirection = npc.direction;
			Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), .5f, .36f, .14f);

			Player target = Main.player[npc.target];
			MyPlayer modPlayer = target.GetSpiritPlayer();

			float distance = npc.DistanceSQ(target.Center);
			if (distance < 500 * 500 && Main.myPlayer == target.whoAmI)
			{
				target.AddBuff(BuffID.WindPushed, 90);

				if (Main.netMode != NetmodeID.MultiplayerClient)
					modPlayer.windEffect2 = true;
			}

			npc.ai[0]++;

			const float VelMagnitude = 5f;

			Vector2 vel = Main.player[npc.target].Center - npc.Center + new Vector2(0f, Main.rand.NextFloat(-200f, -150f));
			float length = vel.Length();

			Vector2 desiredVelocity;
			if (length < 20)
				desiredVelocity = npc.velocity;
			else if (length < 40)
				desiredVelocity = Vector2.Normalize(vel) * (VelMagnitude * 0.35f);
			else if (length < 80)
				desiredVelocity = Vector2.Normalize(vel) * (VelMagnitude * 0.65f);
			else
				desiredVelocity = Vector2.Normalize(vel) * VelMagnitude;

			npc.SimpleFlyMovement(desiredVelocity, 0.08f);
			npc.rotation = npc.velocity.X * 0.1f;

			if (npc.ai[0] >= 90 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * new Vector2(5f, 3f);
				int damage = Main.expertMode ? 12 : 18;
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FrostOrbiterHostile>(), damage, 0.0f, Main.myPlayer, 0.0f, npc.whoAmI);
				Main.projectile[p].hostile = true;

				npc.ai[0] = 0;
				npc.netUpdate = true;
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++)
			{
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * (((npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneOverworldHeight && spawnInfo.player.ZoneSnow && Main.raining && !spawnInfo.playerSafe && !NPC.AnyNPCs(ModContent.NPCType<CrystalDrifter>()) && NPC.downedBoss3 ? 0.09f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 51);

			for (int k = 0; k < 20; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueCrystalShard, hitDirection * 2f, -1f, 0, default, 1f);

			if (npc.life <= 0)
			{
				for (int i = 1; i < 6; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter" + i), .5f);

				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 41);
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 5f;
					dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1f);
					Main.dust[dust].velocity *= 2f;
				}
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 1)
				target.AddBuff(BuffID.Frostburn, 150);
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CryoliteOre>(), Main.rand.Next(8, 14) + 1);

			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.IceDeityQuest>().IsActive)
				Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.IceDeityShard2>());
		}
	}
}