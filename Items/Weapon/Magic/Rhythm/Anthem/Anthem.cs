using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.Rhythm.Anthem
{
	public class Anthem : ModItem, IRhythmWeapon
	{
		public RhythmMinigame Minigame { get; set; }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anthem");
			Tooltip.SetDefault("'We're here to make you think about death and get sad and stuff!'");
		}

		public override void SetDefaults()
		{
			item.damage = 24;
			item.magic = true;
			item.mana = 40;
			item.width = 40;
			item.height = 40;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTurn = true;
			item.holdStyle = ItemHoldStyleID.HarpHoldingOut;
			item.noMelee = true;
			item.knockBack = 0f;
			item.value = 200;
			item.rare = ItemRarityID.Orange;
			item.autoReuse = false;
			item.shootSpeed = 16f;

		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult = 0f;

			base.ModifyManaCost(player, ref reduce, ref mult);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return false;
		}

		public override void UpdateInventory(Player player)
		{
			if (Minigame == null)
			{
				Minigame = new GuitarMinigame(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 120), player, this);
			}

			if (player.inventory[player.selectedItem] != item)
			{
				Minigame.Pause();
			}
			else
			{
				Minigame.Unpause();
			}
			Minigame.Update(SpiritMod.deltaTime);
		}

		public override void HoldItem(Player player) {

			base.HoldItem(player);
		}

		public override Vector2? HoldoutOffset() => new Vector2(-4, -4);

		public override bool PreDrawInInventory(SpriteBatch sB, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) 
		{
			Player owner = Main.player[item.owner];
			if (item.owner == Main.myPlayer && Minigame != null)
			{
				Minigame.Draw(sB);

				if (owner.inventory[owner.selectedItem] == item && Minigame.Combo > 8)
				{
					Texture2D basetexture = SpiritMod.instance.GetTexture("Effects/Masks/Star");

					sB.End();

					sB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);

					float starScale = (Utils.Clamp(Minigame.ComboScale, 0f, 16f) - 8f) / 8f + Minigame.BeatScale;

					float starRot = (float)Main.time * 0.1f + Minigame.BeatScale * 0.5f;

					sB.Draw(basetexture, owner.Center + new Vector2(owner.direction * 4, 4) - Main.screenPosition, null, Color.White * starScale * 0.8f, starRot, basetexture.Size() / 2, starScale * 0.5f, SpriteEffects.None, 0);
					sB.Draw(basetexture, owner.Center + new Vector2(owner.direction * 4, 4) - Main.screenPosition, null, Color.White * starScale * 0.3f, -starRot * 0.8f, basetexture.Size() / 2, starScale * 0.2f, SpriteEffects.None, 0);

					sB.End();

					sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
				}
			}

			return base.PreDrawInInventory(sB, position, frame, drawColor, itemColor, origin, scale);
		}

		public void OnBeat(bool success, int combo)
		{
			Player player = Main.player[item.owner];
			if (success)
			{
				Vector2 mousePos = Main.MouseWorld;
				Vector2 dirToMouse = (mousePos - player.Center);
				dirToMouse.Normalize();

				var proj = Main.projectile[Projectile.NewProjectile(player.Center + dirToMouse * 8, item.shootSpeed * dirToMouse, ModContent.ProjectileType<AnthemNote>(), item.damage, item.knockBack, player.whoAmI)];
				if (proj.modProjectile is AnthemNote note) note.SetUpBeat(Minigame);

				float extraComboScale = Utils.Clamp(Minigame.ComboScale, 0f, 10f) * 0.05f;

				var particle = new AnthemCircle(player.Center - dirToMouse * 4f, dirToMouse * (2f + extraComboScale * 2), 0.1f, 0.25f + extraComboScale, 1f);
				ParticleHandler.SpawnParticle(particle);

				particle = new AnthemCircle(player.Center - dirToMouse * 6f, dirToMouse * (1.5f + extraComboScale * 2), 0.05f, 0.2f + extraComboScale, 0.9f);
				ParticleHandler.SpawnParticle(particle);
			}
			else
			{
				player.statMana -= item.mana;
			}
		}
	}
}
