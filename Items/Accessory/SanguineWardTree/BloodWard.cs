using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class BloodWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Ward");
			Tooltip.SetDefault("Creates a weakening runic aura\nKilling enemies inside your runic aura heals some life");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<BloodWardPlayer>().HasRuneCircle = true;
		}
	}

	public class BloodWardPlayer : ModPlayer
	{
		public bool HasRuneCircle { get; set; }
		private RuneCircle _circle = null;
		private const float MAXRADIUS = 150;
		private int _flashTime = 0;
		private const int MAXFLASHTIME = 50;
		public override void ResetEffects() => HasRuneCircle = false;

		public override void PostUpdateEquips()
		{

			if (!HasRuneCircle && _circle != null)
			{
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBloom(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawRuneCircle(sB); }, ExtraDrawOnPlayer.DrawType.AlphaBlend);
				_circle.Update(player.velocity.Length(), player.direction, true);
				if (_circle.Dead)
					_circle = null;
			}
			if (HasRuneCircle)
			{
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBloom(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawRuneCircle(sB); }, ExtraDrawOnPlayer.DrawType.AlphaBlend);

				if (_circle == null)
					_circle = new RuneCircle(MAXRADIUS, MAXRADIUS * 0.8f, 12, 8);
				else
					_circle.Update(player.velocity.Length(), player.direction);

				foreach(NPC npc in Main.npc.Where(x => x != null && x.active && x.CanBeChasedBy(this)))
				{
					if (npc.Distance(player.MountedCenter) < MAXRADIUS)
					{
						npc.AddBuff(ModContent.BuffType<RunicSiphon>(), 2);
						if (Main.rand.NextBool(10))
							ParticleHandler.SpawnParticle(new GlowParticle(npc.Center,
								Main.rand.NextVector2CircularEdge(2.5f, 2.5f) + (npc.DirectionTo(player.MountedCenter) * npc.Distance(player.MountedCenter) / 60f),
								new Color(252, 3, 102), Main.rand.NextFloat(0.08f, 0.1f), 60));
					}
				}
			}

			_flashTime = Math.Max(_flashTime - 1, 0);
		}

		public void DrawBloom(SpriteBatch spriteBatch)
		{
			if (_circle == null || player.dead || player.frozen)
				return;

			float flashprogress = (float)Math.Sin((_flashTime / (float)MAXFLASHTIME) * MathHelper.Pi) / 2;
			Color color = Color.Lerp(new Color(252, 3, 98), Color.White, flashprogress);
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");

			_circle.DelegateDraw(spriteBatch, player.MountedCenter, (flashprogress/5) + 0.3f, delegate (int runeNumber)
			{
				return new RuneCircle.RuneDrawInfo(bloom, color, bloom.Bounds);
			});
		}

		public void DrawRuneCircle(SpriteBatch spriteBatch)
		{
			if (_circle == null || player.dead || player.frozen)
				return;

			float flashprogress = 0.66f * (float)Math.Sin((_flashTime / (float)MAXFLASHTIME) * MathHelper.Pi);
			Color color = Color.Lerp(new Color(255, 0, 111) * 0.66f, Color.White, flashprogress);
			float scale = 1.33f;
			float timer = (float)(Math.Sin(Main.GameUpdateCount / 15f) / 2) + 0.5f;
			for (int i = 0; i < 8; i++)
			{
				Vector2 drawPos = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 8f) * timer * 8;
				_circle.DelegateDraw(spriteBatch, player.MountedCenter + drawPos, scale, delegate(int runeNumber) 
				{ 
					return new RuneCircle.RuneDrawInfo(mod.GetTexture("Textures/Runes_outline"), color * (1 - timer)); 
				});
			}
			_circle.Draw(spriteBatch, player.MountedCenter, color, scale);

			_circle.DelegateDraw(spriteBatch, player.MountedCenter, scale, delegate (int runeNumber)
			{
				return new RuneCircle.RuneDrawInfo(mod.GetTexture("Textures/Runes_mask"), Color.White * flashprogress);
			});
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0 && target.Distance(player.Center) < MAXRADIUS && !target.SpawnedFromStatue && player.statLife < player.statLifeMax2)
				MakeRunicHeart(target.Center, Main.rand.Next(14, 17));
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0 && target.Distance(player.Center) < MAXRADIUS && !target.SpawnedFromStatue && player.statLife < player.statLifeMax2)
				MakeRunicHeart(target.Center, Main.rand.Next(9, 12));
		}

		private void MakeRunicHeart(Vector2 position, int healAmount)
		{
			_flashTime = MAXFLASHTIME;
			Projectile.NewProjectileDirect(position, player.DirectionTo(position).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(2, 3), ModContent.ProjectileType<RuneHeart>(), healAmount, 0f, player.whoAmI).netUpdate = true;
		}
	}
}
