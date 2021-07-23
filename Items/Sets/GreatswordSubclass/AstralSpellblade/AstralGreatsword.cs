using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
    public class AstralGreatsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Spellblade");
            SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
			item.Size = new Vector2(60, 60);
            item.damage = 200; //balance d
            item.useTime = 20;
            item.useAnimation = 20;
			item.reuseDelay = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
			item.channel = true;
			item.useTurn = false;
            item.value = Item.sellPrice(0, 0, 1, 0);
            item.rare = ItemRarityID.Pink;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<AstralGreatswordHeld>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => 
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			GreatswordPlayer modplayer = player.GetModPlayer<GreatswordPlayer>();
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0, modplayer.Combo);
			modplayer.Combo++;
			modplayer.Combo %= 3;
			return false;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "Starjinx", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}