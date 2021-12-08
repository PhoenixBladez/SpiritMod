using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechStaff
{
	public class GranitechStaffItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Focus M-II");

		public override void SetDefaults()
		{
			item.magic = true;
			item.damage = 90;
			item.Size = new Vector2(76, 80);
			item.useTime = item.useAnimation = 40;
			item.reuseDelay = 20;
			item.knockBack = 9f;
			item.shootSpeed = 9;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.channel = true;
			item.value = Item.sellPrice(0, 1, 32, 0);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.PurificationPowder;
			item.mana = 60;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = ModContent.ProjectileType<GranitechStaffProjectile>();

			Projectile.NewProjectileDirect(position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);
	}
}