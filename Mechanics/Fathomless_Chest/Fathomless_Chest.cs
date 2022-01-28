using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using System;
using SpiritMod.Items.Glyphs;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ObjectData;
using Terraria.Enums;
using static Terraria.ModLoader.ModContent;
using Steamworks;
using System.Linq;

namespace SpiritMod.Mechanics.Fathomless_Chest
{
	public class Fathomless_Chest : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.Origin = new Point16(0, 2);
			Main.tileValue[Type] = 1000;
			TileObjectData.addTile(Type);
			drop = ModContent.ItemType<Tiles.Black_Stone_Item>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fathomless Vase");
			Main.tileSpelunker[Type] = true;
			AddMapEntry(new Color(112, 216, 238), name);
			disableSmartCursor = true;
			Main.tileLighted[Type] = true;
			soundType = SoundID.Trackable;
			soundStyle = 170;
		}
		public override bool CanExplode(int i, int j) => false;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0.2f;
			b = 0.5f;
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool NewRightClick(int i, int j)
		{
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, i, j);
			return true;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			TileUtilities.BlockActuators(i, j);
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.showItemIcon2 = ItemType<Fathomless_Chest_Item>();
			player.showItemIconText = "";
			player.noThrow = 2;
			player.showItemIcon = true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 100, 100)];
			Tile tile = Main.tile[i, j];
			Main.PlaySound(SoundID.Trackable, i * 16, j * 16, 186);
			for (int index1 = 0; index1 < 3; ++index1)
			{
				for (int index2 = 0; index2 < 2; ++index2)
				{
					int index3 = Gore.NewGore(new Vector2(i * 16, j * 16), new Vector2(0.0f, 0.0f), 99, 1.1f);
					Main.gore[index3].velocity *= 0.6f;
				}
			}
			for (int index = 0; index < 9; ++index)
			{
				float SpeedX = (float)(-1 * Main.rand.Next(40, 70) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
				float SpeedY = (float)(-1 * Main.rand.Next(40, 70) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
				int p = Projectile.NewProjectile((float)(i * 16) + 8 + SpeedX, (float)(j * 16) + 12 + SpeedY, SpeedX, SpeedY, mod.ProjectileType("Visual_Projectile"), 0, 0f, player.whoAmI, 0.0f, 0.0f);
				Main.projectile[p].scale = Main.rand.Next(30, 150) * 0.01f;
			}

			//int randomEffectCounter = 5;
			int randomEffectCounter = Main.rand.Next(12);
			bool CheckTileRange(int[] tiletypes, int size)
			{
				for (int k = i - size; k <= i + size; k++)
				{
					for (int l = j - size; l <= j + size; l++)
					{
						if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
						{
							int type = Main.tile[k, l].type;
							if (tiletypes.Contains(type))
								return true;
						}
					}
				}
				return false;
			}

			while (randomEffectCounter == 7)
			{
				if (CheckTileRange(new int[] { TileID.Dirt, TileID.Stone, TileID.IceBlock }, 22))
					break;

				randomEffectCounter = Main.rand.Next(13);
			}

			while (randomEffectCounter == 6 || randomEffectCounter == 11)
			{
				if (CheckTileRange(new int[] { TileID.Stone }, 19))
					break;

				randomEffectCounter = Main.rand.Next(13);
			}

			while (randomEffectCounter == 9)
			{
				if (player.CountBuffs() > 0)
					break;

				randomEffectCounter = Main.rand.Next(13);
			}

			switch (randomEffectCounter)
			{
				case 0: //SPAWN ZOMBIES
					{
						BadLuck(i, j);
						int a = NPC.NewNPC((i * 16) + -8 - 16 - 16, (j * 16) + 6, 21);
						int b = NPC.NewNPC((i * 16) + -8 - 16, (j * 16) + 6, 21);
						int c = NPC.NewNPC((i * 16) + 8, (j * 16) + 6, 21);
						int d = NPC.NewNPC((i * 16) + 24 + 16, (j * 16) + 6, 21);
						int e = NPC.NewNPC((i * 16) + 24 + 32, (j * 16) + 6, 21);
						Main.npc[a].netUpdate = true;
						Main.npc[b].netUpdate = true;
						Main.npc[c].netUpdate = true;
						Main.npc[d].netUpdate = true;
						Main.npc[e].netUpdate = true;
						break;
					}
				case 1: //DROP COINS
					{
						BadLuck(i, j);
						Main.PlaySound(SoundID.Coins, i * 16, j * 16, 0);
						int num1 = 0;
						for (int index = 0; index < 59; ++index)
						{
							if (player.inventory[index].type >= ItemID.CopperCoin && player.inventory[index].type <= ItemID.PlatinumCoin)
							{
								int number = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, player.inventory[index].type, 1);
								if (Main.netMode != NetmodeID.SinglePlayer && number >= 0)
									NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
								int num2 = player.inventory[index].stack / 5;
								if (Main.expertMode)
									num2 = (int)(player.inventory[index].stack * 0.25f);

								int num3 = player.inventory[index].stack - num2;
								player.inventory[index].stack -= num3;
								if (player.inventory[index].type == ItemID.CopperCoin)
									num1 += num3;

								if (player.inventory[index].type == ItemID.SilverCoin)
									num1 += num3 * 100;

								if (player.inventory[index].type == ItemID.GoldCoin)
									num1 += num3 * 10000;

								if (player.inventory[index].type == ItemID.PlatinumCoin)
									num1 += num3 * 1000000;

								if (player.inventory[index].stack <= 0)
									player.inventory[index] = new Item();

								Main.item[number].stack = num3;
								Main.item[number].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
								Main.item[number].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
								Main.item[number].noGrabDelay = 600;
								if (index == 58)
								{
									Main.mouseItem = player.inventory[index].Clone();
								}
							}
						}
						player.lostCoins = num1;
						player.lostCoinString = Main.ValueToCoins(player.lostCoins);
						break;
					}
				case 2: //SPAWN POTIONS
					{
						GoodLuck(i, j);
						int randomPotion = Utils.SelectRandom(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
						int randomPotion2 = Utils.SelectRandom(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
						int randomPotion3 = Utils.SelectRandom(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
						int randomPotion4 = Utils.SelectRandom(Main.rand, new int[38] { 2344, 303, 300, 2325, 2324, 2356, 2329, 2346, 295, 2354, 2327, 291, 305, 2323, 304, 2348, 297, 292, 2345, 2352, 294, 293, 2322, 299, 288, 2347, 289, 298, 2355, 296, 2353, 2328, 290, 301, 2326, 2359, 302, 2349 });
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							randomPotion = 2344;
							randomPotion2 = 303;
							randomPotion3 = 300;
							randomPotion4 = 2356;
						}
						int number = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion, 1);
						Main.item[number].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
						Main.item[number].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
						if (Main.netMode != NetmodeID.SinglePlayer && number >= 0)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
						}
						int number2 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion2, 1);
						Main.item[number2].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
						Main.item[number2].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
						if (Main.netMode != NetmodeID.SinglePlayer && number2 >= 0)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number2, 1f);
						}
						int number3 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion3, 1);
						Main.item[number3].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
						Main.item[number3].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
						if (Main.netMode != NetmodeID.SinglePlayer && number3 >= 0)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number3, 1f);
						}
						int number4 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, randomPotion4, 1);
						Main.item[number4].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
						Main.item[number4].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
						if (Main.netMode != NetmodeID.SinglePlayer && number4 >= 0)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number4, 1f);
						}
						break;
					}
				case 3: //SPAWN AN ITEM
					{
						GoodLuck(i, j);
						int item = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, 393, 1);
						if (Main.netMode != NetmodeID.SinglePlayer && item >= 0)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
						}
						int item1 = Item.NewItem((int)(i * 16) + 8, (int)(j * 16) + 12, 16, 18, 18, 1);
						if (Main.netMode != NetmodeID.SinglePlayer && item1 >= 0)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item1, 1f);
						}
						break;
					}
				case 4: //SPAWN BUNNIES
					{
						NeutralLuck(i, j);
						float npcposX = 0f;
						float npcposY = 0f;
						for (int g = 0; g < 8 + Main.rand.Next(6); g++)
						{
							npcposX = (i * 16) + Main.rand.Next(-60, 60);
							npcposY = (j * 16) + Main.rand.Next(-60, 60);
							int a = NPC.NewNPC((int)npcposX, (int)npcposY, 356);
							Main.npc[a].value = 0;
							Main.npc[a].friendly = false;
							Main.npc[a].netUpdate = true;
							Vector2 spinningpoint = new Vector2(0.0f, -3f).RotatedByRandom(MathHelper.Pi);
							float num1 = (float)28;
							Vector2 vector2 = new Vector2(1.1f, 1f);
							for (float num2 = 0.0f; (double)num2 < (double)num1; ++num2)
							{
								int dustIndex = Dust.NewDust(new Vector2(npcposX, npcposY), 0, 0, DustID.MagicMirror, 0.0f, 0.0f, 0, new Color(), 1f);
								Main.dust[dustIndex].position = new Vector2(npcposX, npcposY);
								Main.dust[dustIndex].velocity = spinningpoint.RotatedBy(6.28318548202515 * (double)num2 / (double)num1, new Vector2()) * vector2 * (float)(0.800000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
								Main.dust[dustIndex].noGravity = true;
								Main.dust[dustIndex].scale = 2f;
								Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
								Dust dust = Dust.CloneDust(dustIndex);
								dust.scale /= 2f;
								dust.fadeIn /= 2f;
							}
							Main.PlaySound(SoundID.Item, (int)npcposX, (int)npcposY, 6, 1f, 0f);
						}
						break;
					}
				case 5: //Convert regional stone into gems
					{
						GoodLuck(i, j);
						int gemType = Main.rand.Next(new int[] { 63, 64, 65, 66, 67, 68 });
						if (Main.netMode != NetmodeID.SinglePlayer)
							gemType = 64;
						ConvertStone(i, j, 22, gemType, 0.1f);

						for (int val = 0; val < 22; val++)
						{
							int num = Dust.NewDust(new Vector2(i * 16, j * 16), 80, 80, DustID.Electric, 0f, -2f, 0, default, 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(77, player);
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].scale *= .25f;
						}
						break;
					}
				case 6: //Places opposite world evil
					{
						BadLuck(i, j);
						for (int value = 0; value < 32; value++)
						{
							int num = Dust.NewDust(new Vector2(i * 16, j * 16), 50, 50, DustID.Wraith, 0f, -2f, 0, default, 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].scale *= .35f;
							Main.dust[num].fadeIn += .1f;
						}
						if (WorldGen.crimson)
						{
							ConvertStone(i, j, 22, 25);
							ConvertDirt(i, j, 22, 23);
							ConvertIce(i, j, 22, TileID.CorruptIce);
						}
						else
						{
							ConvertStone(i, j, 22, 203);
							ConvertDirt(i, j, 22, 199);
							ConvertIce(i, j, 22, TileID.FleshIce);
						}
						break;
					}
				case 7: //Midas effect
					{
						GoodLuck(i, j);
						player.AddBuff(ModContent.BuffType<Buffs.MidasTouch>(), 3600 * 5);
						for (int numi = 0; numi < 8; numi++)
						{
							float SpeedX = (float)(-1 * Main.rand.Next(40, 70) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
							float SpeedY = (float)(-1 * Main.rand.Next(40, 70) * 0.00999999977648258 + Main.rand.Next(-20, 21) * 0.400000005960464);
							int p = Projectile.NewProjectile((float)(i * 16) + 8 + SpeedX, (float)(j * 16) + 12 + SpeedY, SpeedX, SpeedY, mod.ProjectileType("MidasProjectile"), 0, 0f, player.whoAmI, 0.0f, 0.0f);
							Main.projectile[p].scale = Main.rand.Next(60, 150) * 0.01f;
						}
						break;
					}
				case 8: //Clear all buffs and debuffs
					{
						NeutralLuck(i, j);
						for (int index1 = 0; index1 < 22; ++index1)
							player.DelBuff(index1);
						int index = CombatText.NewText(new Rectangle(i * 16, j * 16, player.width, player.height), new Color(255, 255, 255), "All Buffs Cleared", false, false);
						CombatText combatText = Main.combatText[index];
						NetMessage.SendData(MessageID.CombatTextInt, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
						break;
					}
				case 9: //Darkness and Weak
					{
						BadLuck(i, j);
						player.AddBuff(BuffID.Darkness, 3600);
						player.AddBuff(BuffID.Weak, 3600);
						break;
					}
				case 10: //Opposite gold/platinum ore
					{
						GoodLuck(i, j);
						int oreType;
						if (WorldGen.GoldTierOre == TileID.Platinum)
							oreType = TileID.Gold;
						else
							oreType = TileID.Platinum;
						ConvertStone(i, j, 22, oreType, 0.25f);
						break;
					}
				case 11:
					{
						GoodLuck(i, j);
						int item = Item.NewItem((i * 16) + 8, (j * 16) + 12, 16, 18, ModContent.ItemType<Glyph>(), 1);
						if (Main.netMode != NetmodeID.SinglePlayer && item >= 0)
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
						break;
					}
			}
		}
		public void ConvertStone(int i, int j, int size, int typeConvert, float density = 1f)
		{
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
					{
						int type = (int)Main.tile[k, l].type;
						if (TileID.Sets.Conversion.Stone[type] && Main.rand.NextFloat() <= density)
						{
							Main.tile[k, l].type = (ushort)typeConvert;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
					}
				}
			}
		}
		public void ConvertDirt(int i, int j, int size, int typeConvert)
		{
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
					{
						int type = (int)Main.tile[k, l].type;
						if (type == 0 || type == 2)
						{
							Main.tile[k, l].type = (ushort)typeConvert;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
					}
				}
			}
		}
		public void ConvertIce(int i, int j, int size, int typeConvert)
		{
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
					{
						int type = (int)Main.tile[k, l].type;
						if (TileID.Sets.Conversion.Ice[type])
						{
							Main.tile[k, l].type = (ushort)typeConvert;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
					}
				}
			}
		}

		public void BadLuck(int i, int j)
		{
			Player player = Main.LocalPlayer;
			int index = CombatText.NewText(new Rectangle(i * 16, j * 16, player.width, player.height), new Color(255, 150, 150), "Bad Luck!", false, false);
			CombatText combatText = Main.combatText[index];
			NetMessage.SendData(MessageID.CombatTextInt, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
		}

		public void GoodLuck(int i, int j)
		{
			Player player = Main.LocalPlayer;
			int index = CombatText.NewText(new Rectangle(i * 16, j * 16, player.width, player.height), new Color(150, 255, 150), "Good Luck!", false, false);
			CombatText combatText = Main.combatText[index];
			NetMessage.SendData(MessageID.CombatTextInt, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
		}

		public void NeutralLuck(int i, int j)
		{
			Player player = Main.LocalPlayer;
			int index = CombatText.NewText(new Rectangle(i * 16, j * 16, player.width, player.height), new Color(150, 150, 255), "Neutral Luck!", false, false);
			CombatText combatText = Main.combatText[index];
			NetMessage.SendData(MessageID.CombatTextInt, -1, -1, NetworkText.FromLiteral(combatText.text), (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, 0.0f, 0, 0, 0);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i - tile.frameX / 18;
			int top = j - tile.frameY / 18;
			int spawnX = left * 16;
			int spawnY = top * 16;
			if (Main.rand.Next(20) == 0)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(spawnX, spawnY), 16 * 2, 16 * 3, DustID.DungeonSpirit, 0.0f, 0.0f, 150, new Color(), 0.3f)];
				dust.fadeIn = 0.75f;
				dust.velocity *= 0.1f;
				dust.noLight = true;
				dust.noGravity = true;
			}
			return true;
		}
	}

	internal class Fathomless_Chest_Item : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fathomless Vase");
			Tooltip.SetDefault("You aren't supposed to have this!");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.value = 0;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Yellow;
			item.createTile = ModContent.TileType<Fathomless_Chest>();
		}
	}
}