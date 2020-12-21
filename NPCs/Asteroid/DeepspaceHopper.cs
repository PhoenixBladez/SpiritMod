using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.AstronautVanity;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Hostile;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class DeepspaceHopper : ModNPC
	{
		private const int TELEPORT_DISTANCE = 300;

		private int Timer {
			get => (int)npc.ai[1];
			set => npc.ai[1] = value;
		}

		private Vector2 AngleToPlayer {
			get => new Vector2(npc.localAI[0], npc.localAI[1]);
			set {
				npc.localAI[0] = value.X;
				npc.localAI[1] = value.Y;
			}
		}

		private AIState State {
			get => (AIState)(int)npc.ai[0];
			set {
				npc.ai[0] = (int)value;
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					npc.netUpdate = true;
				}
			}
		}

		private enum AIState
		{
			STANDBY,
			AIMING,
			SHOOTING,
			TELEPORT_FAIL,
			TELEPORT_SUCCESS
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shockhopper");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 6;
			npc.lifeMax = 55;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 130f;
			npc.knockBackResist = 1f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.noTileCollide = false;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.ShockhopperBanner>();

			// start with 5 seconds to the first teleport
			Timer = 300;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 226, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, 226, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper4"));
				for (int i = 0; i < 15; i++) {
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default, 0.4f);
					dust.noGravity = true;
					dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					if (dust.position != npc.Center) {
						dust.velocity = npc.DirectionTo(dust.position) * 3f;
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Asteroid/DeepspaceHopper_Glow"));
			}
		}

		public override bool PreAI()
		{
			npc.velocity.X = 0;
			npc.velocity.Y = 0;
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];

			Timer++;
			// Update NPC state
			// Standby mode; doing nothing, just looking at player
			if(Timer < 100) State = AIState.STANDBY;
			// Aiming mode; preparing to shoot, look angle locked
			else if(Timer == 100) State = AIState.AIMING;
			// Shooting mode; shoots the laser
			else if(Timer == 130) State = AIState.SHOOTING;
			// Teleport mode; performs a teleport
			else if(Timer >= 280) {
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 angle = Vector2.UnitX.RotateRandom(Math.PI * 2);
					npc.position.X = player.Center.X + (int)(TELEPORT_DISTANCE * angle.X);
					npc.position.Y = player.Center.Y + (int)(TELEPORT_DISTANCE * angle.Y);
					npc.netUpdate = true;
					if(Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active()) {
						State = AIState.TELEPORT_FAIL;
					} else {
						State = AIState.TELEPORT_SUCCESS;
					}
				}
			}

			// Look at the player
			if(State == AIState.STANDBY) {
				npc.rotation = npc.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
			}

			if(State == AIState.TELEPORT_FAIL) {
				npc.alpha = 255;
			}

			// When we succeed at performing the teleport
			if(State == AIState.TELEPORT_SUCCESS) {
				Timer = 0;
				npc.alpha = 0;
				if(Main.netMode != NetmodeID.Server) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
					for(int i = 0; i < 50; i++) {
						Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default, 0.4f);
						dust.noGravity = true;
						dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						if(dust.position != npc.Center) {
							dust.velocity = npc.DirectionTo(dust.position) * 3f;
						}
					}
				}
				State = AIState.STANDBY;
			}

			// Lock in the targeting angle
			if (State == AIState.AIMING && Timer == 100) {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				var offset = player.Center - npc.Center;
				offset.Normalize();
				AngleToPlayer = offset;
			}

			// Spawn dust while waiting to shoot
			if (Main.netMode != NetmodeID.Server && State == AIState.AIMING) {
				Dust dust = Dust.NewDustDirect(npc.Center, npc.width, npc.height, 226);
				dust.velocity *= -1f;
				dust.scale *= .8f;
				dust.noGravity = true;
				Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
				dust.velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				dust.position = npc.Center - vector2_3;
			}

			// Fire the laser
			if(State == AIState.SHOOTING) {
				// Play sound on client, fire projectile on server
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 91);
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					Projectile.NewProjectile(npc.Center, AngleToPlayer * 30, ModContent.ProjectileType<HopperLaser>(), 19, 1, Main.myPlayer);
				}
				State = AIState.STANDBY;
			}

			// Idle dusts while we're not failing a teleport
			if(Main.netMode != NetmodeID.Server && State != AIState.TELEPORT_FAIL) {
				if(Main.rand.NextBool()) {
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(npc.Center.X - 10, npc.Center.Y).RotatedBy(npc.rotation, npc.Center);
					var dust = Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
					dust.noGravity = true;
				}
				if(Main.rand.NextBool()) {
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(npc.Center.X + 10, npc.Center.Y).RotatedBy(npc.rotation, npc.Center);
					var dust = Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
					dust.noGravity = true;
				}
			}

			return false;
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(40)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GateStaff>(), 1);
			}
			if (Main.rand.NextBool(400)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityModulator>());
			}
			int[] lootTable = {
				ModContent.ItemType<AstronautLegs>(),
				ModContent.ItemType<AstronautHelm>(),
				ModContent.ItemType<AstronautBody>()
			};
			if (Main.rand.NextBool(40)) {
				int loot = Main.rand.Next(lootTable.Length);
				npc.DropItem(lootTable[loot]);
			}
		}
	}
}
