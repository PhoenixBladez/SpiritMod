using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.FallenAngel
{
	public class FallenAngel : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fallen Angel");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingFish];
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 60;
			NPC.damage = 50;
			NPC.defense = 31;
			NPC.lifeMax = 3200;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60f;
			NPC.knockBackResist = 0.03f;
			NPC.aiStyle = 44;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.stepSpeed = 2f;
			NPC.rarity = 3;

			AIType = NPCID.FlyingFish;
			AnimationType = NPCID.FlyingFish;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("Banished from the heavens, this rogue cherub takes his frustrations out on the mortal beings of this realm."),
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Sky && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<FallenAngel>()) ? 0.013f : 0f;

		public override void AI()
		{
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.091f, 0.24f, .24f);

			NPC.rotation = NPC.velocity.X * .009f;
			NPC.ai[0]++;
			NPC.ai[1] += 0.04f;

			if (NPC.ai[0] == 100 || NPC.ai[0] == 240 || NPC.ai[0] == 360 || NPC.ai[0] == 620)
			{
				SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
				Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * new Vector2(Main.rand.Next(8, 10), Main.rand.Next(8, 10));
				NPC.velocity = direction * 0.96f;
			}

			if (NPC.ai[0] >= 680)
			{
				SoundEngine.PlaySound(SoundID.Item109, NPC.Center);
				DustHelper.DrawStar(NPC.Center, DustID.GoldCoin, pointAmount: 5, mainSize: 2.25f * 2.33f, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);

				for (int i = 0; i < 5; i++)
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-8, 8), ModContent.ProjectileType<ShootingStarHostile>(), 30, 1, Main.myPlayer, 0, 0);

				NPC.ai[0] = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
			float sineAdd = (float)Math.Sin(NPC.ai[1]) + 3;

			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[49].Value, (NPC.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);

			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			if (NPC.velocity != Vector2.Zero)
			{
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * (((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return false;
		}

		public override bool PreKill()
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/FallenAngel/FallenAngel_Glow").Value, screenPos);

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarPiece>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessory.FallenAngel>(), 5));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				for (int i = 0; i < 3; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);

			for (int k = 0; k < 2; k++)
			{
				Vector2 vel = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * (Main.rand.Next(50, 100) * 0.04f);
				int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GoldCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = vel;
				Main.dust[dust].position = NPC.Center - (Vector2.Normalize(vel) * 34f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.Cursed, 300);
		}
	}
}