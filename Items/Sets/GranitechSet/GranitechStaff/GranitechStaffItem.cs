using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechStaff
{
	public class GranitechStaffItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granitech Staff");

		public override void SetDefaults()
		{
			item.damage = 117;
			item.Size = new Vector2(54, 56);
			item.useTime = item.useAnimation = 40;
			item.reuseDelay = 20;
			item.knockBack = 0f;
			item.shootSpeed = 9;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.ranged = true;
			item.channel = true;
			item.value = Item.sellPrice(0, 1, 32, 0);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.PurificationPowder;
			item.mana = 1;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = ModContent.ProjectileType<GranitechStaffProjectile>();

			Projectile.NewProjectileDirect(position, Vector2.Zero, type, damage, 0, player.whoAmI);
			return false;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult = 0; //Dont actually spend mana, due to hacky mana display
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			for(int i = 0; i < tooltips.Count; i++)
			{
				if (tooltips[i].Name == "UseMana")
					tooltips[i].text = Language.GetText("Uses").ToString() + " " + (int)(GranitechStaffProjectile.BASE_MANA_COST * Main.LocalPlayer.manaCost) + " " + Language.GetText("Mana").ToString();
			}
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);
	}
}