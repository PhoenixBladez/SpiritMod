using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.FallenAngel
{
	public class FallenAngel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fallen Angel");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingFish];
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 60;
			npc.damage = 50;
			npc.defense = 31;
			npc.lifeMax = 3200;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.CursedInferno] = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = 0.03f;
			npc.aiStyle = 44;
			npc.noGravity = true;
			npc.noTileCollide = true;
			aiType = NPCID.FlyingFish;
			animationType = NPCID.FlyingFish;
			npc.stepSpeed = 2f;
			npc.rarity = 3;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.sky && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<FallenAngel>()) ? 0.013f : 0f;
		}
		int aiTimer;
		float alphaCounter;
		public override void AI()
		{
			alphaCounter += 0.04f;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.091f, 0.24f, .24f);
			npc.rotation = npc.velocity.X * .009f;
			aiTimer++;
			if (aiTimer == 100 || aiTimer == 240 || aiTimer == 360 || aiTimer == 620) {
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
				direction.X = direction.X * Main.rand.Next(8, 10);
				direction.Y = direction.Y * Main.rand.Next(8, 10);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
				npc.velocity *= 0.96f;
			}
			if (aiTimer >= 680) {
				Main.PlaySound(SoundID.Item, npc.Center, 109);
				float ScaleMult = 2.33f;
				DustHelper.DrawStar(new Vector2(npc.Center.X, npc.Center.Y), DustID.GoldCoin, pointAmount: 5, mainSize: 2.25f * ScaleMult, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);

				for (int i = 0; i < 5; i++) {
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-8, 8), ModContent.ProjectileType<ShootingStarHostile>(), 30, 1, Main.myPlayer, 0, 0);
				}
				aiTimer = 0;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);

			{
				float sineAdd = (float)Math.Sin(alphaCounter) + 3;
				Vector2 drawPos1 = npc.Center - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
			if (npc.velocity != Vector2.Zero) {
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(drawColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return false;
		}
        public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/FallenAngel/FallenAngel_Glow"));
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StarPiece>(), Main.rand.Next(1, 3));

			if (Main.rand.Next(100) == 20) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.FallenAngel>());
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
			}
			for (int k = 0; k < 2; k++) {
				int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.GoldCoin);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[dust].position = npc.Center - vector2_3;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.Cursed, 300);
		}
	}
}
