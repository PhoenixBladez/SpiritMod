using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.JinxprobeWand
{
	public class JinxprobeWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jinxprobe Wand");
			Tooltip.SetDefault("Conjures a mini meteorite that orbits you, firing mini bouncing stars at nearby foes");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/StarjinxSet/JinxprobeWand/JinxprobeWand_glow");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.SpiderStaff);
			item.damage = 56;
			item.Size = new Vector2(36, 52);
			item.shoot = mod.ProjectileType("Jinxprobe");
			item.value = Item.sellPrice(gold: 12);
			item.rare = ItemRarityID.Pink;
            ProjectileID.Sets.MinionTargettingFeature[item.shoot] = true;
            item.UseSound = SoundID.Item78;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, mod.GetTexture(Texture.Remove(0, "SpiritMod/".Length) + "_glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectileDirect(Main.MouseWorld, Vector2.Normalize(Main.MouseWorld - player.Center).RotatedBy(MathHelper.PiOver2) * 10, type, damage, knockBack, player.whoAmI, 0);

            return false;
        }

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
