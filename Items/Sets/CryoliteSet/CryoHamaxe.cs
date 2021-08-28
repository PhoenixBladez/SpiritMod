using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet
{
	public class CryoHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Hamaxe");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/CryoliteSet/CryoHamaxe_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Orange;

			item.axe = 30;
			item.hammer = 70;

			item.damage = 18;
			item.knockBack = 5;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 27;
			item.useAnimation = 27;

			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Sets/CryoliteSet/CryoHamaxe_Glow"),
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
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 12);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.DungeonSpirit);
			Main.dust[dust].noGravity = true;
		}
	}
}
