using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.SeraphSet;
using SpiritMod.Items.Sets.MagicMisc.AstralClock;
using SpiritMod.Items.Weapon.Summon;
using System;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 54;
			npc.damage = 29;
			npc.defense = 16;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath16;
			npc.buffImmune[ModContent.BuffType<StarFlame>()] = true;
			npc.value = 600f;
			npc.knockBackResist = .35f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.BloomshroomBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Obsidian, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.CursedTorch, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glumshroom/Glumshroom1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glumshroom/Glumshroom2"), 1f);
			}
		}


		int frame = 0;
		int timer = 0;

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));

			if (distance < 360)
				attack = true;

			if (distance > 380)
				attack = false;

			if (attack)
			{
				npc.velocity.X = .008f * npc.direction;

				if (frame == 9 && timer == 0)
				{
					Main.PlaySound(SoundID.Item, npc.Center, 95);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, 0, -4, ModContent.ProjectileType<BloomshroomHostile>(), 31, 0);
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, 6f, -4, ModContent.ProjectileType<BloomshroomHostile>(), 31, 0);

						if (Main.rand.Next(3) == 0)
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, -6f, -4, ModContent.ProjectileType<BloomshroomHostile>(), 25, 0);

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
				if (target.position.X > npc.position.X)
					npc.direction = 1;
				else
					npc.direction = -1;
			}
			else
			{
				npc.aiStyle = 26;
				aiType = NPCID.Skeleton;
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

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Bloomshroom>()) < 2 && spawnInfo.player.ZoneOverworldHeight ? 1f : 0f;
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BlueMoon/Bloomshroom/Bloomshroom_Glow"));
		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(5))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonStone>());
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StopWatch>());
			if (Main.rand.NextBool(20))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GloomgusStaff>());
		}
	}
}