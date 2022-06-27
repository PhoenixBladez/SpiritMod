using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.SeraphSet;
using SpiritMod.Items.Sets.MagicMisc.AstralClock;
using SpiritMod.Items.Weapon.Summon;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;

namespace SpiritMod.NPCs.BlueMoon.Bloomshroom
{
	public class Bloomshroom : ModNPC
	{
		bool attack = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomshroom");
			Main.npcFrameCount[NPC.type] = 12;
		}

		public override void SetDefaults()
		{
			NPC.width = 50;
			NPC.height = 54;
			NPC.damage = 29;
			NPC.defense = 16;
			NPC.lifeMax = 600;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath16;
			NPC.buffImmune[ModContent.BuffType<StarFlame>()] = true;
			NPC.value = 600f;
			NPC.knockBackResist = .35f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BloomshroomBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Obsidian, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Glumshroom/Glumshroom1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Glumshroom/Glumshroom2").Type, 1f);
			}
		}


		int frame = 0;
		int timer = 0;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));

			if (distance < 360)
				attack = true;

			if (distance > 380)
				attack = false;

			if (attack)
			{
				NPC.velocity.X = .008f * NPC.direction;

				if (frame == 9 && timer == 0)
				{
					SoundEngine.PlaySound(SoundID.Item, NPC.Center, 95);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y - 10, 0, -4, ModContent.ProjectileType<BloomshroomHostile>(), 31, 0);
						Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y - 10, 6f, -4, ModContent.ProjectileType<BloomshroomHostile>(), 31, 0);

						if (Main.rand.Next(3) == 0)
							Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y - 10, -6f, -4, ModContent.ProjectileType<BloomshroomHostile>(), 25, 0);

						timer++;
					}
				}

				timer++;

				if (timer >= 12)
				{
					frame++;
					timer = 0;
				}

				if (frame > 11)
					frame = 7;

				if (frame < 7)
					frame = 7;
				if (target.position.X > NPC.position.X)
					NPC.direction = 1;
				else
					NPC.direction = -1;
			}
			else
			{
				NPC.aiStyle = 26;
				AIType = NPCID.Skeleton;
				timer++;
				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}

				if (frame > 6)
					frame = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Bloomshroom>()) < 2 && spawnInfo.Player.ZoneOverworldHeight ? 1f : 0f;
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.GetTexture("NPCs/BlueMoon/Bloomshroom/Bloomshroom_Glow"));
		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void OnKill()
		{
			if (Main.rand.NextBool(5))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<MoonStone>());
			if (Main.rand.NextBool(100))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<StopWatch>());
			if (Main.rand.NextBool(20))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<GloomgusStaff>());
		}
	}
}