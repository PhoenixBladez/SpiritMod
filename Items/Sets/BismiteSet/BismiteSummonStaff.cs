using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteSummonStaff : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bismite Crystal Staff");
            Tooltip.SetDefault("Summons a stationary Bismite Crystal that shoots poison shards at enemies\nRight-click to cause Bismite Crystals to emit a festering wave at the cost of mana");
        }

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.QueenSpiderStaff);
			Item.damage = 7;
			Item.mana = 10;
			Item.width = 50;
			Item.height = 50;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.knockBack = 2.5f;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<BismiteSentrySummon>();
			Item.shootSpeed = 0f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            if (player.altFunctionUse != 2)
            {
                Vector2 mouse = Main.MouseWorld;
                float distance = Vector2.Distance(mouse, position);
                if (distance < 600f)
                {
                    Projectile.NewProjectile(source, mouse.X, mouse.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
                    player.UpdateMaxTurrets();
                }
            }
            else
            {
                for (int projectileFinder = 0; projectileFinder < 200; ++projectileFinder)
                {
					Projectile proj = Main.projectile[projectileFinder];
					if (proj.type == Item.shoot && proj.alpha == 0)
					{
						proj.alpha = BismiteSentrySummon.BurstAlpha;
						(proj.ModProjectile as BismiteSentrySummon).SpecialAttack();
					}
                }
            }
            return false;
		}

        public override void AddRecipes()  
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
