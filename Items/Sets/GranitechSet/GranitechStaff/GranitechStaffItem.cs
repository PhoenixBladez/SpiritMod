using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechStaff
{
	public class GranitechStaffItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Focus M-II");

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Magic;
			Item.damage = 90;
			Item.Size = new Vector2(76, 80);
			Item.useTime = Item.useAnimation = 40;
			Item.reuseDelay = 20;
			Item.knockBack = 9f;
			Item.shootSpeed = 9;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.channel = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.mana = 60;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			type = ModContent.ProjectileType<GranitechStaffProjectile>();
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/FocusCharge") with { PitchVariance = 0.2f }, player.Center);

			Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<GranitechMaterial>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}