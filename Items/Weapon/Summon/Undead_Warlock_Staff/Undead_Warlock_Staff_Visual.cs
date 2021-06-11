using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Weapon.Summon.Undead_Warlock_Staff
{
	public class Undead_Warlock_Staff_Visual : ModPlayer
	{
		public bool flag = false;
		public bool sacrificed = false;
		public static bool isHolding = false;
		public static int coolDownStatic = 0;
		public int coolDown = 0;
		public override void ResetEffects()
		{
			isHolding = false;
			sacrificed = false;
		}
		public override void PostUpdate()
		{
			coolDown--;
			if (isHolding)
			{
				if (sacrificed)
				{
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile projectile = Main.projectile[i];
						if (projectile.minion && projectile.active && projectile.owner == player.whoAmI)
						{
							int num = 1100;
							for (int index1 = 0; index1 < (int) byte.MaxValue; ++index1)
							{
								if (Main.player[index1].active && !Main.player[index1].dead && ((double) (projectile.Center - Main.player[index1].position).Length() < (double) num && Main.player[index1].inventory[Main.player[index1].selectedItem].type == mod.ItemType("Undead_Warlock_Staff")))
								{
									for (int aa = 0; aa < 20; ++aa)
									{
										Vector2 center = projectile.Center;
										center.X += (float) Main.rand.Next(-300, 300) * 0.05f;
										center.Y += (float) Main.rand.Next(-300, 300) * 0.05f;
										int index2 = Dust.NewDust(center + projectile.velocity, 20, 20, 235, 0.0f, 0.0f, 0, new Color(), 1f);
										Main.dust[index2].velocity *= 0.0f;
										Main.dust[index2].scale = (float) Main.rand.Next(70, 85) * 0.01f;
										Main.dust[index2].fadeIn = (float) (index1 + 1);
									}
									for (int bb = 0; bb < 20; ++bb)
									{
										Vector2 center = projectile.Center;
										center.X += (float) Main.rand.Next(-300, 300) * 0.05f;
										center.Y += (float) Main.rand.Next(-300, 300) * 0.05f;
										int index2 = Dust.NewDust(center + projectile.velocity, 20, 20, DustType<Dusts.Mana>(), 0.0f, 0.0f, 0, new Color(), 1f);
										Main.dust[index2].velocity *= 0.0f;
										Main.dust[index2].scale = (float) Main.rand.Next(70, 85) * 0.01f;
										Main.dust[index2].fadeIn = (float) (index1 + 1);
									}
								}				
							}
							projectile.Kill();
							sacrificed = false;
							{
								if (player.statLife < player.statLifeMax)
								{
									player.statLife += 5;
									player.HealEffect(5);
								}
								if (player.statMana < player.statManaMax2)
								{
									player.statMana += 20;
									player.ManaEffect(20);
								}
							}
							break;
						}
					}
				}
				coolDownStatic = coolDown;
				if (coolDown > 0 && flag)
				{		
					if (coolDown == 10*60-1)
						Main.PlaySound(42, (int)player.position.X, (int)player.position.Y, 61, 1f, 0.0f);
					player.minionDamage += 0.25f;
					float num5 = player.direction == 1 ? 0.0f : 3.141593f;
					for (int index = 0; index < 4; ++index)
					{
						Vector2 vector2_4 = Vector2.Zero;
						vector2_4 = Main.rand.Next(2) == 0 ? Vector2.UnitX.RotatedByRandom(6.28318548202515) * new Vector2(200f, 50f) * (float) ((double) Main.rand.NextFloat() * 0.699999988079071 + 0.300000011920929) : Vector2.UnitX.RotatedByRandom(3.14159274101257).RotatedBy((double) num5, new Vector2()) * new Vector2(200f, 50f) * (float) ((double) Main.rand.NextFloat() * 0.699999988079071 + 0.300000011920929);
						if (Main.rand.Next(2)==0)
						{
							int p = Projectile.NewProjectile(player.Center.X, player.Center.Y - 90, vector2_4.X, vector2_4.Y, mod.ProjectileType("Necromancer_Ray"), 10, player.whoAmI, player.whoAmI, 0.0f, 0.0f);
							Main.projectile[p].scale = Main.rand.Next(10,500)*0.001f;
						}
					}
				}
				if (coolDown <= 0)
					flag = false;
			}
		}
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int arms = layers.FindIndex(l => l == PlayerLayer.Arms);
			if (arms < 0)
				return;

			layers.Insert(arms, new PlayerLayer(mod.Name, "HeldItem",
				delegate(PlayerDrawInfo drawInfo)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				if (drawInfo.shadow != 0f)
				{
					return;
				}
			
				SpriteEffects spriteEffects;
				SpriteEffects effect;
				if ((double) drawPlayer.gravDir == 1.0)
				{
					if (drawPlayer.direction == 1)
					{
						spriteEffects = SpriteEffects.None;
						effect = SpriteEffects.None;
					}
					else
					{
						spriteEffects = SpriteEffects.FlipHorizontally;
						effect = SpriteEffects.FlipHorizontally;
					}
				}
				else
				{
					if (drawPlayer.direction == 1)
					{
						spriteEffects = SpriteEffects.FlipVertically;
						effect = SpriteEffects.FlipVertically;
					}
					else
					{
						spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
						effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					}
				}
				DrawData drawData = new DrawData();
				Vector2 Position = drawInfo.position;
				float num23 = 26f;
				float num24 = -30f;
				
				Mod mod = ModLoader.GetMod("SpiritMod");
				Texture2D agate = mod.GetTexture("Items/Weapon/Summon/Undead_Warlock_Staff/Undead_Warlock_Staff");
				Microsoft.Xna.Framework.Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int) ((double) Position.X + (double) drawPlayer.width * 0.5) / 16, (int) ((double) Position.Y + (double) drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), drawInfo.shadow);
				if (isHolding && !drawPlayer.mount.Active)
				{
					drawData = new DrawData(agate, new Vector2((float) (int) ((double) Position.X - (double) Main.screenPosition.X + (double) (drawPlayer.width / 2) - (double) (9 * drawPlayer.direction)) + num23 * (float) drawPlayer.direction, (float) (int) ((double) Position.Y - (double) Main.screenPosition.Y + (double) (drawPlayer.height / 2) + 2.0 * (double) drawPlayer.gravDir + (double) num24 * (double) drawPlayer.gravDir)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, agate.Width, agate.Height)), color12, drawPlayer.bodyRotation, new Vector2((float) (agate.Width / 2), (float) (agate.Height / 2)), 1f, spriteEffects, 0);
					Main.playerDrawData.Add(drawData);	
				}
			}));
			
			int arms2 = layers.FindIndex(l => l == PlayerLayer.Arms);
			if (arms2 < 0)
				return;

			layers.Insert(arms+1, new PlayerLayer(mod.Name, "HeldItem",
				delegate(PlayerDrawInfo drawInfo)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				if (drawInfo.shadow != 0f)
				{
					return;
				}
			
				SpriteEffects spriteEffects;
				SpriteEffects effect;
				if ((double) drawPlayer.gravDir == 1.0)
				{
					if (drawPlayer.direction == 1)
					{
						spriteEffects = SpriteEffects.None;
						effect = SpriteEffects.None;
					}
					else
					{
						spriteEffects = SpriteEffects.FlipHorizontally;
						effect = SpriteEffects.FlipHorizontally;
					}
				}
				else
				{
					if (drawPlayer.direction == 1)
					{
						spriteEffects = SpriteEffects.FlipVertically;
						effect = SpriteEffects.FlipVertically;
					}
					else
					{
						spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
						effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					}
				}
				DrawData drawData = new DrawData();
				Vector2 Position = drawInfo.position;
				float num23 = 26f;
				float num24 = -30f;
				
				Mod mod = ModLoader.GetMod("SpiritMod");
				Texture2D agate = mod.GetTexture("Items/Weapon/Summon/Undead_Warlock_Staff/Undead_Warlock_Staff_Glow");
				Microsoft.Xna.Framework.Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int) ((double) Position.X + (double) drawPlayer.width * 0.5) / 16, (int) ((double) Position.Y + (double) drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), drawInfo.shadow);
				if (isHolding && coolDown > 0 && flag && !drawPlayer.mount.Active)
				{
					drawData = new DrawData(agate, new Vector2((float) (int) ((double) Position.X - (double) Main.screenPosition.X + (double) (drawPlayer.width / 2) - (double) (9 * drawPlayer.direction)) + num23 * (float) drawPlayer.direction, (float) (int) ((double) Position.Y - (double) Main.screenPosition.Y + (double) (drawPlayer.height / 2) + 2.0 * (double) drawPlayer.gravDir + (double) num24 * (double) drawPlayer.gravDir)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, agate.Width, agate.Height)), Color.White, drawPlayer.bodyRotation, new Vector2((float) (agate.Width / 2), (float) (agate.Height / 2)), 1f, spriteEffects, 0);
					Main.playerDrawData.Add(drawData);	
				}
			}));
		}
	}
}