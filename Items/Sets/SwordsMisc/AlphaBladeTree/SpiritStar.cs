using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SwordsMisc.AlphaBladeTree
{
	public class SpiritStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Star");
			Tooltip.SetDefault("Rains down multiple starry bolts from the sky that inflict Star Fracture\nThese stars explode into multiple souls that inflict Soul Burn\n'The convergence of souls and the cosmos'");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/SwordsMisc/AlphaBladeTree/SpiritStar_Glow");
		}


		public override void SetDefaults()
		{
			item.damage = 112;
			item.useTime = 17;
			item.useAnimation = 17;
			item.melee = true;
			item.width = 56;
			item.height = 56;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = Terraria.Item.sellPrice(0, 16, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.shootSpeed = 8;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<HarpyFeather>();
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			{
				int dust1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 226);
				Main.dust[dust1].scale *= .23f;

			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/SwordsMiscAlphaBladeTree/SpiritStar_Glow"),
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 3; ++i) {
				if (Main.myPlayer == player.whoAmI) {
					Vector2 mouse = Main.MouseWorld;
					Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(11, 23), ModContent.ProjectileType<Projectiles.SpiritStar>(), damage, knockBack, player.whoAmI);
				}
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Starblade>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.SpiritSet.SpiritSaber>(), 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 15);
			recipe.AddIngredient(ItemID.FragmentSolar, 4);
			recipe.AddIngredient(ItemID.FragmentVortex, 4);
			recipe.AddIngredient(ItemID.FragmentNebula, 4);
			recipe.AddIngredient(ItemID.FragmentStardust, 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}