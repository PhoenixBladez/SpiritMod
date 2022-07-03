using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.JinxprobeWand
{
	public class JinxprobeWand : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jinxprobe Wand");
			Tooltip.SetDefault("Conjures a mini meteorite that orbits you, firing mini bouncing stars at nearby foes");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/StarjinxSet/JinxprobeWand/JinxprobeWand_glow");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SpiderStaff);
			Item.damage = 56;
			Item.Size = new Vector2(36, 52);
			Item.shoot = ModContent.ProjectileType<Jinxprobe>();
			Item.value = Item.sellPrice(gold: 12);
			Item.rare = ItemRarityID.Pink;
            ProjectileID.Sets.MinionTargettingFeature[Item.shoot] = true;
            Item.UseSound = SoundID.Item78;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, Mod.Assets.Request<Texture2D>(Texture.Remove(0, "SpiritMod/".Length) + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
			Projectile.NewProjectileDirect(source, Main.MouseWorld, Vector2.Normalize(Main.MouseWorld - player.Center).RotatedBy(MathHelper.PiOver2) * 10, type, damage, knockback, player.whoAmI, 0);
            return false;
        }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
