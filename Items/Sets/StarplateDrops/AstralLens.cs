using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class AstralLens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Convergence");
			Tooltip.SetDefault("Shoots out bursts of electrical stars that reconverge on the player");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/StarplateDrops/AstralLens_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 24;
			item.magic = true;
			item.mana = 11;
			item.width = 44;
			item.height = 46;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 01, 10, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.NPCDeath7;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Starshock1>();
			item.shootSpeed = 63f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 2; i++)
			{
				Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-300, 300) / 30), speedY + ((float)Main.rand.Next(-300, 300) / 30), type, damage, knockBack, player.whoAmI, 0f, 0f);
				if (Main.rand.Next(6) == 0)
					Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-300, 300) / 30), speedY + ((float)Main.rand.Next(-300, 300) / 30), type, damage, knockBack, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .28f, .38f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/StarplateDrops/AstralLens_Glow"),
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
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 17);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}