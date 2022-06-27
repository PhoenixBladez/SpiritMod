using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.GlobalClasses.Players;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System;
using Terraria;
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
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<BloodWardPlayer>().HasRuneCircle = true;
	}

	public class BloodWardPlayer : ModPlayer
	{
		private const float MAXRADIUS = 150;
		private const int MAXFLASHTIME = 50;

		public bool HasRuneCircle { get; set; }

		private RuneCircle _circle = null;
		private int _flashTime = 0;
		private int _combatTime = 0;

		public override void ResetEffects() => HasRuneCircle = false;

		public override void PostUpdateEquips()
		{
			_combatTime--;
			if (_combatTime < 0)
				_combatTime = 0;
			if (_combatTime > 300)
				_combatTime = 300;

			float rotSpeed = Player.velocity.Length() * 0.15f * (Math.Max(60, _combatTime) / 6f);

			if (!HasRuneCircle && _circle != null)
			{
				Player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBloom(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
				Player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawRuneCircle(sB); }, ExtraDrawOnPlayer.DrawType.AlphaBlend);
				_circle.Update(rotSpeed, Player.direction, 0.2f, true);

				if (_circle.Dead)
					_circle = null;
			}

			if (HasRuneCircle)
			{
				Player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBloom(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
				Player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawRuneCircle(sB); }, ExtraDrawOnPlayer.DrawType.AlphaBlend);

				float opacity = Math.Min(((_combatTime + 50) / 250f) + 0.05f, 0.7f);
				if (_circle == null)
					_circle = new RuneCircle(MAXRADIUS, MAXRADIUS * 0.8f, 12, 8);
				else
					_circle.Update(rotSpeed, Player.direction, opacity);

				for (int i = 0; i < Main.maxNPCs; ++i)
				{
					NPC npc = Main.npc[i];
					if (npc != null && npc.active && npc.CanBeChasedBy(this) && npc.Distance(Player.MountedCenter) < MAXRADIUS)
					{
						_combatTime++;

						npc.AddBuff(ModContent.BuffType<RunicSiphon>(), 2);

						if (Main.rand.NextBool(10))
						{
							Vector2 position = Main.rand.NextVector2CircularEdge(2.5f, 2.5f) + (npc.DirectionTo(Player.MountedCenter) * npc.Distance(Player.MountedCenter) / 60f);
							ParticleHandler.SpawnParticle(new GlowParticle(npc.Center, position, new Color(252, 3, 102), Main.rand.NextFloat(0.08f, 0.1f), 60));
						}
					}
				}
			}

			_flashTime = Math.Max(_flashTime - 1, 0);
		}

		public void DrawBloom(SpriteBatch spriteBatch)
		{
			if (_circle == null || Player.dead || Player.frozen)
				return;

			float flashprogress = (float)Math.Sin((_flashTime / (float)MAXFLASHTIME) * MathHelper.Pi) / 2;
			Color color = Color.Lerp(new Color(252, 3, 98), Color.White, flashprogress);
			Texture2D bloom = Mod.GetTexture("Effects/Masks/CircleGradient");

			_circle.DelegateDraw(spriteBatch, Player.MountedCenter, 0.3f, delegate (int runeNumber)
			{
				return new RuneCircle.RuneDrawInfo(bloom, color, bloom.Bounds);
			});
		}

		public void DrawRuneCircle(SpriteBatch spriteBatch)
		{
			if (_circle == null || Player.dead || Player.frozen || Player.stoned)
				return;

			const float Scale = 1f;
			const float Repeats = 4f;

			float flashprogress = 0.66f * (float)Math.Sin(_flashTime / (float)MAXFLASHTIME * MathHelper.Pi);
			Color color = Color.Lerp(new Color(255, 0, 111) * 0.66f, Color.White, flashprogress);
			float timer = (float)(Math.Sin(Main.GameUpdateCount / 15f) / 2) + 0.5f;

			for (int i = 0; i < Repeats; i++)
			{
				Vector2 drawPos = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / Repeats) * timer * 8;
				_circle.DelegateDraw(spriteBatch, Player.MountedCenter + drawPos, Scale, delegate(int runeNumber) 
				{ 
					return new RuneCircle.RuneDrawInfo(Mod.GetTexture("Textures/Runes_outline"), color * (1 - timer)); 
				});
			}

			_circle.Draw(spriteBatch, Player.MountedCenter, color, Scale);

			_circle.DelegateDraw(spriteBatch, Player.MountedCenter, Scale, delegate (int runeNumber)
			{
				return new RuneCircle.RuneDrawInfo(Mod.GetTexture("Textures/Runes_mask"), Color.White * flashprogress);
			});
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0 && target.Distance(Player.Center) < MAXRADIUS && !target.SpawnedFromStatue && Player.statLife < Player.statLifeMax2 && HasRuneCircle)
				MakeRunicHeart(target.Center, Main.rand.Next(14, 17));
			_combatTime = 180;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0 && target.Distance(Player.Center) < MAXRADIUS && !target.SpawnedFromStatue && Player.statLife < Player.statLifeMax2 && HasRuneCircle)
				MakeRunicHeart(target.Center, Main.rand.Next(9, 12));
			_combatTime = 180;
		}

		private void MakeRunicHeart(Vector2 position, int healAmount)
		{
			_flashTime = MAXFLASHTIME;
			Projectile.NewProjectileDirect(position, Player.DirectionTo(position).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(2, 3), ModContent.ProjectileType<RuneHeart>(), healAmount, 0f, Player.whoAmI).netUpdate = true;
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit) => _combatTime = 300;
	}
}
