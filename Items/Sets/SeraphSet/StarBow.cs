using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet
{
	public class StarBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Storm");
			Tooltip.SetDefault("Launches bolts of sporadic lunar energy");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/SeraphSet/StarBow_Glow");

		}


		//private Vector2 newVect;
		public override void SetDefaults()
		{
			item.damage = 28;
            item.width = 22;
            item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.knockBack = 4;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 13;
			item.useAnimation = 13;

			item.useAmmo = AmmoID.Arrow;

			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;

			item.shoot = ModContent.ProjectileType<SleepingStar>();
			item.shootSpeed = 9;

			item.UseSound = SoundID.Item5;
		}

		/*    public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
             {
                 if(Main.myPlayer == player.whoAmI) {
                     Vector2 mouse = Main.MouseWorld;
                      for (int i = 0; i < 3; ++i)
                 {
                     Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 550 + Main.rand.Next(-50, 50), 0, Main.rand.Next(14,18), ModContent.ProjectileType<StarBolt>(), damage, knockBack, player.whoAmI);
                 }
                 }
                 return false;
             }*/

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int i = 1; i <= 2; i++)
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SleepingStar>(), damage, knockBack, player.whoAmI, i);

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/SeraphSet/StarBow_Glow"),
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}