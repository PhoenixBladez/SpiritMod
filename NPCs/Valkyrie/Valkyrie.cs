using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Valkyrie
{
	public class Valkyrie : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 88;
			npc.height = 60;
			npc.damage = 20;
			npc.defense = 15;
			npc.lifeMax = 120;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 360f;
			npc.rarity = 2;
			npc.knockBackResist = .45f;
			npc.aiStyle = 14;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.ValkyrieBanner>();
		}

		int aiTimer;
		bool trailing;

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Feather, Main.rand.Next(2, 4));

			if (Main.rand.Next(10) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ValkyrieSpear>());
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GiantHarpyFeather);
		}

		public override void AI()
		{
			aiTimer++;
			if (aiTimer == 100 || aiTimer == 480)
			{
				Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);

				var direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * Main.rand.Next(6, 9);
				npc.velocity = direction * 0.98f;
			}

			trailing = aiTimer >= 100 && aiTimer <= 120 || aiTimer >= 480 && aiTimer <= 500;

			if (aiTimer >= 120 && aiTimer <= 300)
			{
				int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.PortalBolt);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;

				Vector2 dustSpeed = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101)));
				dustSpeed *= (Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = dustSpeed;
				Main.dust[dust].position = npc.Center - Vector2.Normalize(dustSpeed) * 34f;
			}

			if (aiTimer == 300)
			{
				Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 9f;
					int damage = Main.expertMode ? 9 : 15;

					int amountOfProjectiles = Main.rand.Next(2, 4);
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = Main.rand.Next(-150, 150) * 0.01f;
						float B = Main.rand.Next(-150, 150) * 0.01f;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<ValkyrieSpearHostile>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}

			if (aiTimer >= 500)
				aiTimer = 0;
		}

		public override void FindFrame(int frameHeight)
		{
			if (aiTimer == 270)
				npc.frameCounter = 0;

			if (aiTimer < 270 || aiTimer > 330)
			{
				npc.frameCounter += 0.15f;
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			else
			{
				npc.frameCounter += 0.0666f;
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter + 4;
				npc.frame.Y = frame * frameHeight;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Valkyrie>()) || !spawnInfo.sky)
				return 0;
			if (QuestManager.GetQuest<SlayerQuestValkyrie>().IsActive)
				return 0.15f;
			return 0.09f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie3"), 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			if (trailing)
			{
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++)
				{
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * ((npc.oldPos.Length - k) / (float)npc.oldPos.Length / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 1)
				target.AddBuff(BuffID.Bleeding, 300);
		}
	}
}