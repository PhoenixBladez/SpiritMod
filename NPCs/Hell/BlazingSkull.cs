using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using System.IO;
using Terraria.Audio;
using System;
using SpiritMod.Projectiles;
using SpiritMod.Gores;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.Hell
{
	public class BlazingSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrathful Soul");
			Main.npcFrameCount[npc.type] = 17;
			npc.gfxOffY = 50;
			//NPCID.Sets.TrailCacheLength[npc.type] = 3;
			//NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		int frame = 0;
		public override void SetDefaults()
		{
			npc.Size = new Vector2(54, 50);
			npc.damage = 50;
			npc.defense = 6;
			npc.lifeMax = 90;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 300f;
			npc.knockBackResist = 1f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.lavaImmune = true;
			//banner = npc.type;
			//bannerItem = ModContent.ItemType<Items.Banners.GluttonousDevourerBanner>();
		}
		Vector2 targetpos;

		const int rechargetime = 180;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.damage = 50;
		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(targetpos);
		public override void ReceiveExtraAI(BinaryReader reader) => targetpos = reader.ReadVector2();
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneUnderworldHeight && NPC.downedBoss3 ? 0.04f : 0f;
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => npc.ai[2] > rechargetime;
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.1f, 0.04f, 0.02f);

			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = -npc.direction;

			npc.ai[0] = ((npc.Distance(player.Center) < 800 || npc.ai[2] > rechargetime)
				&& Collision.CanHit(npc.position, npc.width, npc.height, player.position, 0, 0)
				&& player.active
				&& !player.dead) ? 1 : 0;

			if (npc.ai[0] == 0) {
				IdleMovement();
				npc.ai[1] = 0; //dash timer
				npc.ai[2] = 0; //targetting timer
				npc.ai[3] = 0;
			}
			else { //set its target position as the player's center when it starts its enrage, then dash to it once per frame cycle. If it's close enough to the target position, explode and retarget

				if (npc.ai[2] <= rechargetime) {
					IdleMovement();
					targetpos = player.Center;
					npc.ai[2]++;
				}
				else {
					if (npc.ai[3] == 0) {
						SpiritMod.primitives.CreateTrail(new PrimFireTrail(npc, new Color(255, 170, 0), 26));
						npc.ai[3] = 1;
					}

					npc.spriteDirection = -Math.Sign(npc.velocity.X);

					if(Main.rand.Next(4) == 0)
						Gore.NewGorePerfect(npc.Center + Main.rand.NextVector2Circular(10, 10), npc.velocity.RotatedByRandom(MathHelper.Pi / 8) / 2, mod.GetGoreSlot("Gores/FireTrail"), Main.rand.NextFloat(0.7f, 1.2f));

					if (Main.expertMode) {
						targetpos = Vector2.Lerp(targetpos, player.Center, 0.03f);
					}
					if (npc.ai[1] == 0) {
						Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/skullscrem").WithPitchVariance(0.2f), npc.Center);
						npc.velocity = npc.DirectionTo(targetpos) * 14;
						npc.ai[1]++;
					}
					if (Main.expertMode)
						npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(targetpos) * 14, 0.2f);
					if (frame > 10)
						npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.05f);

					if (npc.Distance(targetpos) < 20) {
						Main.PlaySound(SoundID.Item14, npc.Center);
						int damage = (Main.expertMode) ? npc.damage / 4 : npc.damage / 2;
						for (int i = 0; i < 6; i++) {
							Gore.NewGore(npc.Center + Main.rand.NextVector2Square(-20, 20), Main.rand.NextVector2Circular(3, 3), 11);
						}
						Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("WrathBoom"), damage, 1, Main.myPlayer);
						npc.velocity = Vector2.Zero;
						npc.ai[2] = 0;
						npc.ai[3] = 0;
					}

					UpdateFrame(20, 6, 16);
				}
			}
			CheckPlatform();
		}
		private void IdleMovement()
		{
			if (Main.rand.Next(10) == 0)
				npc.velocity = Main.rand.NextVector2Circular(0.75f, 0.75f);
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);
			UpdateFrame(10, 0, 5);
		}
		private void CheckPlatform()
		{
			bool onplatform = true;
			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4) {
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
					onplatform = false;
			}
			if (onplatform)
				npc.noTileCollide = true;
			else
				npc.noTileCollide = false;
		}
		private void UpdateFrame(int framespersecond, int minframe, int maxframe)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= (60/framespersecond)) {
				frame++;
				npc.frameCounter = 0;
			}
			if (frame >= maxframe || frame < minframe) {
				frame = minframe;
				npc.ai[1] = 0;
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(mod.GetTexture("NPCs/Hell/BlazingSkull_glow"), 
				npc.Center - Main.screenPosition - new Vector2(0, npc.gfxOffY + 16), 
				npc.frame, 
				Color.White, 
				npc.rotation, 
				npc.frame.Size() / 2, 
				npc.scale,
				(npc.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
				0);

			if(npc.ai[0] == 1) {
				Texture2D target = mod.GetTexture("NPCs/Hell/TargetX");
				spriteBatch.Draw(target, targetpos - Main.screenPosition, target.Bounds, Color.White * 0.75f * (npc.ai[2] / rechargetime), 0, target.Size()/2, 1.5f, SpriteEffects.None, 0);
			}
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 180);

		public override void NPCLoot() => Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CarvedRock>(), Main.rand.Next(2) + 2);
	}
}
