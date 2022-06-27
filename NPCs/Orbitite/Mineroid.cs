using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Weapon.Summon;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Orbitite
{
	public class Mineroid : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbitite");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 25;
			NPC.defense = 15;
			NPC.lifeMax = 70;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 110f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 44;
			AIType = NPCID.FlyingAntlion;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.OrbititeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneMeteor && spawnInfo.SpawnTileY < Main.rockLayer && NPC.downedBoss2 ? 0.15f : 0f;

		public override void OnKill()
		{
			if (Main.rand.Next(20) == 0) 
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<OrbiterStaff>());

			if (Main.rand.Next(1) == 400) {
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<GravityModulator>());
			}

			string[] lootTable = { "AstronautLegs", "AstronautHelm", "AstronautBody" };
			if (Main.rand.Next(40) == 0) {
				int loot = Main.rand.Next(lootTable.Length);
				NPC.DropItem(Mod.Find<ModItem>(lootTable[loot]).Type);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.GetTexture("NPCs/Orbitite/Mineroid_Glow"));

		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastHurt, NPC.Center);
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CorruptionThorns, hitDirection, -1f, 0, default, .61f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Mineroid/Mineroid1").Type);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Mineroid/Mineroid2").Type);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Mineroid/Mineroid3").Type);
				Gore.NewGore(NPC.position, NPC.velocity, 61);
				Gore.NewGore(NPC.position, NPC.velocity, 62);
				Gore.NewGore(NPC.position, NPC.velocity, 63);
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.CorruptionThorns, 0f, 0f, 100, default, 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		int timer = 0;

		public override void AI()
		{
			timer++;
			if (timer >= 90 && Main.netMode != NetmodeID.MultiplayerClient) {
				bool expertMode = Main.expertMode;
				int damage = expertMode ? 10 : 16;
				Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				int p = Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, vector2_2.X, vector2_2.Y, ModContent.ProjectileType<MeteorShardHostile1>(), damage, 0.0f, Main.myPlayer, 0.0f, (float)NPC.whoAmI);
				Main.projectile[p].hostile = true;
				timer = 0;
				NPC.netUpdate = true;
			}

			NPC.spriteDirection = NPC.direction;
		}
	}
}
