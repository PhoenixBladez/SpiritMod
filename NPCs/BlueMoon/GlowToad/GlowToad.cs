using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;
using SpiritMod.Buffs;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.BlueMoon.GlowToad
{
	public class GlowToad : ModNPC
	{
		//TODO:
		//Get animation
		//smoother head rotation

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glow Toad");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 64;
			NPC.height = 50;
			NPC.damage = 100;
			NPC.defense = 50;
			NPC.lifeMax = 1080;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = 600f;
			NPC.buffImmune[ModContent.BuffType<StarFlame>()] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.knockBackResist = 0.5f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("Psychedelic fungus grows upon the back of this toad. They are lost in delirium, as they find themselves snacking on it frequently."),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, hitDirection, -1f, 1, default, .81f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VenomStaff, hitDirection, -1f, 1, default, .51f);
			}
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 11; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, hitDirection, -1f, 1, default, .81f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VenomStaff, hitDirection, -1f, 1, default, .71f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GlowToad1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GlowToad2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GlowToad3").Type, 1f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<GlowToad>()) < 4 && spawnInfo.Player.ZoneOverworldHeight ? .6f : 0f;
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(tongueOut);
		public override void ReceiveExtraAI(BinaryReader reader) => tongueOut = reader.ReadBoolean();

		bool tongueOut = false;
		bool mouthOpen = false;
		float headRotation;
		int tongueCooldown = 300;
		int tongueProj = -1;
		Vector2 offset;

		private void tongueStuff(Player player, Vector2 dir)
		{
			offset = NPC.Center;
			offset.X += NPC.direction * 15;
			offset.Y += dir.Y * 12;
			tongueCooldown--;
			if (tongueCooldown < 0 && NPC.velocity.Y == 0)
			{
				tongueOut = true;
				tongueCooldown = 300;
			}
			mouthOpen = tongueOut || tongueCooldown < 60;
			if (tongueOut)
			{
				if (tongueProj == -1)
					tongueProj = Projectile.NewProjectile(NPC.GetSource_FromAI(), offset + dir * 5, dir * 30, ModContent.ProjectileType<GlowToadTongue>(), 100, 0, player.whoAmI, NPC.whoAmI, offset.Y);
				else if (!Main.projectile[tongueProj].active)
				{
					tongueProj = -1;
					tongueOut = false;
				}
			}
		}

		int direction;
		int jumpCounter = 0;

		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			Vector2 dir = player.Center - NPC.Center;
			dir.Normalize();
			if (!NPC.collideY)
			{
				NPC.velocity.X *= 1.045f;
			}
			if (!tongueOut && tongueCooldown >= 60 && (NPC.velocity.Y == 0 || NPC.collideY))
			{
				//movement
				if (jumpCounter >= 45)
				{
					NPC.velocity.Y = -7;
					NPC.velocity.X = NPC.direction * 10;
					jumpCounter = 0;
				}
				jumpCounter++;
			}
			if (!tongueOut)
			{
				direction = NPC.direction;
				headRotation = dir.ToRotation();
				if (NPC.direction == -1)
					headRotation += 3.14f;
			}
			else
				NPC.direction = direction;

			tongueStuff(player, dir);
		}
		//int tongueDirection = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			Texture2D headTexture = ModContent.Request<Texture2D>("SpiritMod/NPCs/BlueMoon/GlowToad/GlowToad_Head", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 headOffset = new Vector2(NPC.direction == -1 ? 25 : headTexture.Width - 25, 20);
			spriteBatch.Draw(headTexture, NPC.position - screenPos + headOffset, new Rectangle(0, mouthOpen ? 52 : 0, headTexture.Width, headTexture.Height / 2), drawColor, headRotation, headOffset, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D glow = ModContent.Request<Texture2D>("SpiritMod/NPCs/BlueMoon/GlowToad/GlowToad_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Texture2D headGlow = ModContent.Request<Texture2D>("SpiritMod/NPCs/BlueMoon/GlowToad/GlowToad_Head", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw(glow, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			Vector2 headOffset = new Vector2(NPC.direction == -1 ? 25 : headGlow.Width - 25, 20);
			spriteBatch.Draw(headGlow, NPC.position - screenPos + headOffset, new Rectangle(0, mouthOpen ? 52 : 0, headGlow.Width, headGlow.Height / 2), Color.White, headRotation, headOffset, NPC.scale, effects, 0);
		}
	}
	public class GlowToadTongue : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glow Tongue");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.damage = 0;
			Projectile.timeLeft = 100;
		}
		int speed = -30;
		Vector2 origin = Vector2.Zero;
		Vector2 dir;
		public override void AI()
		{
			NPC parent = Main.npc[(int)Projectile.ai[0]];
			if (speed > 0 && Projectile.Hitbox.Intersects(parent.Hitbox))
				Projectile.active = false;

			origin = parent.Center;
			origin.X += parent.direction * 15;
			origin.Y = Projectile.ai[1];
			dir = origin - Projectile.Center;
			dir.Normalize();
			Projectile.rotation = dir.ToRotation() - 1.57f;
			Projectile.velocity = dir * speed;
			speed += 1;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawChain(Projectile.whoAmI, origin, "SpiritMod/NPCs/BlueMoon/GlowToad/GlowToadTongue_Chain", white: true);
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, Color.White);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SpiritMod/NPCs/BlueMoon/GlowToad/GlowToadTongue_ChainEnd", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, origin - Main.screenPosition, null, Color.White, dir.ToRotation() - 1.57f, new Vector2(7, 5), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}