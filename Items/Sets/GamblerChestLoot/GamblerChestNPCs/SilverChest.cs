using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
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
	internal class SilverChestBottom : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Chest");
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 34;
			npc.knockBackResist = 0;
			npc.aiStyle = -1;
			npc.lifeMax = 1;
			npc.immortal = true;
			npc.noTileCollide = false;
			npc.dontCountMe = true;
		}
		int counter = -1;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Rectangle rect = new Rectangle(0, frame * (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]), Main.npcTexture[npc.type].Width, (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]));
			Main.spriteBatch.Draw(mod.GetTexture("Items/Sets/GamblerChestLoot/GamblerChestNPCs/SilverChestTop"), (npc.Center - Main.screenPosition + new Vector2(0, 4)), rect, lightColor, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
			if (counter > 0 && frame > 0)
			{
				Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49_Top"), (npc.Center - Main.screenPosition) + new Vector2(-2, 4), null, new Color(200, 200, 200, 0), 0f, new Vector2(50, 50), 0.375f * npc.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(Main.npcTexture[npc.type], (npc.Center - Main.screenPosition + new Vector2(0, 4)), rect, lightColor, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool rightClicked = false;
		int frame = 0;
		public override void AI()
		{
			if (rightClicked && npc.velocity.Y == 0 && counter < 0)
			{
				npc.rotation = 0;
				npc.frame.Y = npc.height;
				counter = 100;
				//Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<CopperChestTop>(), 0, 0, npc.target, npc.position.X, npc.position.Y);
			}
			if (!rightClicked && npc.velocity.Y == 0 && counter < -70)
			{
				rightClicked = true;
				npc.velocity.Y = -5;
			}
			if (rightClicked && npc.velocity.Y != 0)
			{
				npc.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
			}
			if (rightClicked && npc.velocity.Y == 0 && npc.localAI[0] == 0)
			{
				npc.localAI[0]++;
				Main.PlaySound(SoundID.Dig, npc.Center);
			}

			counter--;
			if (counter == 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 11);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 13);
				Main.PlaySound(SoundID.DoubleJump, npc.Center);

				npc.active = false;
				// Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/XynonCrateGore_2"), 1f);
				// Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/XynonCrateGore_3"), 1f);
				// Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/XynonCrateGore_4"), 1f);
			}
			if (counter % 10 == 0)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.SilverCoin, 0, 0);
				Main.dust[dust].velocity = Vector2.Zero;
			}
			if (counter > 0)
			{
				if (counter < 14)
				{
					npc.scale = counter / 14f;
				}
				if (frame < 4 && counter % 12 == 4)
				{
					frame++;
				}
				if (counter < 50 && counter % 3 == 0)
				{
					int itemid;
					int item = 0;
					float val = Main.rand.NextFloat();
					if (val < .577f)
					{
						itemid = ItemID.CopperCoin;
					}
					else if (val < 0.988f)
					{
						itemid = ItemID.SilverCoin;
					}
					else
					{
						itemid = ItemID.GoldCoin;
					}

					item = Item.NewItem(npc.Center, Vector2.Zero, itemid, 1);
					Main.item[item].velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f)) * 4;
					Main.item[item].velocity.Y /= 2;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item);
				}

				if (counter == 25)
				{
					npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.Jem.Jem>(), 0.005f);
					npc.DropItem(ModContent.ItemType<Items.Consumable.Food.GoldenCaviar>(), 0.07f);
					npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.FunnyFirework.FunnyFirework>(), 0.07f, Main.rand.Next(5, 9));
					npc.DropItem(ItemID.AngelStatue, 0.03f);
					npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.Champagne.Champagne>(), 0.06f, Main.rand.Next(1, 3));
					npc.DropItem(ModContent.ItemType<Mystical_Dice>(), 0.04f);
					switch (Main.rand.NextBool())
					{ //mutually exclusive
						case true:
							npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.GildedMustache.GildedMustache>(), 0.03f);
							break;
						case false:
							npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.RegalCane.RegalCane>(), 0.03f);
							break;
					}
					string[] lootTable = { "DiverLegs", "DiverHead", "DiverBody", "AstronautBody", "AstronautHelm", "AstronautLegs", "BeekeeperBody", "BeekeeperHead", "BeekeeperLegs", "CapacitorBody", "CapacitorHead", "CapacitorLegs", "CenturionBody", "CenturionlLegs", "CenturionHead", "CommandoHead", "CommandoBody", "CommandoLegs", 
						"CowboyBody", "CowboyLegs", "CowboyHead", "FreemanBody", "FreemanLegs", "FreemanHead", "GeodeHelmet", "GeodeChestplate", "GeodeLeggings", "SnowRangerBody", "SnowRangerHead", "SnowRangerLegs",
						"JackBody", "JackLegs", "JackHead", "PlagueDoctorCowl", "PlagueDoctorRobe", "PlagueDoctorLegs", "ProtectorateBody", "ProtectorateLegs", "LeafPaddyHat", "PsychoMask", 
						"OperativeBody", "OperativeHead", "OperativeLegs", "WitchBody", "WitchHead", "WitchLegs"};
					int loot = Main.rand.Next(lootTable.Length);
					if (Main.rand.Next(100) == 0)
					{
						npc.DropItem(mod.ItemType(lootTable[loot]));
						for (int value = 0; value < 32; value++)
						{
							int num = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y - 20), 50, 50, 173, 0f, -2f, 0, default, 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].scale *= .35f;
							Main.dust[num].fadeIn += .1f;
						}
					}
				}
			}
		}
	}
}
