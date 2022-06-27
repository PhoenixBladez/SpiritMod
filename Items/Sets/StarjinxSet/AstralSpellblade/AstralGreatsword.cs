using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.StarjinxSet.AstralSpellblade
{
    public class AstralGreatsword : ModItem
    {
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Spellblade");
			Tooltip.SetDefault("Does a devastating spin attack after 2 swings\nReleases stars when swung or swinging");
            SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
			Item.Size = new Vector2(60, 60);
            Item.damage = 100;
            Item.useTime = 60;
            Item.useAnimation = 60;
			Item.reuseDelay = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 12;
			Item.channel = true;
			Item.useTurn = false;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<AstralGreatswordHeld>();
            Item.shootSpeed = 1f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => 
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow"), rotation, scale);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			AstralGreatswordPlayer modplayer = player.GetModPlayer<AstralGreatswordPlayer>();
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI, modplayer.Combo);
			modplayer.Combo++;
			modplayer.Combo %= 3;
			return false;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod, "Starjinx", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }

	internal class AstralGreatswordPlayer : ModPlayer
	{
		public int Combo { get; set; } = 1;

		public override void PostUpdate()
		{
			if (Player.HeldItem.type != ModContent.ItemType<AstralGreatsword>()) //Reset when held item changes
				Combo = 1;
		}
	}
}