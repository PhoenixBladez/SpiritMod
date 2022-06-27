using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.MarbleSet;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.GladiatorSpirit
{
	public class GladiatorSpirit : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gladiator Spirit");
			Main.npcFrameCount[NPC.type] = 8;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 56;
			NPC.damage = 30;
			NPC.defense = 9;
			NPC.lifeMax = 80;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.value = 220f;
			NPC.knockBackResist = .40f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = 22;
			AIType = NPCID.Wraith;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.GladiatorSpiritBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.SpawnTileX;
			int y = spawnInfo.SpawnTileY;
			return spawnInfo.Player.GetSpiritPlayer().ZoneMarble && spawnInfo.SpawnTileY > Main.rockLayer && NPC.downedBoss2 ? 0.135f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 3; ++i)
					Gore.NewGore(NPC.position, NPC.velocity, 99);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/GladSpirit/GladSpirit1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/GladSpirit/GladSpirit2").Type, 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		bool reflectPhase;
		int reflectTimer;

		public override void AI()
		{
			NPC.spriteDirection = -NPC.direction;
			reflectTimer++;

			if (reflectTimer == 720)
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse, NPC.Center);

			if (reflectTimer > 720)
				reflectPhase = true;
			else
				reflectPhase = false;

			if (reflectTimer >= 1000)
				reflectTimer = 0;

			if (reflectPhase)
			{
				NPC.velocity = Vector2.Zero;
				NPC.defense = 9999;
				Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)NPC.height, (float)NPC.height) * NPC.scale * 1.85f / 2f;
				int index = Dust.NewDust(NPC.Center + vector2, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index].position = NPC.Center + vector2;
				Main.dust[index].velocity = Vector2.Zero;
			}
			else
				NPC.defense = 9;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			{
				var effects = NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
								 lightColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
				{
					Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
					for (int k = 0; k < NPC.oldPos.Length; k++)
					{
						Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
						Color color = NPC.GetAlpha(lightColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
						spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
					}
				}
			}
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (reflectPhase && !projectile.minion && !projectile.sentry && !Main.player[projectile.owner].channel)
			{
				projectile.hostile = true;
				projectile.friendly = false;
				projectile.penetrate = 2;
				projectile.velocity.X *= -1f;
			}
		}

		public override void OnKill()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<MarbleChunk>());

			if (Main.rand.Next(120) == 2)
			{
				int[] lootTable = new int[] { 3187, 3188, 3189 };
				NPC.DropItem(lootTable[Main.rand.Next(3)]);
			}
		}
	}
}