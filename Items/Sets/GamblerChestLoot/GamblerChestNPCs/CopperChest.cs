using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Mechanics.Fathomless_Chest;

namespace SpiritMod.Items.Sets.GamblerChestLoot.GamblerChestNPCs
{
	internal class CopperChestBottom : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Chest");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 26;
			NPC.knockBackResist = 0;
			NPC.aiStyle = -1;
			NPC.lifeMax = 1;
			NPC.immortal = true;
			NPC.noTileCollide = false;
			NPC.dontCountMe = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 center = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Rectangle rect = new Rectangle(0, frame * (TextureAssets.Npc[NPC.type].Value.Height / 4), TextureAssets.Npc[NPC.type].Value.Width, (TextureAssets.Npc[NPC.type].Value.Height / 4));
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/Sets/GamblerChestLoot/GamblerChestNPCs/CopperChestTop").Value, (NPC.Center - Main.screenPosition + new Vector2(0, 4)), rect, lightColor, NPC.rotation, center, NPC.scale, SpriteEffects.None, 0f);
			if (counter > 0 && frame > 0)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49_Top").Value, (NPC.Center - Main.screenPosition) + new Vector2(-2, 4), null, new Color(200, 200, 200, 0), 0f, new Vector2(50, 50), 0.3f * NPC.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, (NPC.Center - Main.screenPosition + new Vector2(0, 4)), rect, lightColor, NPC.rotation, center, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}

		bool rightClicked = false;
		int frame = 0;
		int counter = -1;

		public override void AI()
		{
			if (!rightClicked && NPC.velocity.Y == 0 && counter < -70)
				rightClicked = true;
			if (rightClicked && NPC.velocity.Y != 0)
				NPC.rotation += Main.rand.NextFloat(-0.1f, 0.1f);

			counter--;
			if (counter == 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, 11);
				Gore.NewGore(NPC.position, NPC.velocity, 12);
				Gore.NewGore(NPC.position, NPC.velocity, 13);
				SoundEngine.PlaySound(SoundID.DoubleJump, NPC.Center);
				NPC.active = false;
			}
			if (counter % 10 == 0)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CopperCoin, 0, 0);
				Main.dust[dust].velocity = Vector2.Zero;
			}
			if (counter > 0)
			{
				if (counter < 14)
					NPC.scale = counter / 14f;

				if (frame < 3 && counter % 12 == 4)
					frame++;

				if (counter <= 50 && counter % 10 == 0)
				{
					int itemid;
					float val = Main.rand.NextFloat();
					if (val < .454f)
						itemid = ItemID.CopperCoin;
					else if (val < 0.99f)
						itemid = ItemID.SilverCoin;
					else
						itemid = ItemID.GoldCoin;

					int item = Item.NewItem(NPC.Center, Vector2.Zero, itemid, 1);
					Main.item[item].velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f)) * 4;
					Main.item[item].velocity.Y /= 2;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item);
				}
				if (counter == 25)
				{
					string[] lootTable = { "DiverLegs", "DiverHead", "DiverBody", "AstronautBody", "AstronautHelm", "AstronautLegs", "BeekeeperBody", "BeekeeperHead", "BeekeeperLegs",
						"CapacitorBody", "CapacitorHead", "CapacitorLegs", "CenturionBody", "CenturionlLegs", "CenturionHead", "CommandoHead", "CommandoBody", "CommandoLegs",
						"CowboyBody", "CowboyLegs", "CowboyHead", "FreemanBody", "FreemanLegs", "FreemanHead", "GeodeHelmet", "GeodeChestplate", "GeodeLeggings", "SnowRangerBody", "SnowRangerHead", "SnowRangerLegs",
						"JackBody", "JackLegs", "JackHead", "PlagueDoctorCowl", "PlagueDoctorRobe", "PlagueDoctorLegs", "ProtectorateBody", "ProtectorateLegs", "LeafPaddyHat", "PsychoMask",
						"OperativeBody", "OperativeHead", "OperativeLegs", "WitchBody", "WitchHead", "WitchLegs"};
					int loot = Main.rand.Next(lootTable.Length);

					string[] donatorLootTable = { "WaasephiVanity", "MeteorVanity", "LightNovasVanity", "PixelatedFireballVanity"};
					int donatorloot = Main.rand.Next(lootTable.Length);
					if (Main.rand.NextBool(100))
					{
						NPC.DropItem(Mod.Find<ModItem>(donatorLootTable[donatorloot]).Type);
					}
					if (Main.rand.Next(250) == 0)
					{
						NPC.DropItem(Mod.Find<ModItem>(lootTable[loot]).Type);
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
					NPC.DropItem(ModContent.ItemType<Jem.Jem>(), 0.0025f);
					NPC.DropItem(ModContent.ItemType<Consumable.Food.GoldenCaviar>(), 0.05f);
					NPC.DropItem(ModContent.ItemType<FunnyFirework.FunnyFirework>(), 0.05f, Main.rand.Next(5, 9));
					NPC.DropItem(ItemID.AngelStatue, 0.05f);
					NPC.DropItem(ModContent.ItemType<Champagne.Champagne>(), 0.04f, Main.rand.Next(1, 3));
					NPC.DropItem(ModContent.ItemType<Mystical_Dice>(), 0.01f);

					switch (Main.rand.NextBool())
					{ //mutually exclusive
						case true:
							NPC.DropItem(ModContent.ItemType<GildedMustache.GildedMustache>(), 0.01f);
							break;
						case false:
							NPC.DropItem(ModContent.ItemType<RegalCane.RegalCane>(), 0.01f);
							break;
					}
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			if (rightClicked && NPC.velocity.Y == 0 && counter < 0)
			{
				NPC.rotation = 0;
				NPC.frame.Y = frameHeight;
				counter = 100;
			}
		}
	}
}