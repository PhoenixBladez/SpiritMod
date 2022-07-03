using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Potion;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class MoonlightPreserver : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Preserver");
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 60;
			NPC.damage = 18;
			NPC.defense = 0;
			NPC.lifeMax = 80;
			NPC.HitSound = SoundID.NPCHit25;
			NPC.DeathSound = SoundID.NPCDeath28;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.value = 60f;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 64;
			NPC.scale = 1f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			AIType = NPCID.Firefly;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.MoonlightPreserverBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 15; k++)
			{
				Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.65f);
				d.noGravity = true;
			}

			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 5; i++)
					{
						int p = Projectile.NewProjectile(NPC.GetSource_OnHurt(null), NPC.Center.X + Main.rand.Next(-10, 10), NPC.Center.Y + Main.rand.Next(-10, 10), Main.rand.NextFloat(-1.1f, 1.1f), Main.rand.NextFloat(-1.1f, 1.1f), ModContent.ProjectileType<JellyfishOrbiter>(), NPCUtils.ToActualDamage(25, 1.5f), 0.0f, Main.myPlayer, 0.0f, (float)NPC.whoAmI);
						Main.projectile[p].scale = Main.rand.NextFloat(.5f, .8f);
						Main.projectile[p].timeLeft = Main.rand.Next(75, 95);
					}
				}
				for (int k = 0; k < 50; k++)
				{
					Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.95f);
					d.noGravity = true;
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

		bool chosen = false;
		int tremorItem;

		public override void AI()
		{
			if (!chosen)
			{
				if (Main.rand.NextBool(3))
				{
					if (Main.rand.NextBool(6))
						tremorItem = Main.rand.Next(new int[] { ModContent.ItemType<Items.Weapon.Summon.ElectricGun.ElectricGun>(), ModContent.ItemType<Items.Sets.MagicMisc.Arclash.ArcLash>() });
					else
						tremorItem = Main.rand.Next(new int[] { 19, 20, 21, 22, ModContent.ItemType<Items.Placeable.Tiles.SpaceJunkItem>(), 286, ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>() });
				}
				chosen = true;
				NPC.netUpdate = true;
			}

			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddOneFromOptions<Items.Weapon.Summon.ElectricGun.ElectricGun, Items.Sets.MagicMisc.Arclash.ArcLash>(3);
			npcLoot.AddOneFromOptions(3, 19, 20, 21, 22, ModContent.ItemType<Items.Placeable.Tiles.SpaceJunkItem>(), 286, ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>());
			npcLoot.AddCommon(ItemID.Gel, 1, 1, 3);
			npcLoot.AddCommon<MoonJelly>();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Color color9 = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));

			int num356 = tremorItem;
			float num355 = NPC.scale;
			float num354 = 22f * NPC.scale;
			float num353 = 18f * NPC.scale;
			float num352 = 18;
			float num351 = 34;
			if (num352 > num354)
			{
				num355 *= num354 / num352;
				num352 *= num355;
				num351 *= num355;
			}
			if (num351 > num353)
			{
				num355 *= num353 / num351;
				num352 *= num355;
				num351 *= num355;
			}
			float num348 = -1f;
			float num347 = 1f;
			int num346 = NPC.frame.Y / (TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type]);
			num347 -= (float)Math.Sin(num346) * Main.rand.NextFloat(.8f, 1.3f);
			num348 += (float)(Math.Sin(num346) * Main.rand.NextFloat(1.6f, 2.3f));
			float num349 = -1f + (float)(Math.Sin(num346) * -3f);
			float num343 = 0.2f;
			num343 -= 0.02f * (float)num346;

			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1").Value, new Vector2(NPC.Center.X - Main.screenPosition.X + num348, NPC.Center.Y - Main.screenPosition.Y + NPC.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1_Glow").Value, new Vector2(NPC.Center.X - Main.screenPosition.X + num348, NPC.Center.Y - Main.screenPosition.Y + NPC.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1").Value, new Vector2(NPC.Center.X + 5 - Main.screenPosition.X + num348, NPC.Center.Y + 7 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1_Glow").Value, new Vector2(NPC.Center.X + 5 - Main.screenPosition.X + num348, NPC.Center.Y + 7 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_2").Value, new Vector2(NPC.Center.X + 4 - Main.screenPosition.X + num349, NPC.Center.Y - 14 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_2_Glow").Value, new Vector2(NPC.Center.X + 4 - Main.screenPosition.X + num349, NPC.Center.Y - 14 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_5").Value, new Vector2(NPC.Center.X - 8 - Main.screenPosition.X + num349, NPC.Center.Y + 4 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_5_Glow").Value, new Vector2(NPC.Center.X - 8 - Main.screenPosition.X + num349, NPC.Center.Y + 4 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_4").Value, new Vector2(NPC.Center.X - 6 - Main.screenPosition.X + num348, NPC.Center.Y - 9 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_4_Glow").Value, new Vector2(NPC.Center.X - 6 - Main.screenPosition.X + num348, NPC.Center.Y - 9 - Main.screenPosition.Y + NPC.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TextureAssets.Item[tremorItem].Value, new Vector2(NPC.Center.X - Main.screenPosition.X + num348, NPC.Center.Y - Main.screenPosition.Y + NPC.gfxOffY + num347), null, color9, num343, new Vector2((float)(TextureAssets.Item[tremorItem].Value.Width / 2), (float)(TextureAssets.Item[tremorItem].Value.Height / 2)), num355 * 1.3f, SpriteEffects.None, 0f);

			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			const float Repeats = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num106 = 0f;

			Texture2D glow = Mod.Assets.Request<Texture2D>("NPCs/MoonjellyEvent/MoonlightPreserver_Glow").Value;
			Color color29 = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.LightBlue);
			for (int num103 = 0; num103 < Repeats; num103++)
			{
				Color color28 = NPC.GetAlpha(color29);
				color28 *= 1f - num107;
				Vector2 vector29 = new Vector2(NPC.Center.X, NPC.Center.Y) + ((float)num103 / (float)Repeats * 6.28318548f + NPC.rotation + num106).ToRotationVector2() * (6f * num107 + 2f) - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)num103;
				Main.spriteBatch.Draw(glow, vector29, NPC.frame, color28, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(glow, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
			}
		}
	}
}