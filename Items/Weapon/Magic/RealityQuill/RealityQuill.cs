using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Quill");
			Tooltip.SetDefault("Creates a tear in reality, damaging enemies \n Write faster to deal more damage\n'Write your own destiny'");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.knockBack = 0.1f;
			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.channel = true;
			Item.width = 40;
			Item.height = 60;
			Item.mana = 1;
			Item.noMelee = true;
			Item.autoReuse = false;
			Item.channel = true;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.shoot = ModContent.ProjectileType<RealityQuillProjectileTwo>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 mouseDelta = Main.MouseWorld;
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentNebula, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
