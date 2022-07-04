using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Items.Sets.GamblerChestLoot.Jem;
using SpiritMod.Items.Sets.GamblerChestLoot.FunnyFirework;
using SpiritMod.Items.Sets.GamblerChestLoot.GildedMustache;
using SpiritMod.Items.Sets.GamblerChestLoot.Champagne;
using SpiritMod.Mechanics.Fathomless_Chest;
using SpiritMod.Items.Sets.GamblerChestLoot.RegalCane;

namespace SpiritMod.Items.Sets.GamblerChestLoot.GamblerChestNPCs
{
	internal class GoldChestBottom : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chest");
			Main.npcFrameCount[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 24;
			NPC.knockBackResist = 0;
			NPC.aiStyle = -1;
			NPC.lifeMax = 1;
			NPC.immortal = true;
			NPC.noTileCollide = false;
			NPC.dontCountMe = true;
		}
		int counter = -1;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 center = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			#region shader
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			Vector4 colorMod = Color.Gold.ToVector4();
			SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("Textures/noise").Value);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sineAdd / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (NPC.Center - Main.screenPosition) + new Vector2(0, 2), null, Color.White, 0f, new Vector2(50, 50), 1.1f + (sineAdd / 9), SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			#endregion

			if (counter > 0)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49_Top", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (NPC.Center - Main.screenPosition) + new Vector2(0, 2), null, new Color(200, 200, 200, 0), 0f, new Vector2(50, 50), 0.33f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, (NPC.Center - Main.screenPosition) + new Vector2(0, 2), new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), drawColor, NPC.rotation, center, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 center = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			if (counter > 0)
			{
				Color color = Color.White;
				float alpha = (Math.Max(0, 50 - counter)) / 50f;
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Sets/GamblerChestLoot/GamblerChestNPCs/GoldChestTop_White").Value, (NPC.Center - Main.screenPosition) + new Vector2(0, 2), new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), color * alpha, NPC.rotation, center, NPC.scale, SpriteEffects.None, 0f);
			}
		}
		bool rightClicked = false;
		float sineAdd = -1;
		public override void AI()
		{
			sineAdd += 0.03f;
			if (!rightClicked && NPC.velocity.Y == 0 && counter < -50)
			{
				rightClicked = true;
				NPC.velocity.Y = -5;
			}
			if (counter % 10 == 0)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldCoin, 0, 0);
				Main.dust[dust].velocity = Vector2.Zero;
			}
			if (rightClicked && NPC.velocity.Y != 0)
			{
				NPC.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
			}
			if (rightClicked && NPC.velocity.Y == 0 && NPC.localAI[0] == 0)
			{
				NPC.localAI[0]++;
				SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
			}

			counter--;
			if (counter == 0)
			{
				NPC.active = false;
				Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, Mod.Find<ModGore>("GoldChestGore4").Type, 1f);
				Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, Mod.Find<ModGore>("GoldChestGore5").Type, 1f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 30), Vector2.Zero, ProjectileID.DD2ExplosiveTrapT2Explosion, 0, 0, NPC.target);
				SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
			}

			if (counter > 0)
			{
				if (counter <= 100 && counter % 5 == 0)
				{
					int itemid;
					int item = 0;
					float val = Main.rand.NextFloat();
					if (val < .382f)
					{
						itemid = ItemID.CopperCoin;
					}
					else if (val < 0.83f)
					{
						itemid = ItemID.SilverCoin;
					}
					else if (val < 1.005f)
					{
						itemid = ItemID.GoldCoin;
					}
					else
					{
						itemid = ItemID.PlatinumCoin;
					}

					item = Item.NewItem(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, itemid, 1);
					Main.item[item].velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f)) * 4;
					Main.item[item].velocity.Y /= 2;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item);
				}

				if (counter == 50)
				{
					var source = NPC.GetSource_FromAI();

					NPC.DropItem(ModContent.ItemType<Jem.Jem>(), 0.01f, source);
					NPC.DropItem(ModContent.ItemType<Consumable.Food.GoldenCaviar>(), 0.1f, source);
					NPC.DropItem(ModContent.ItemType<FunnyFirework.FunnyFirework>(), 0.08f, source, Main.rand.Next(5, 9));
					NPC.DropItem(ItemID.AngelStatue, 0.02f, source);
					NPC.DropItem(ModContent.ItemType<Champagne.Champagne>(), 0.08f, source, Main.rand.Next(1, 3));
					NPC.DropItem(ModContent.ItemType<Mystical_Dice>(), 0.05f, source);

					switch (Main.rand.NextBool())
					{ //mutually exclusive
						case true:
							NPC.DropItem(ModContent.ItemType<GildedMustache.GildedMustache>(), 0.05f, source);
							break;
						case false:
							NPC.DropItem(ModContent.ItemType<RegalCane.RegalCane>(), 0.05f, source);
							break;
					}

					string[] lootTable = { "DiverLegs", "DiverHead", "DiverBody", "AstronautBody", "AstronautHelm", "AstronautLegs", "BeekeeperBody", "BeekeeperHead", "BeekeeperLegs", 
						"CapacitorBody", "CapacitorHead", "CapacitorLegs", "CenturionBody", "CenturionlLegs", "CenturionHead", "CommandoHead", "CommandoBody", "CommandoLegs", 
						"CowboyBody", "CowboyLegs", "CowboyHead", "FreemanBody", "FreemanLegs", "FreemanHead", "GeodeHelmet", "GeodeChestplate", "GeodeLeggings", "SnowRangerBody", "SnowRangerHead", "SnowRangerLegs",
						"JackBody", "JackLegs", "JackHead", "PlagueDoctorCowl", "PlagueDoctorRobe", "PlagueDoctorLegs", "ProtectorateBody", "ProtectorateLegs", "LeafPaddyHat", "PsychoMask", 
						"OperativeBody", "OperativeHead", "OperativeLegs", "WitchBody", "WitchHead", "WitchLegs"};
					int loot = Main.rand.Next(lootTable.Length);


					string[] donatorLootTable = { "WaasephiVanity", "MeteorVanity", "LightNovasVanity", "PixelatedFireballVanity" };
					int donatorloot = Main.rand.Next(lootTable.Length);
					if (Main.rand.NextBool(20))
					{
						NPC.DropItem(Mod.Find<ModItem>(donatorLootTable[donatorloot]).Type, source);
					}
					if (Main.rand.Next(50) == 0)
					{
						NPC.DropItem(Mod.Find<ModItem>(lootTable[loot]).Type, source);
						for (int value = 0; value < 32; value++)
						{
							int num = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 20), 50, 50, DustID.ShadowbeamStaff, 0f, -2f, 0, default, 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].scale *= .35f;
							Main.dust[num].fadeIn += .1f;
						}
					}
				}
			}

			if (rightClicked && NPC.velocity.Y == 0 && counter < 0)
			{
				NPC.rotation = 0;
				NPC.frame.Y = NPC.height;
				counter = 200;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<GoldChestTop>(), 0, 0, NPC.target, NPC.position.X, NPC.position.Y);
			}
		}
	}
	internal class GoldChestTop : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chest");
			Main.projFrames[Projectile.type] = 12;
		}

		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.damage = 0;
			Projectile.timeLeft = 200;
		}

		public override void AI()
		{
			Projectile.velocity.Y = -0.05f;
			Projectile.velocity.X = 0;
			Projectile.frameCounter++;
			if (Projectile.frameCounter > Math.Sqrt(Math.Max((Projectile.timeLeft - 100), 1) / 2) / 2)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= 12)
				{
					Projectile.frame = 0;
				}
			}

			if (Projectile.timeLeft == 1)
			{
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, Mod.Find<ModGore>("GoldChestGore1").Type, 1f);
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, Mod.Find<ModGore>("GoldChestGore2").Type, 1f);
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, Mod.Find<ModGore>("GoldChestGore3").Type, 1f);
			}
		}

		public override void PostDraw(Color lightColor)
		{
			Color color = Color.White;
			float alpha = (Math.Max(0, 50 - Projectile.timeLeft)) / 50f;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Sets/GamblerChestLoot/GamblerChestNPCs/GoldChestTop_White").Value, (Projectile.position - Main.screenPosition), new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), color * alpha, Projectile.rotation, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
		}
	}
}
