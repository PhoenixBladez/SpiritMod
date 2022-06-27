using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Yoyo;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MoonWizardDrops
{
	public class Moonburst : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonburst");
			Tooltip.SetDefault("Hitting enemies builds up an unstable bubble\nThe bubble explodes after ten successful strikes");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/MoonWizardDrops/Moonburst_Glow");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 17;
			Item.value = Item.sellPrice(0, 2, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.knockBack = 3.5f;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.useTime = 23;
			Item.shoot = ModContent.ProjectileType<MoonburstProj>();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.1f, .37f, .52f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.GetTexture("Items/Sets/MoonWizardDrops/Moonburst_Glow"),
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		public override void HoldItem(Player player) => player.stringColor = 1 + 3299 - 3293;
	}
}
