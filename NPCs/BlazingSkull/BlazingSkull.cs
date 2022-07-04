using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.SlagSet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Prim;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.BlazingSkull
{
	public class BlazingSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrathful Soul");
			Main.npcFrameCount[NPC.type] = 17;
			NPC.gfxOffY = 50;
			//NPCID.Sets.TrailCacheLength[npc.type] = 3;
			//NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		int frame = 0;
		public override void SetDefaults()
		{
			NPC.Size = new Vector2(54, 50);
			NPC.damage = 50;
			NPC.defense = 10;
			NPC.lifeMax = 125;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 300f;
			NPC.knockBackResist = 1f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.lavaImmune = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BlazingSkullBanner>();
		}
		Vector2 targetpos;

		const int rechargetime = 180;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.damage = 60;
		//public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(targetpos);

		//public override void ReceiveExtraAI(BinaryReader reader) => targetpos = reader.ReadVector2();

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneUnderworldHeight && NPC.downedBoss3 ? 0.04f : 0f;
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.ai[2] > rechargetime;
		public override void AI()
		{
			Lighting.AddLight(NPC.Center, Color.OrangeRed.ToVector3());

			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			NPC.spriteDirection = -NPC.direction;

			NPC.ai[0] = ((NPC.Distance(player.Center) < 800 || NPC.ai[2] > rechargetime)
				&& Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, 0, 0)
				&& player.active
				&& !player.dead) ? 1 : 0;

			if (NPC.ai[0] == 0)
			{
				IdleMovement();
				NPC.ai[1] = 0; //dash timer
				NPC.ai[2] = 0; //targetting timer
				NPC.ai[3] = 0;
			}
			else
			{ //set its target position as the player's center when it starts its enrage, then dash to it once per frame cycle. If it's close enough to the target position, explode and retarget

				if (NPC.ai[2] <= rechargetime)
				{ //todo: make target position only update on client side in multiplayer, then sync that to the server? since it seems like the server stores player positions differently
					IdleMovement();
					targetpos = player.Center;

					NPC.ai[2]++;

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
				}
				else
				{
					if (NPC.ai[3] == 0)
					{
						if (Main.netMode != NetmodeID.Server)
							SpiritMod.primitives.CreateTrail(new PrimFireTrail(NPC, new Color(255, 170, 0), 26));

						NPC.ai[3] = 1;
					}

					NPC.spriteDirection = -Math.Sign(NPC.velocity.X);

					if (Main.expertMode)
						targetpos = Vector2.Lerp(targetpos, player.Center, 0.03f);

					if (NPC.ai[1] == 0)
					{
						if (Main.netMode != NetmodeID.Server)
							SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/skullscrem") with { PitchVariance = 0.2f }, NPC.Center);

						NPC.velocity = NPC.DirectionTo(targetpos) * 14;
						NPC.ai[1]++;
					}
					if (Main.expertMode)
						NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(targetpos) * 14, 0.2f);
					if (frame > 10)
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.05f);

					if (NPC.Distance(targetpos) < 20)
					{
						SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
						int damage = (Main.expertMode) ? NPC.damage / 4 : NPC.damage / 2;

						for (int i = 0; i < 6; i++)
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2Square(-20, 20), Main.rand.NextVector2Circular(3, 3), 11);

						if (Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WrathBoom>(), damage, 1, Main.myPlayer);

						NPC.velocity = Vector2.Zero;
						NPC.ai[2] = 0;
						NPC.ai[3] = 0;
					}

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);

					UpdateFrame(20, 6, 16);
				}
			}
			CheckPlatform();
		}
		private void IdleMovement()
		{
			if (Main.rand.Next(10) == 0)
			{
				NPC.velocity = Main.rand.NextVector2Circular(0.75f, 0.75f);

				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
			}

			NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);
			UpdateFrame(10, 0, 5);
		}
		private void CheckPlatform()
		{
			bool onplatform = true;
			for (int i = (int)NPC.position.X; i < NPC.position.X + NPC.width; i += NPC.width / 4)
			{
				Tile tile = Framing.GetTileSafely(new Point((int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.TileType])
					onplatform = false;
			}
			if (onplatform)
				NPC.noTileCollide = true;
			else
				NPC.noTileCollide = false;
		}
		private void UpdateFrame(int framespersecond, int minframe, int maxframe)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= (60 / framespersecond))
			{
				frame++;
				NPC.frameCounter = 0;
			}
			if (frame >= maxframe || frame < minframe)
			{
				frame = minframe;
				NPC.ai[1] = 0;
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/BlazingSkull/BlazingSkull_glow").Value,
				NPC.Center - Main.screenPosition - new Vector2(0, NPC.gfxOffY + 16),
				NPC.frame,
				Color.White,
				NPC.rotation,
				NPC.frame.Size() / 2,
				NPC.scale,
				(NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
				0);

			if (NPC.ai[0] == 1)
			{
				Texture2D target = Mod.Assets.Request<Texture2D>("NPCs/BlazingSkull/TargetX").Value;
				spriteBatch.Draw(target, targetpos - Main.screenPosition, target.Bounds, Color.White * 0.75f * (NPC.ai[2] / rechargetime), 0, target.Size() / 2, 1.5f, SpriteEffects.None, 0);
			}
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;
		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 180);

		public override void OnKill()
		{
			for (int i = 1; i <= 2; i++)
				Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/WrathfulSoul/wrathfulskullgore" + i.ToString()).Type);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CarvedRock>(), 1, 2, 3));
	}
}