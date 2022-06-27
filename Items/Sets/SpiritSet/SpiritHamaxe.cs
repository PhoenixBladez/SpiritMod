using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Hamaxe");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SpiritSet/SpiritHamaxe_Glow");
		}


		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;

			Item.axe = 35;
			Item.hammer = 85;

			Item.damage = 35;
			Item.knockBack = 5;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 24;
			Item.useAnimation = 24;

			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 15);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Sets/SpiritSet/SpiritHamaxe_Glow").Value,
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

		public override void MeleeEffects(Player player, Rectangle hitbox) => Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Flare_Blue);
	}
}
