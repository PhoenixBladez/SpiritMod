using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
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
        int timer = 0;
		public override bool PreAI()
		{
			npc.spriteDirection = -npc.direction;
			Player target = Main.player[npc.target];
			MyPlayer modPlayer = target.GetSpiritPlayer();
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 500) {
				{
					target.AddBuff(BuffID.WindPushed, 90);
					modPlayer.windEffect2 = true;
				}
			}
			timer++;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .5f, .36f, .14f);
			npc.spriteDirection = npc.direction;
			float num1 = 5f;
			float moveSpeed = 0.08f;
			npc.TargetClosest(true);
			Vector2 vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(0.0f, Main.rand.NextFloat(-200f, -150f));
			float num2 = vector2_1.Length();
			Vector2 desiredVelocity;
			if ((double)num2 < 20.0)
				desiredVelocity = npc.velocity;
			else if ((double)num2 < 40.0) {
				vector2_1.Normalize();
				desiredVelocity = vector2_1 * (num1 * 0.35f);
			}
			else if ((double)num2 < 80.0) {
				vector2_1.Normalize();
				desiredVelocity = vector2_1 * (num1 * 0.65f);
			}
			else {
				vector2_1.Normalize();
				desiredVelocity = vector2_1 * num1;
			}
			npc.SimpleFlyMovement(desiredVelocity, moveSpeed);
			npc.rotation = npc.velocity.X * 0.1f;
			if (timer >= 90 && Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				bool expertMode = Main.expertMode;
				int damage = expertMode ? 12 : 18;
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector2_2.X, vector2_2.Y, ModContent.ProjectileType<FrostOrbiterHostile>(), damage, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
				Main.projectile[p].hostile = true;
				timer = 0;
			}
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
			for (int k = 0; k < npc.oldPos.Length; k++) {
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
			return true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneOverworldHeight && spawnInfo.player.ZoneSnow && Main.raining && !NPC.AnyNPCs(ModContent.NPCType<CrystalDrifter>()) && NPC.downedBoss3 ? 0.09f : 0f;
		}


		public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 51);
			for (int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 68, hitDirection * 2f, -1f, 0, default(Color), 1f);
			}

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter1"), .5f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter2"), .5f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter3"), .5f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter4"), .5f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter5"), .5f);
				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 41);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].velocity *= 2f;
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
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CryoliteOre>(), Main.rand.Next(4, 9) + 1);
		}
	}
}