using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Yoyo;
using Terraria;
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
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/MoonWizardDrops/Moonburst_Glow");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 17;
			item.value = Item.sellPrice(0, 2, 30, 0);
			item.rare = ItemRarityID.Green;
			item.knockBack = 3.5f;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.useTime = 23;
			item.shoot = ModContent.ProjectileType<MoonburstProj>();
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.1f, .37f, .52f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Sets/MoonWizardDrops/Moonburst_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
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
	}
}
