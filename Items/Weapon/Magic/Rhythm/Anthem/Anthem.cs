using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			item.mana = 10;
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

				Texture2D circle = SpiritMod.instance.GetTexture("Items/Weapon/Magic/Rhythm/Anthem/Circle");
				Vector2 pos = owner.Center - Main.screenPosition;
				float beatScale = (float)Math.Sqrt(Minigame.BeatScale);
				float comboScale = Utils.Clamp(0.1f + Minigame.ComboScale * 0.1f, 0, 1f);
				float alpha = beatScale * (0.3f + 0.3f * comboScale);

				sB.End();

				sB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);

				sB.Draw(circle, pos, null, new Color(255, 0, 0, alpha), 0f, new Vector2(128, 128), 0.1f + (0.3f * comboScale + 0.11f) * (1-beatScale), SpriteEffects.None, 0);
				sB.Draw(circle, pos, null, new Color(0, 255, 0, alpha), 0f, new Vector2(128, 128), 0.1f + (0.3f * comboScale + 0.1f) * (1-beatScale), SpriteEffects.None, 0);
				sB.Draw(circle, pos, null, new Color(0, 0, 255, alpha), 0f, new Vector2(128, 128), 0.1f + (0.3f * comboScale + 0.09f) * (1-beatScale), SpriteEffects.None, 0);


				sB.End();

				sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
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
			}
		}
	}
}
